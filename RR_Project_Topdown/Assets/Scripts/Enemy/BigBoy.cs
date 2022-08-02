using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBoy : LivingEntity
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float enemyRange = 20f;
    [SerializeField] private float searchCoolTime = 0.25f;
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float startTimeBtwShots = 2f;
    [SerializeField] private float enemyHealth = 10f;
    [SerializeField] private GameObject projectile;

    private float timeBtwShots;
    private float lastAttackTime;
    private float enemySpeed;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 direction;
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;


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
        currentHealth = enemyHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemySpeed = pathFinder.speed;
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
        if (gameObject.name == "R(Clone)" && hasTarget)
        {
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
                animator.SetBool("isWalking", true);
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                animator.SetBool("isWalking", false);
                pathFinder.isStopped = true;

                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, enemyRange, whatIsTarget);

                for (int i = 0; i < collider2Ds.Length; i++)
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

    public override void OnDamage(float damage)
    {
        if (!dead)
        {
            //hurt animation
            //hurt audio
            //hurt particle effect
        }

        base.OnDamage(damage);
    }

    public override void Die()
    {
        base.Die();

        Collider2D[] enemyColliders = GetComponents<Collider2D>();

        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        //dead animation
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
