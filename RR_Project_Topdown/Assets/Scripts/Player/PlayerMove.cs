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
    }
    

    [SerializeField] private float speed = 7f;
    [SerializeField] private float movementSpeedWhileAttack = 4f;
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
    }

    private void PlayerPlugIn_OnPlugInUnlocked(object sender, PlayerPlugIn.OnPlugInUnlockedEventArgs e)
    {
        switch (e.plugInType)
        {
            case PlayerPlugIn.PlugInType.Gauntlet_Enhance:
                //Gauntlet++
                break;
            case PlayerPlugIn.PlugInType.Health_BarrierMax_1:
                //Health or Barrier ++
                healthBar.UpgradeHealthBar();
                break;
            case PlayerPlugIn.PlugInType.Health_BarrierMax_2:
                //Health or Barrier ++
                //ShieldEffect.SetActive(true);
                gameObject.GetComponent<Health>().ShieldEffect.SetActive(true);
                break;
            case PlayerPlugIn.PlugInType.SummonAttack:
                //Summon Attack Possible
                break;
            case PlayerPlugIn.PlugInType.AttributeAttack:
                //Attribute Attack ++
                break;

        }
    }


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
                        DisableSkillActive();
                        animate.SkillDisactive();
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

                    CMDebug.TextPopupMouse("" + attackDir);
                }



                if (Input.GetMouseButtonDown(1))
                {
                    rangeAttackTimer = enableRangeAttackTime;
                    rangeAttackObject.GetComponent<SpriteRenderer>().enabled = true;
                    cm.GetComponent<CursorManager>().SwitchToRangeAttackCursor();
                    UtilsClass.GetMouseWorldPosition();
                    Vector3 mousePosition = GetMousePosition(Input.mousePosition, Camera.main);
                    Vector3 attackDir = (mousePosition - transform.position).normalized;
                    rangeAttackObject.GetComponent<RangeAttack>().PlayerShootProjectiles_OnShoot(attackDir);
                    CMDebug.TextPopupMouse("Range" + attackDir);



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
        }

        
        
        

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
    }

    public void SpeedReturn()
    {
        speed = 7f;
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


    public PlayerPlugIn GetPlayerPlugIn()
    {
        return playerPlugIn;
    }



    //Gauntlet++ function
    //RangeAttack++ function
    //Health&Barrier++ function
    //SummonAttack++ function
    //AttributeAttack++ function
}
