using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;

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
    [SerializeField] public GameObject gameManager;
    [SerializeField] public GameObject skillActiveScreen;
    [SerializeField] public GameObject skillBar;
    [SerializeField] public GameObject skillFX;
    [SerializeField] private GameObject Shield;
    [SerializeField] private GameObject Pet;
    [SerializeField] private GameObject ringOfFire;
    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    [SerializeField] public float attackCooldownTime = 0.5f;
    [SerializeField] private StageType stageType;
    private float attackCooldownTimer;

    private Health health;
    private PlayerPlugIn playerPlugIn;

    private State state;
    public Element currentElement;
    public EventHandler OnTriggerPortal;
    private float currentMovementSpeed;
    private float rangeAttackTimer;
    private float skillBarTimer = 0.1f;
    public bool skillActivated = false;
    private int upgradeFourthAttackCount = 0;

    private float lastHorizontalInput;
    private float lastVerticalInput;

    private bool left, right, up, down;

    #endregion

    private void Awake()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
        animate = GetComponent<Animate>();
        //playerPlugIn = new PlayerPlugIn();
        //playerPlugIn.OnPlugInUnlocked += PlayerPlugIn_OnPlugInUnlocked;
        state = State.Normal;
        DisableSkillActive();
        skillBar.GetComponent<SkillBar>().SetMaxSkill(playerScriptableObject.skillBarMax);
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = false;
        attackCooldownTimer = attackCooldownTime;
        playerScriptableObject.movespeed = playerScriptableObject.constant_movementSpeed;
        currentMovementSpeed = playerScriptableObject.constant_movementSpeed;
        health = GetComponent<Health>();

        //Util Upgrade Events
        FindObjectOfType<StageManager>().UpgradeSetShieldActive += SetShieldActive;
        FindObjectOfType<StageManager>().UpgradeMoveSpeed += UpgradeMovementSpeed;
        FindObjectOfType<StageManager>().UpgradeSetPetActive += SetPetActive;

        //초기 속성은 물리로 설정
        currentElement = Element.Physical;

        //EnableElementAttack(Element.Corrosion);

        playerScriptableObject.enabledFourthUpgrade = false;
        playerScriptableObject.enabledThirdUpgrade = false;

    }


    #region Update Function
    // Update is called once per frame
    void Update()
    {
        if (stageType == StageType.Main || stageType == StageType.Infinity)
        {
            if (!FindObjectOfType<UIManager>().isGamePaused)
            {
                if(Input.GetKeyDown(KeyCode.T))
                {
                    UpgradeAttackSpeed(1.2f);
                }

                if(Input.GetKeyDown(KeyCode.Q))
                {
                    if (FindObjectOfType<UIManager>().canInteract)
                    {
                        FindObjectOfType<UIManager>().CloseGuideUI();
                        OnTriggerPortal?.Invoke(this, EventArgs.Empty);
                    }
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
        }
        else if(stageType == StageType.jatoichi)
        {
            animate.horizontal = Input.GetAxisRaw("Horizontal");
            animate.vertical = Input.GetAxisRaw("Vertical");

            if (animate.horizontal > 0f)
            {
                right = true; left = false; up = false; down = false;
            }
            else if (animate.horizontal < 0f)
            {
                right = false; left = true; up = false; down = false;
            }
            else if (animate.vertical > 0f)
            {
                right = false; left = false; up = true; down = false;
            }
            else if(animate.vertical < 0f)
            {
                right = false; left = false; up = false; down = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (up)
                {
                    animate.PlayAttackAnimation(8);
                }
                else if(down)
                {
                    animate.PlayAttackAnimation(2);
                }
                else if (right)
                {
                    animate.PlayAttackAnimation(6);
                }
                else if (left)
                {
                    animate.PlayAttackAnimation(4);
                }
            }

        }
    }

    #endregion
    



    private void BasicMeleeAttack()
    {

        attackCooldownTimer = attackCooldownTime;
        FindObjectOfType<AudioManager>().Play("MeleeAttack", 0);
        DisableRangeAttack();
        currentMovementSpeed = playerScriptableObject.movementSpeedWhileAttack;
        UtilsClass.GetMouseWorldPosition();
        Vector3 mousePosition = GetMousePosition(Input.mousePosition, Camera.main);
        Vector3 attackDir = (mousePosition - transform.position).normalized;

        if(playerScriptableObject.enabledThirdUpgrade)
        {
            switch(currentElement)
            {
                case Element.Fire:
                    if(upgradeFourthAttackCount<=2)
                    {
                        upgradeFourthAttackCount += 1;
                    }
                    else if(upgradeFourthAttackCount >=3)
                    {
                        //Quaternion diff = this.transform.rotation;
                        Quaternion diff = rangeAttackObject.transform.rotation;

                        Vector3 fireAttackDir = (mousePosition - this.transform.position).normalized;

                        Transform fireBallTransform = Instantiate(playerScriptableObject.fireBall, this.transform.position, diff);
                        fireBallTransform.GetComponent<FireBall>().Setup(fireAttackDir);
                        upgradeFourthAttackCount = 0;
                    }
                    
                    break;

                case Element.Ice:
                    break;

                case Element.Corrosion:
                    break;
            }
        }

        if (playerScriptableObject.enabledFourthUpgrade)
        {
            switch(currentElement)
            {
                case Element.Fire:


                    break;
            }
        }


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

        FindObjectOfType<CinemachineShake>().ShakeCamera(5f, 0.1f);
        //CMDebug.TextPopupMouse("" + attackDir);
    }

    private void BasicRangeAttack()
    {

        rangeAttackTimer = playerScriptableObject.enableRangeAttackTime;
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = true;
        gameManager.GetComponent<CursorManager>().SwitchToRangeAttackCursor();
        Vector3 mousePosition = GetMousePosition(new Vector3(Input.mousePosition.x, Input.mousePosition.y, rangeAttackObject.transform.position.z), Camera.main);

        rangeAttackObject.GetComponent<RangeAttack>().PlayerShootProjectiles_OnShoot(mousePosition);
        //CMDebug.TextPopupMouse("Range" + attackDir);
    }

    private void DisableRangeAttack()
    {
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = false;
        gameManager.GetComponent<CursorManager>().SwitchToArrowCursor();
    }

    public void DisableSkillActive()
    {
        skillActiveScreen.SetActive(false);
        skillActivated = false;
        skillFX.SetActive(false);
    }

    public void SpeedReturn()
    {
        currentMovementSpeed = playerScriptableObject.movespeed;
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
            //Debug.Log("Get Next Room");
            //OnTriggerPortal?.Invoke(this, EventArgs.Empty);
            //RoomManager.Instance.NextStage();
            FindObjectOfType<UIManager>().OpenGuideUI();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("NextRoom"))
        {
            FindObjectOfType<UIManager>().CloseGuideUI();
        }
    }

    #region Plug In Function
    public PlayerPlugIn GetPlayerPlugIn()
    {
        return playerPlugIn;
    }

    //Gauntlet++ function
    public void UpgradeAttackSpeed(float multiplier)
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
    public void UpgradeAttackDamage(float multiplier)
    {
        playerScriptableObject.meleeAttackDamage *= multiplier;
        playerScriptableObject.skillActiveMeleeAttackDamage *= multiplier;
    }

    //RangeAttack++ function
    //Health&Barrier++ function
    //SummonAttack++ function
    //AttributeAttack++ function


    //MovementSpeedFunc
    public void UpgradeMovementSpeed(object sender, EventArgs e)
    {
        playerScriptableObject.movespeed *= playerScriptableObject.movementSpeedMultiplier;
        currentMovementSpeed = playerScriptableObject.movespeed;
        FindObjectOfType<StageManager>().UpgradeMoveSpeed -= UpgradeMovementSpeed;
    }

    public void EnableElementAttack(Element element)
    {
        currentElement = element;
    }
    public void SetShieldActive(object sender, EventArgs e)
    {
        Shield.SetActive(true);
        FindObjectOfType<StageManager>().UpgradeSetShieldActive -= SetShieldActive;
    }

    private void SetPetActive(object sender, EventArgs e)
    {
        Pet.SetActive(true);
        FindObjectOfType<StageManager>().UpgradeSetPetActive -= SetPetActive;
    }


    public void SetFireFourthUpgrade()
    {
        ringOfFire.SetActive(true);
    }

    #endregion

}
