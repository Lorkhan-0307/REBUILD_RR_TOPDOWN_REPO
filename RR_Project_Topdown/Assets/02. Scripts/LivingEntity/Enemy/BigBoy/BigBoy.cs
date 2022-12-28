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
    [SerializeField] private float bossHealth = 10f;
    [SerializeField] private float bossSpeed = 5f;
    [SerializeField] private float rushSpeed = 30f;
    [SerializeField] private float ghostDelay;
    [SerializeField] private GameObject projectile;
    [SerializeField] public GameObject LaserArm;
    [SerializeField] private GameObject LaserArm_2;
    [SerializeField] private GameObject LaserArm_3;
    [SerializeField] private GameObject LaserEffect;
    [SerializeField] private GameObject LaserEffect_2;
    [SerializeField] private GameObject LaserEffect_3;
    [SerializeField] private GameObject Ghost;
    [SerializeField] private int maxAmmo = 5;
    [SerializeField] private int maxRush=3;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int fireShot = 5;
    [SerializeField] private float bulletSpeed = 10f;


    [SerializeField] EnemyScriptableObject enemyScriptableObject;

    private Rigidbody2D rb;
    //private float laserAttackDuration = 5f;
    //private float laserAttackTime = 5f;
    private float lastAttackTime;
    private float ghostDelaySeconds;
    //private float enemySpeed;
    private int currentAmmo;
    private int rushCnt = 0;
    private int phase = 1;
    public bool isRushing;
    public bool canLaserAttack;
    public bool canRangeAttack;
    public bool isArmFlipped = false;
    public bool changePhase;


    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 direction;
    private Vector2 armDirection;
    private Vector3 firePointVector;
    private NavMeshAgent pathFinder;
    private LivingEntity targetEntity;
    private float maxHealth;
    float angle = 0f;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 0.4f;
    [SerializeField] private int numberOfFlashes = 2;


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
        currentHealth = bossHealth;
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
        ghostDelaySeconds = ghostDelay;
        dotTickTimers = new List<int>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentHealth);
        //if Rush Attack, do not set direction of boss
        if (!isRushing)
        {
            SetDirection();
        }
        else
        {
            UpdateGhostDelay();
        }
        firePointVector = new Vector3(firePoint.position.x, firePoint.position.y, 0f);

        //Debug.Log(currentHealth);
    }


    /*public void SetUp(float newHealth, float newSpeed)
    {
        currentHealth = newHealth;
        maxHealth = newHealth;
        pathFinder.speed = newSpeed;
        enemySpeed = normalEnemySpeed = newSpeed;
    }*/

    #region Attack Pattern

    private void UpdateGhostDelay()
    {
        if(ghostDelaySeconds > 0)
        {
            ghostDelaySeconds -= Time.deltaTime;
        }
        else
        {
            GameObject currentGhost = Instantiate(Ghost, transform.position, transform.rotation);
            Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
            currentGhost.transform.localScale = this.transform.localScale;
            currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
            ghostDelaySeconds = ghostDelay;
            Destroy(currentGhost, 1f);
        }
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
            if(phase == 1)
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
            else if(phase == 2)
            {
                //play bigboy shoot sound
                LaserArm.SetActive(true);
                LaserArm_2.SetActive(true);
                LaserArm_3.SetActive(true);
                yield return new WaitForSeconds(5f);

                if (LaserEffect.activeInHierarchy && LaserEffect_2.activeInHierarchy && LaserEffect_3.activeInHierarchy)
                {
                    LaserArm.SetActive(false);
                    LaserArm_2.SetActive(false);
                    LaserArm_3.SetActive(false);
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isLaserAttack", false);
                    canLaserAttack = false;
                }


            }

            //play bigboy shoot sound
            /*LaserArm.SetActive(true);
            LaserArm_2.SetActive(true);
            LaserArm_3.SetActive(true);
            yield return new WaitForSeconds(5f);
            if (phase == 1)
            {
                if (LaserEffect.activeInHierarchy)
                {
                    LaserArm.SetActive(false);
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isLaserAttack", false);
                    canLaserAttack = false;
                }
            }
            else if (phase == 2)
            {
                if (LaserEffect.activeInHierarchy && LaserEffect_2.activeInHierarchy)
                {
                    LaserArm.SetActive(false);
                    LaserArm_2.SetActive(false);
                    LaserArm_3.SetActive(false);
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isLaserAttack", false);
                    canLaserAttack = false;
                }
            }*/
        }
    }

    /*public IEnumerator LaserOff()
    {
        while (true)
        {
            yield return null;
            if (phase == 1)
            {
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
            else if (phase == 2)
            {
                if (LaserEffect.activeInHierarchy && LaserEffect_2.activeInHierarchy)
                {
                    laserAttackDuration -= Time.deltaTime;
                    if (laserAttackDuration <= 0)
                    {
                        laserAttackDuration = laserAttackTime;
                        LaserArm.SetActive(false);
                        LaserArm_2.SetActive(false);
                        LaserArm_3.SetActive(false);
                        animator.SetBool("isWalking", true);
                        animator.SetBool("isLaserAttack", false);
                        canLaserAttack = false;
                    }
                }
            }
        }
    }*/


    public void RangeAttack()
    {
        if (currentAmmo > 0)
        {
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

    public void RangeShotAttack()
    {
        /*if (currentAmmo > 0)
        {
            for(int i = 0; i < fireShot; i++)
            {
                GameObject bullet = GameObject.Instantiate(projectile_2);
                bullet.transform.position = transform.position;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 dirVec = direction;
                Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f));
                dirVec += ranVec;
                rb.AddForce(dirVec.normalized * bulletSpeed, ForceMode2D.Impulse);
            }
            currentAmmo--;
        }*/

        if (currentAmmo > 0)
        {

            for(int i = 0; i < fireShot; i++)
            {
                GameObject bullet = GameObject.Instantiate(projectile);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos((Mathf.PI * 2 * i / fireShot) + angle), Mathf.Sin((Mathf.PI * 2 * i / fireShot) + angle));
                rb.AddForce(dirVec.normalized * bulletSpeed, ForceMode2D.Impulse);

                Vector3 rotVec = Vector3.forward * 360 * i / fireShot + Vector3.forward * 90;
                bullet.transform.Rotate(rotVec);
            }
            angle += 30f;
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

            //TEST CODE
            //if(!dead) pathFinder.isStopped = true;

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

            rb.AddForce(direction * rushSpeed, ForceMode2D.Impulse); //force 변수 설정

            yield return new WaitForSeconds(0.75f); //다시 살펴봐야함

            rb.velocity = Vector2.zero;
            rushCnt--;

        }
        
        animator.SetBool("isLaserAttack", true);
        animator.SetBool("isRushing", false);
        rushCnt = maxRush;
        canLaserAttack = true;
        isRushing = false;
        if(!dead) pathFinder.isStopped = false;
        pathFinder.speed = bossSpeed;
        pathFinder.stoppingDistance = 4f;

        yield return null;

    }

    #endregion

    #region Animation Trigger
    private void RangeAttackTrigger()
    {
        currentAmmo = maxAmmo;
        canRangeAttack = true;
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

    private void BossStopRBTrigger()
    {
        rb.velocity = Vector2.zero;
    }

    private void BossWalkTrigger()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("changePhase", false);
    }

    #endregion

    #region IDamageable Interfaces
    private IEnumerator HurtSpriteChanger()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
    }


    // Apply 시리즈가 보스에 따로 있는 이유 : 보스의 경우 틱데미지의 감소를 넣을까 함.
    public override void ApplyBurn(int ticks, int maxTicks, float tickDamage)
    {
        base.ApplyBurn(ticks,maxTicks, tickDamage);
        if (dotTickTimers.Count <= 11)
        {
            if (dotTickTimers.Count <= 0)
            {
                dotTickTimers.Add(ticks);
                GameObject fireBurst = Instantiate(enemyScriptableObject.fireBurst, transform.position, Quaternion.identity);
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

    public override void ApplyCorrosion(int ticks, int maxTicks, float tickDamage, bool fourthUpgrade)
    {
        base.ApplyCorrosion(ticks,maxTicks, tickDamage, fourthUpgrade);
        if (dotTickTimers.Count <= 11)
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

    public override void ApplyIce(float slowDownSpeed, bool enabledSecondUpgrade, bool enabledThirdUpgrade)
    {
        enemySpeed *= slowDownSpeed;
        if (dotTickTimers.Count <= 0)
        {
            dotTickTimers.Add(30);
            //ice의 경우 type 는 1이다.
            StartCoroutine(DOTApply(0, 1));
            GameObject iceLock = Instantiate(enemyScriptableObject.iceLock, transform.position, Quaternion.identity);
            iceLock.transform.SetParent(this.transform);
            if (dotTickTimers.Count >= 5)
            {
                if (enabledThirdUpgrade)
                {
                    OnDamage(maxHealth * 0.1f);
                }
                //Freeze
                dotTickTimers.Clear();
            }
        }
        else
        {
            dotTickTimers.Add(10);
        }

    }

    #endregion

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
        if(phase == 1)
        {
            //increase rush cnt
            maxRush++; // rush count to 4
            //increase ammo
            maxAmmo *= 2;
            //phase change animation
            changePhase = true;
            //Reset Boss Health
            bossHealth *= 2;
            currentHealth = bossHealth;
            //increase phase
            phase++;
        }

        else if (phase == 2)
        {
            Collider2D[] enemyColliders = GetComponents<Collider2D>();

            // 보스 죽는거 추가해주세용 :ㅇ

            //dead audio
            //dead animation
            //dead audio
            base.Die();
            animator.SetBool("isDead", true);
            Destroy(gameObject, 2f);

            for (int i = 0; i < enemyColliders.Length; i++)
            {
                enemyColliders[i].enabled = false;
            }
        
            pathFinder.isStopped = true;
            pathFinder.enabled = false;

            //dead animation
            //dead audio

        }

    }

    #region OnTrigger Function
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
    #endregion
}
