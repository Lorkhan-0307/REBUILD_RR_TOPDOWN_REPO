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
    [SerializeField] private GameObject LaserEffect;
    [SerializeField] private GameObject LaserArm;
    [SerializeField] private float bossSpeed = 5f;
    [SerializeField] private int maxAmmo = 5;
    [SerializeField] private int maxRush=3;
    [SerializeField] private Transform firePoint;

    private Rigidbody2D rb;
    private float laserAttackDuration = 5f;
    private float laserAttackTime = 5f;
    private float lastAttackTime;
    private float timeBtwShots;
    private float enemySpeed;
    private int currentAmmo;
    private int rushCnt = 0;
    public bool canLaserAttack;
    public bool canRangeAttack;
    private bool isRushing;
    public bool isArmFlipped = false;


    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 direction;
    private Vector2 armDirection;
    private Vector3 firePointVector;
    private NavMeshAgent pathFinder;
    private LivingEntity targetEntity;
    

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
        rb = GetComponent<Rigidbody2D>();
        pathFinder.speed = bossSpeed;
        //pathFinder.stoppingDistance = 4f;
        LaserArm.SetActive(false);
        canLaserAttack=true;
        canRangeAttack = true;
        currentAmmo = maxAmmo;
        rushCnt = maxRush;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeBtwShots = startTimeBtwShots;
        //StartCoroutine(UpdatePath());
        //StartCoroutine(LaserShoot());
        //StartCoroutine(LaserOff());
    }

    // Update is called once per frame
    void Update()
    {
        //if Rush Attack, do not set direction of boss
        if (!isRushing)
        {
            SetDirection();
        }

        firePointVector = new Vector3(firePoint.position.x, firePoint.position.y, 0f);
        //Debug.Log(ammo);
        //Debug.Log(pathFinder.speed);
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

    public IEnumerator UpdatePath()
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


    public IEnumerator LaserShoot()
    {
        if (hasTarget && canLaserAttack)
        {
            //play bigboy shoot sound
            LaserArm.SetActive(true);
            yield return new WaitForSeconds(5f);
            if (LaserEffect.activeInHierarchy)
            {
                LaserArm.SetActive(false);
                animator.SetBool("isWalking", true);
                animator.SetBool("isLaserAttack", false);
                canLaserAttack = false;
            }
        }
    }

    public IEnumerator LaserOff()
    {
        while (true)
        {
            yield return null;
            if (LaserEffect.activeInHierarchy)
            {
                laserAttackDuration -= Time.deltaTime;
                if (laserAttackDuration <= 0)
                {
                    laserAttackDuration = laserAttackTime;
                    LaserArm.SetActive(false);
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isLaserAttack", false);
                    canLaserAttack = false;
                }
            }
        }
    }


    public void RangeAttack()
    {
        /*if (timeBtwShots <= 0)
        {
            Debug.Log("range attack");
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
            ammo--;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }*/

        if (currentAmmo > 0)
        {
            //StartCoroutine(bossStayOnPosition());
            Instantiate(projectile, firePointVector, Quaternion.identity);
            currentAmmo--;
        }
        else
        {
            canRangeAttack = false;
            animator.SetBool("isRushing", true);
            animator.SetBool("isRangeAttack", false);
        }

    }

    public IEnumerator RushAttack()
    {
        while (rushCnt > 0)
        {
            isRushing = true;
            pathFinder.isStopped = true;
            SetDirection();
            //boss charge animation 추가
            yield return new WaitForSeconds(1f);

            /*
            pathFinder.stoppingDistance = 0f;
            pathFinder.SetDestination(targetEntity.transform.position);

            pathFinder.isStopped = false;
            pathFinder.speed = 8000f;
            */

            rb.AddForce(direction * 30f, ForceMode2D.Impulse); //force 변수 설정

            yield return new WaitForSeconds(0.75f); //다시 살펴봐야함

            rb.velocity = Vector2.zero;
            rushCnt--;

        }
        
        animator.SetBool("isLaserAttack", true);
        animator.SetBool("isRushing", false);
        rushCnt = maxRush;
        canLaserAttack = true;
        isRushing = false;
        pathFinder.isStopped = false;
        pathFinder.speed = bossSpeed;
        pathFinder.stoppingDistance = 4f;

        yield return null;

    }

    private void RangeAttackTrigger()
    {
        currentAmmo = maxAmmo;
        canRangeAttack = true;
    }

    private void LaserAttackTrigger()
    {
        canLaserAttack = true;
    }


    private IEnumerator bossStayOnPosition(float x)
    {
        //pathFinder.speed = 0;
        pathFinder.isStopped = true;
        yield return new WaitForSeconds(x);
        pathFinder.isStopped = false;
        //pathFinder.speed = bossSpeed;
        
    }

    private void BossStopTrigger()
    {
        pathFinder.isStopped = true;
        pathFinder.speed = 0;
    }

    private void BossMoveTrigger()
    {
        pathFinder.isStopped = false;
        pathFinder.speed = bossSpeed;
    }

    public void SetArmPosition()
    {
        if (targetEntity != null && LaserArm.activeSelf)
        {
            armDirection = (targetEntity.transform.position - LaserArm.transform.position).normalized;

            if (armDirection.x >= 0)
            {
                isArmFlipped = false;
                LaserArm.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                isArmFlipped = true;
                LaserArm.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
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

    //player가 부딪쳤을때
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

    //벽에 박을때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
        }
    }
}
