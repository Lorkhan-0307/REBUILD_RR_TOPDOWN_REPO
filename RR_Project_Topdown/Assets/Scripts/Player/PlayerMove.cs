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

    #region Variables
    [SerializeField] private float speed = 7f;
    [SerializeField] private float movementSpeedWhileAttack = 4f;
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] public float enableRangeAttackTime = 3f;
    [SerializeField] public float enableSkillTime = 15f;
    [SerializeField] public float skillBarMax = 150f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] public GameObject rangeAttackObject;
    [SerializeField] public GameObject cm;
    [SerializeField] public GameObject skillActiveScreen;
    [SerializeField] public GameObject skillBar;

    private PlayerPlugIn playerPlugIn;

    private State state;
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
        skillBar.GetComponent<SkillBar>().SetMaxSkill(skillBarMax);
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    #region Manage PlugInTree
    private void PlayerPlugIn_OnPlugInUnlocked(object sender, PlayerPlugIn.OnPlugInUnlockedEventArgs e)
    {
        switch (e.plugInType)
        {
            case PlayerPlugIn.PlugInType.Gauntlet_Damage_1:
                UpgradeAttackDamage(2f);
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Damage_2:
                UpgradeAttackSpeed(0.5f);
                UpgradeAttackDamage(2f);
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Damage_3:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Damage_4:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Range_1:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Range_2:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Range_3:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Range_4:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Speed_1:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Speed_2:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Speed_3:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Gauntlet_Speed_4:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Health_BarrierMax_1:
                //Health or Barrier ++
                healthBar.UpgradeHealthBar();
                break;
            case PlayerPlugIn.PlugInType.Health_BarrierMax_2:
                //ShieldEffect.SetActive(true);
                gameObject.GetComponent<Health>().ShieldEffect.SetActive(true);
                break;
            case PlayerPlugIn.PlugInType.SummonAttack:
                //Summon Attack Possible
                break;
            case PlayerPlugIn.PlugInType.FireAttack_1:
                //Attribute Attack ++
                break;
            case PlayerPlugIn.PlugInType.IceAttack_1:
                //Attribute Attack ++
                break;
            case PlayerPlugIn.PlugInType.ElectricAttack_1:
                //Attribute Attack ++
                break;

        }
    }
    #endregion

    #region Update Function
    // Update is called once per frame
    void Update()
    {

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
                movementVector *= speed;


                animate.horizontal = movementVector.x;
                animate.vertical = movementVector.y;


                if (Input.GetMouseButtonDown(0))
                {
                    DisableRangeAttack();
                    speed = movementSpeedWhileAttack;
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



                if (Input.GetMouseButtonDown(1))
                {
                    rangeAttackTimer = enableRangeAttackTime;
                    rangeAttackObject.GetComponent<SpriteRenderer>().enabled = true;
                    cm.GetComponent<CursorManager>().SwitchToRangeAttackCursor();
                    UtilsClass.GetMouseWorldPosition();
                    Vector3 mousePosition = GetMousePosition(new Vector3(Input.mousePosition.x, Input.mousePosition.y, rangeAttackObject.transform.position.z), Camera.main);
                    
                    rangeAttackObject.GetComponent<RangeAttack>().PlayerShootProjectiles_OnShoot(mousePosition);
                    //CMDebug.TextPopupMouse("Range" + attackDir);



                }

                if (rangeAttackTimer > 0)
                {
                    rangeAttackTimer -= Time.deltaTime;
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
                    }
                    if(skillActivated == true)
                    {
                        animate.SkillDisactive();
                        state = State.SkillDisactive;
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

    private void DisableRangeAttack()
    {
        rangeAttackObject.GetComponent<SpriteRenderer>().enabled = false;
        cm.GetComponent<CursorManager>().SwitchToArrowCursor();
    }

    public void DisableSkillActive()
    {
        skillActiveScreen.SetActive(false);
        skillActivated = false;
    }

    public void SpeedReturn()
    {
        speed = 7f;
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

    private void UpgradeAttackSpeed(float multiplier)
    {
        animate.attackSpeed *= multiplier;
    }

    private void UpgradeAttackDamage(float multiplier)
    {
        attackDamage *= multiplier;
    }
    //Gauntlet++ function
    //RangeAttack++ function
    //Health&Barrier++ function
    //SummonAttack++ function
    //AttributeAttack++ function

    #endregion

}
