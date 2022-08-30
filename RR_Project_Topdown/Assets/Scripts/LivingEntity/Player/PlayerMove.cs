using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;



[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMove : MonoBehaviour
{


    Rigidbody2D rgbd2d;
    Animate animate;
    Vector3 movementVector;

    private enum State
    {
        Normal,
        SkillActive,
        SkillDisactive,
        Hurt,
        Death,
    }

    public enum Element
    {
        //물리
        Physical,
        //화염
        Fire,
        //얼음
        Ice,
        //부식
        Corrosion
    }

    #region Variables
    [SerializeField] private HealthBar healthBar;
    [SerializeField] public GameObject rangeAttackObject;
    [SerializeField] public GameObject cm;
    [SerializeField] public GameObject skillActiveScreen;
    [SerializeField] public GameObject skillBar;
    [SerializeField] public GameObject skillFX;
    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    [SerializeField] public float attackCooldownTime = 0.5f;
    private float attackCooldownTimer;

    private PlayerPlugIn playerPlugIn;

    private State state;
    public Element currentElement;
    private float currentMovementSpeed;
    private float rangeAttackTimer;
    private float skillBarTimer = 0.1f;
    private bool skillActivated = false;
    

    #endregion

    private void Awake()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
        animate = GetComponent<Animate>();
        playerPlugIn = new PlayerPlugIn();
        playerPlugIn.OnPlugInUnlocked += PlayerPlugIn_OnPlugInUnlocked;
        state = State.Normal;
        DisableSkillActive();
        skillBar.GetComponent<SkillBar>().SetMaxSkill(playerScriptableObject.skillBarMax);
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = false;
        attackCooldownTimer = attackCooldownTime;
        currentMovementSpeed = playerScriptableObject.movementSpeed;

        //초기 속성은 물리로 설정
        currentElement = Element.Physical;

        EnableElementAttack(Element.Fire);


    }

    #region Manage PlugInTree
    private void PlayerPlugIn_OnPlugInUnlocked(object sender, PlayerPlugIn.OnPlugInUnlockedEventArgs e)
    {
        switch (e.plugInType)
        {
            //단순 데미지 증가 15%
            case PlayerPlugIn.PlugInType.GauntletAttack_1:
                UpgradeAttackDamage(1.15f);
                break;
            //데미지 30% 증가, 공격속도 10% 감소
            case PlayerPlugIn.PlugInType.GauntletAttack_2:
                UpgradeAttackSpeed(0.9f);
                UpgradeAttackDamage(1.3f);
                break;
            //공격속도 15% 증가, 데미지 25% 감소
            case PlayerPlugIn.PlugInType.GauntletAttack_3:
                UpgradeAttackSpeed(1.15f);
                UpgradeAttackDamage(0.75f);
                //Gauntlet++
                break;
            //공격력 50% 증가, 범위증가 OR 공격속도 10% 증가[고민중]
            case PlayerPlugIn.PlugInType.GauntletAttack_4:
                UpgradeAttackSpeed(1.1f);
                UpgradeAttackDamage(1.5f);
                //Gauntlet++
                break;

            // 속성 공격 시리즈 생성

            /*
             * 각 속성공격은 하나를 고르면 나머지를 고를 수 없다.
             * 
             * 화염
             * 도트 데미지 중첩 불가, 도트를 맞고 있는 인원에게는 다시금 리필되는 형식
             * 1.공격에 화염 데미지 추가[도트 화염 데미지 추가, 강하지만 짧은 도트뎀] 
             * 2.화염의 도트 데미지 증가
             * 3.적이 죽은 위치에 화염이 남아 이전?
             * 4.[화염 데미지 중 사망시 폭발, 이전]
             * 
             */
            case PlayerPlugIn.PlugInType.FireAttack_1:
                EnableElementAttack(Element.Fire);
                break;

            case PlayerPlugIn.PlugInType.FireAttack_2:

                break;

            case PlayerPlugIn.PlugInType.FireAttack_3:

                break;

            case PlayerPlugIn.PlugInType.FireAttack_4:

                break;

            /*
             * 냉기
             * 1.공격에 냉기 데미지 추가[느려짐] 다수 타격시 속박
             * 2.느려짐 시간 증가
             * 3.타격시 일정 확률로 느려짐 지대 생성
             * 4. 속박 후 얼음이 깨질 때 추가데미지
             * 해당 속박은 총알의 속박을 포함.
             */

            case PlayerPlugIn.PlugInType.IceAttack_1:
                EnableElementAttack(Element.Ice);
                break;

            case PlayerPlugIn.PlugInType.IceAttack_2:

                break;

            case PlayerPlugIn.PlugInType.IceAttack_3:

                break;

            case PlayerPlugIn.PlugInType.IceAttack_4:

                break;
            /*
             * 부정(독, 부식)
             * 1.공격에 부식 데미지 추가[도트 부식 데미지 추가, 부식 스택에 따른 도트 데미지 강화]
             * 2.부식 도트 데미지 지속시간 강화
             * 3.공격시 일정 확률로 부식 지대 생성, 부식 지대 위를 지나는 적은 3초에 1회씩 부식 스택이 쌓인다.
             * 4. [부식 데미지 중첩 무제한 증가?]
             * 
             */
            case PlayerPlugIn.PlugInType.CorrosionAttack_1:
                EnableElementAttack(Element.Corrosion);
                break;

            case PlayerPlugIn.PlugInType.CorrosionAttack_2:

                break;

            case PlayerPlugIn.PlugInType.CorrosionAttack_3:

                break;

            case PlayerPlugIn.PlugInType.CorrosionAttack_4:

                break;

            //체력 증가 플러그인 라인업에 이동속도 증가를 넣으면 어떨까?
            /*Utility Plugin 으로 이름 변경
             * 1. 체력 강화
             * 2. 실드 생성
             * 3. 이동속도 증가
             * 4. 체력 강화 및 피해시 화면 전체의 적에게 적은 데미지
             */
            case PlayerPlugIn.PlugInType.Utility_1:
                //Health or Barrier ++
                healthBar.UpgradeHealthBar();
                break;
            case PlayerPlugIn.PlugInType.Utility_2:
                //ShieldEffect.SetActive(true);
                gameObject.GetComponent<Health>().ShieldEffect.SetActive(true);
                break;
            case PlayerPlugIn.PlugInType.Utility_3:
                //이동속도 증가
                break;
            case PlayerPlugIn.PlugInType.Utility_4:
                //체력 강화 및 피해시 화면 전체의 적에게 적은 데미지
                break;


            //여기가 소환수 생성인거지?

            /*소환수의 경우 
             * 1. 소환수 생성
             * 2. 소환수 Melee 공격 강화
             * 3. 소환수 Debuff 강화
             * 4. 소환수 Damage&속성공격 플레이어 강화 상태에 맞춰서 강화
             */
            case PlayerPlugIn.PlugInType.SummonAttack_1:
                //Summon Attack Possible
                break;
            case PlayerPlugIn.PlugInType.SummonAttack_2:
                //Summon Attack Possible
                break;
            case PlayerPlugIn.PlugInType.SummonAttack_3:
                //Summon Attack Possible
                break;
            case PlayerPlugIn.PlugInType.SummonAttack_4:
                //Summon Attack Possible
                break;

        }
    }
    #endregion

    #region Update Function
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            UpgradeAttackSpeed(1.2f);
        }

        switch(state)
        {
            case State.Normal:

                //Skill Activated
                if(skillActivated == true)
                {
                    if (skillBar.GetComponent<SkillBar>().slider.value == 0)
                    {
                        animate.SkillDisactive();
                        state = State.SkillDisactive;
                    }
                    skillBarTimer -= Time.deltaTime;
                    if (skillBarTimer < 0)
                    {
                        skillBar.GetComponent<SkillBar>().AddSkill(-1f);
                        skillBarTimer = 0.1f;
                    }
                }

                else if(skillActivated == false)
                {
                    skillBarTimer -= Time.deltaTime;
                    if (skillBarTimer < 0)
                    {
                        skillBar.GetComponent<SkillBar>().AddSkill(0.5f);
                        skillBarTimer = 0.1f;
                    }
                }

                //Basic Movement

                movementVector.x = Input.GetAxisRaw("Horizontal");
                movementVector.y = Input.GetAxisRaw("Vertical");
                movementVector *= currentMovementSpeed;


                animate.horizontal = movementVector.x;
                animate.vertical = movementVector.y;

                if(attackCooldownTimer>0)
                {
                    attackCooldownTimer -= Time.deltaTime;

                }

                if (rangeAttackTimer > 0)
                {
                    rangeAttackTimer -= Time.deltaTime;

                }

                if (Input.GetMouseButtonDown(0) && attackCooldownTimer<=0)
                {
                    BasicMeleeAttack();
                }

                if (Input.GetMouseButtonDown(1) && rangeAttackObject.GetComponent<RangeAttack>().bulletCount >0)
                {
                    BasicRangeAttack();
                }

                

                else if (rangeAttackTimer < 0)
                {
                    DisableRangeAttack();

                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(skillActivated == false)
                    {
                        animate.SkillActive();
                        state = State.SkillActive;
                        skillFX.SetActive(true);
                    }
                    if(skillActivated == true)
                    {
                        animate.SkillDisactive();
                        state = State.SkillDisactive;
                        skillFX.SetActive(false);
                    }
                }


                break;


            case State.SkillActive:

                skillActiveScreen.SetActive(true);
                skillActivated = true;
                break;

            case State.SkillDisactive:
                DisableSkillActive();
                break;
            case State.Hurt:
                break;
        }
    }

    
    #endregion



    private void BasicMeleeAttack()
    {

        attackCooldownTimer = attackCooldownTime;
        FindObjectOfType<AudioManager>().Play("MeleeAttack");
        DisableRangeAttack();
        currentMovementSpeed = playerScriptableObject.movementSpeedWhileAttack;
        UtilsClass.GetMouseWorldPosition();
        Vector3 mousePosition = GetMousePosition(Input.mousePosition, Camera.main);
        Vector3 attackDir = (mousePosition - transform.position).normalized;

        if (attackDir.y >= attackDir.x)
        {
            if (attackDir.y >= attackDir.x * -1)
            {
                animate.PlayAttackAnimation(8);

            }
            else
            {
                animate.PlayAttackAnimation(4);
            }
        }
        else
        {
            if (attackDir.y >= attackDir.x * -1)
            {
                animate.PlayAttackAnimation(6);
            }
            else
            {
                animate.PlayAttackAnimation(2);
            }
        }

        //CMDebug.TextPopupMouse("" + attackDir);
    }

    private void BasicRangeAttack()
    {

        rangeAttackTimer = playerScriptableObject.enableRangeAttackTime;
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = true;
        cm.GetComponent<CursorManager>().SwitchToRangeAttackCursor();
        UtilsClass.GetMouseWorldPosition();
        Vector3 mousePosition = GetMousePosition(new Vector3(Input.mousePosition.x, Input.mousePosition.y, rangeAttackObject.transform.position.z), Camera.main);

        rangeAttackObject.GetComponent<RangeAttack>().PlayerShootProjectiles_OnShoot(mousePosition);
        //CMDebug.TextPopupMouse("Range" + attackDir);
    }

    private void DisableRangeAttack()
    {
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = false;
        cm.GetComponent<CursorManager>().SwitchToArrowCursor();
    }

    public void DisableSkillActive()
    {
        skillActiveScreen.SetActive(false);
        skillActivated = false;
        skillFX.SetActive(false);
    }

    public void SpeedReturn()
    {
        currentMovementSpeed = playerScriptableObject.movementSpeed;
    }

    public void ActivateHurtState()
    {
        state = State.Hurt;
    }

    public static Vector3 GetMousePosition(Vector3 screenPosition, Camera WorldCamera)
    {
        Vector3 worldPosition = WorldCamera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;
        return worldPosition;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                rgbd2d.velocity = movementVector;
                break;
            case State.SkillActive:
                rgbd2d.velocity = new Vector3(0,0,0);
                break;
            case State.SkillDisactive:
                rgbd2d.velocity = new Vector3(0, 0, 0);
                break;
            case State.Hurt:
                rgbd2d.velocity = new Vector3(0, 0, 0);
                break;
        }

        
    }

    public void StateNormalize()
    {
        state = State.Normal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("NextRoom"))
        {
            Debug.Log("Get Next Room");
            RoomManager.Instance.NextStage();
        }
    }


    #region Plug In Function
    public PlayerPlugIn GetPlayerPlugIn()
    {
        return playerPlugIn;
    }

    //Gauntlet++ function
    private void UpgradeAttackSpeed(float multiplier)
    {
        //주로 1.1 or 1.2를 사용하자. 1.2 기준 세번 이상 업그레이드시 이상해짐.
        animate.attackSpeed *= multiplier;
        Debug.Log(Mathf.Round((attackCooldownTime /= multiplier) * 100));
        attackCooldownTime = Mathf.Round((attackCooldownTime /= multiplier)*100) * 0.01f ;
        Debug.Log(attackCooldownTime);
    }

    //현재 M 의 체력은 5로 설정되어 있음
    //초기 공격 기준 대략 3~4대의 피격시 사망으로 설정할 예정
    //OB의 MeleeAttack 을 10으로, M의 체력을 35로 설정
    private void UpgradeAttackDamage(float multiplier)
    {
        playerScriptableObject.meleeAttackDamage *= multiplier;
    }
    
    //RangeAttack++ function
    //Health&Barrier++ function
    //SummonAttack++ function
    //AttributeAttack++ function


    //MovementSpeedFunc
    private void UpgradeMovementSpeed(float multiplier)
    {
        playerScriptableObject.movementSpeed *= multiplier;
        currentMovementSpeed = playerScriptableObject.movementSpeed;
    }

    private void EnableElementAttack(Element element)
    {
        currentElement = element;
    }

    #endregion

}
