using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Melee, Range
}

public class Enemy : LivingEntity
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float enemyRange = 20f;
    [SerializeField] private float searchCoolTime = 0.25f;
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float startTimeBtwShots = 2f;
    [SerializeField] private EnemyType enemyType;
    //[SerializeField] private float enemyHealth = 10f;
    [SerializeField] private GameObject projectile;

    public EnemyScriptableObject enemyScriptableObject;


    private float timeBtwShots;
    private float lastAttackTime;
    private Animator animator;
    private Vector2 direction;
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;
    private Rigidbody2D rgbd;
    private float maxHealth;
    private bool corrosionDeath = false;
    private bool iceSecondUpgrade = false;

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
        if(corrosionDeath)
        {
            if (dotTickTimers.Count <= 4)
                corrosionDeath = false;
        }

        if (dotTickTimers.Count <= 0)
            enemySpeed = normalEnemySpeed;

        if(gameObject.name=="R(Clone)" || gameObject.name == "M(Clone)")
        {
            SetDirection();
        }
        //나중에 추가된 Enemy들에 해당
        else
        {
            SetDirection_2();
        }

        switch (enemyType)
        {
            case EnemyType.Melee:
                break;
            case EnemyType.Range:
                if (hasTarget)
                {
                    timeBtwShots -= Time.deltaTime;
                    if(timeBtwShots <= 0f)
                    {
                        Shoot();
                    }
                }
                break;
        }

        pathFinder.speed = enemySpeed;

    }
        

    public void SetUp(float newHealth, float newSpeed)
    {
        currentHealth = newHealth;
        maxHealth = newHealth;
        pathFinder.speed = newSpeed;
        enemySpeed = newSpeed;
        normalEnemySpeed = newSpeed;
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

    private void SetDirection_2()
    {
        if(targetEntity!=null)
        {
            direction = (targetEntity.transform.position - transform.position).normalized;

            if (direction.x >= 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                if(gameObject.name!= "Turret Robot(Clone)")
                {
                    pathFinder.SetDestination(targetEntity.transform.position);
                }
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
        //StartCoroutine(enemyStayOnPosition());
        animator.SetBool("Attack", true);
        Instantiate(projectile, transform.position, Quaternion.identity);
        animator.SetBool("Attack", false);

        timeBtwShots = startTimeBtwShots;
    }

    private IEnumerator enemyStayOnPosition()
    {
        pathFinder.isStopped = true;
        pathFinder.speed = 0;
        yield return new WaitForSeconds(1f);
        pathFinder.isStopped = false;
        pathFinder.speed = enemySpeed;
    }

    public void EnemyRestraint(float time)
    {
        StartCoroutine(Restraint(time));
    }

    public IEnumerator Restraint(float time)
    {
        if(!isStun && pathFinder.isOnNavMesh)
        {
            Debug.Log("CONSTRAINT");
            isStun = true;
            GameObject iceLock = Instantiate(enemyScriptableObject.iceLock, transform.position, Quaternion.identity);
            iceLock.transform.parent = this.transform;
            //속박 수정해야함~
            pathFinder.isStopped = true;
            yield return new WaitForSeconds(time);
            pathFinder.isStopped = false;
            isStun = false;
            if (iceSecondUpgrade)
                OnDamage(enemyScriptableObject.enemyHealth * 0.1f);

            if(iceLock)
            {
                Destroy(iceLock);
            }
        }
    }
    
    

    private IEnumerator HurtSpriteChanger()
    {
        if(enemySpeed >= normalEnemySpeed)
        {
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = new Color(1, 0, 0, 0.5f);
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            }
        }
     
    }

    public void EnemyKnockBack()
    {
        StartCoroutine(IEnemyKnockBack());
    }

    private IEnumerator IEnemyKnockBack()
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

    
    public override void ApplyBurn(int ticks,int maxTicks, float tickDamage)
    {
        if(dotTickTimers.Count<=maxTicks)
        {
            if (dotTickTimers.Count <= 0)
            {
                dotTickTimers.Add(ticks);
                GameObject fireBurst = Instantiate(enemyScriptableObject.fireBurst, transform.position, Quaternion.identity);
                fireBurst.transform.SetParent(this.transform);
                Debug.Log($"{tickDamage} : fire damage)");

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

    public override void ApplyIce(float slowDownSpeed, bool enabledSecondUpgrade, bool enabledThirdUpgrade)
    {
        if(enemySpeed >= normalEnemySpeed)
            enemySpeed *= slowDownSpeed;

        if (enabledSecondUpgrade)
            iceSecondUpgrade = true;

        

        // 10%로 속박
        if (UnityEngine.Random.Range(0, 10) == 0)
            EnemyRestraint(2f);

        if(dotTickTimers.Count <= 10)
        {
            if (dotTickTimers.Count <= 0)
            {
                dotTickTimers.Add(30);
                //ice의 경우 type 는 1이다.
                StartCoroutine(DOTApply(0, 1));
                //GameObject iceLock = Instantiate(enemyScriptableObject.iceLock, transform.position, Quaternion.identity);
                //iceLock.transform.SetParent(this.transform);
            }
            else
            {
                dotTickTimers.Add(10);
            }
        }
        

    }

    public override IEnumerator DOTApply(float tickDamage, int type)
    {
        return base.DOTApply(tickDamage, type);
    }


    public override void ApplyCorrosion(int ticks, int maxTicks, float tickDamage, bool fourthUpgrade)
    {
        if (fourthUpgrade)
            corrosionDeath = true;
        if (dotTickTimers.Count <= maxTicks)
        {
            if (dotTickTimers.Count <= 0)
            {
                dotTickTimers.Add(ticks);
                GameObject corrosion = Instantiate(enemyScriptableObject.corrosion, transform.position, Quaternion.identity);
                corrosion.transform.SetParent(this.transform);
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
            //Debug.Log(this.name + " HP: " + currentHealth);
            //hurt audio
            //hurt particle effect
        }

        base.OnDamage(damage);
    }



    public override void Die()
    {
        if(corrosionDeath == true && dotTickTimers.Count >= 5)
        {
            //폭발 일어나기~
            Transform _corrosionExplosion = Instantiate(enemyScriptableObject.corrosionExplosion);
        }
        if(isStun)
        {
            if (!pathFinder.isStopped)
                pathFinder.isStopped = true;
            for (var i = this.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        base.Die();
        animator.SetTrigger("Death");

        /*Collider2D[] enemyColliders = GetComponents<Collider2D>();

        for(int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }*/

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

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
