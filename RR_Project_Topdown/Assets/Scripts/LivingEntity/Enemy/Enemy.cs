using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float enemyRange = 20f;
    [SerializeField] private float searchCoolTime = 0.25f;
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float startTimeBtwShots = 2f;
    //[SerializeField] private float enemyHealth = 10f;
    [SerializeField] private GameObject projectile;

    [SerializeField] private EnemyScriptableObject enemyScriptableObject;



    private float timeBtwShots;
    private float lastAttackTime;
    private float enemySpeed;
    private Animator animator;
    private Vector2 direction;
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;
    private Rigidbody2D rgbd;

    //도트뎀 사용을 위한 변수들 모음
    
    
    


    //For Flashing Sprite On Damage
    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 0.5f;
    [SerializeField] private int numberOfFlashes = 2;
    private SpriteRenderer spriteRenderer;


    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        pathFinder.updateRotation = false;
        pathFinder.updateUpAxis = false;
        //currentHealth = enemyHealth;
        animator = GetComponent<Animator>();
        enemySpeed = pathFinder.speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();

        dotTickTimers = new List<int>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timeBtwShots = startTimeBtwShots;
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.name);
        SetDirection();
        if (gameObject.name == "R(Clone)" && hasTarget){
            Shoot();
        }
    }

    public void SetUp(float newHealth, float newSpeed)
    {
        currentHealth = newHealth;
        pathFinder.speed = newSpeed;
    }

    private void SetDirection()
    {
        if (targetEntity != null)
        {
            direction = (targetEntity.transform.position - transform.position).normalized;
            
            if(direction.y >= direction.x)
            {
                if (direction.y >= direction.x * (-1))
                {
                    animator.SetFloat("x", 0);
                    animator.SetFloat("y", 1);
                }
                else
                {
                    animator.SetFloat("x", -1);
                    animator.SetFloat("y", 0);
                }
            }
            else
            {
                if (direction.y >= direction.x * (-1))
                {
                    animator.SetFloat("x", 1);
                    animator.SetFloat("y", 0);
                }
                else
                {
                    animator.SetFloat("x", 0);
                    animator.SetFloat("y", -1);
                }
            }
        }
        else
        {
            animator.SetFloat("x", 0);
            animator.SetFloat("y", -1);
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;

                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, enemyRange, whatIsTarget);

                for(int i = 0; i < collider2Ds.Length; i++)
                {
                    LivingEntity livingEntity = collider2Ds[i].GetComponent<LivingEntity>();

                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(searchCoolTime);
        }
    }


    private void Shoot()
    {
        if (timeBtwShots <= 0)
        {
            StartCoroutine(enemyStayOnPosition());
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    private IEnumerator enemyStayOnPosition()
    {
        pathFinder.speed = 0;
        yield return new WaitForSeconds(1f);
        pathFinder.speed = enemySpeed;
    }

    public override IEnumerator Restraint(float time)
    {
        if(!isStun)
        {
            isStun = true;
            GameObject iceLock = Instantiate(enemyScriptableObject.IceLock, transform.position, Quaternion.identity);
            iceLock.transform.parent = this.transform;
            pathFinder.speed = 0;
            yield return new WaitForSeconds(time);
            pathFinder.speed = enemySpeed;
            isStun = false;
            if(iceLock)
            {
                Destroy(iceLock);
            }
            
        }
        
    }
    


    private IEnumerator HurtSpriteChanger()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
        }
    }
    
    

    private IEnumerator EnemyKnockBack()
    {
        if(!isKnockback)
        {
            isKnockback = true;

            rgbd.isKinematic = false;
            pathFinder.speed = 0;
            Vector3 diff = this.transform.position - targetEntity.transform.position;
            diff = diff.normalized * enemyScriptableObject.fKnockbackMultiplier;
            rgbd.AddForce(diff, ForceMode2D.Impulse);
            yield return new WaitForSeconds(enemyScriptableObject.fKnockbackDuration);
            rgbd.velocity = Vector2.zero;
            pathFinder.speed = enemySpeed;
            rgbd.isKinematic = true;

            isKnockback = false;
        }
    }

    

    //도트뎀 기능

    
    public override void ApplyBurn(int ticks, float tickDamage)
    {
        if(dotTickTimers.Count<=11)
        {
            if (dotTickTimers.Count <= 0)
            {
                dotTickTimers.Add(ticks);
                GameObject fireBurst = Instantiate(enemyScriptableObject.FireBurst, transform.position, Quaternion.identity);
                fireBurst.transform.SetParent(this.transform);
                //Burn의 경우 type 는 0이다.
                StartCoroutine(DOTApply(tickDamage, 0));
                if (dotTickTimers.Count >= 5)
                {
                    //Explosion
                }
            }
            else
            {
                dotTickTimers.Add(ticks);
            }
        }
        
        
    }

    public override void ApplyIce()
    {

         if (dotTickTimers.Count <= 0)
         {
             dotTickTimers.Add(10);
            //ice의 경우 type 는 1이다.
            StartCoroutine(DOTApply(0, 1));
             if (dotTickTimers.Count >= 5)
             {
                //Freeze
                dotTickTimers.Clear();
            }
         }
         else
         {
             dotTickTimers.Add(10);
         }

    }

    public override void ApplyCorrosion(int ticks, float tickDamage)
    {
        if (dotTickTimers.Count <= 11)
        {
            if (dotTickTimers.Count <= 0)
            {
                dotTickTimers.Add(ticks);
                //corrosion의 경우 type 는 2이다.
                StartCoroutine(DOTApply(tickDamage, 2));
            }
            else
            {
                dotTickTimers.Add(ticks);
            }
        }
    }


    public override void OnDamage(float damage)
    {
        if (!dead)
        {
            //hurt animation
            StartCoroutine(HurtSpriteChanger());
            //hurt audio
            //hurt particle effect
        }

        base.OnDamage(damage);
    }



    public override void Die()
    {
        
        if(isStun)
        {
            for (var i = this.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        Collider2D[] enemyColliders = GetComponents<Collider2D>();

        for(int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        animator.SetTrigger("Death");
        base.Die();
        //dead audio
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!dead && Time.time >= lastAttackTime + attackCoolTime)
        {
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;

                attackTarget.OnDamage(damage);
            }
        }
    }


}
