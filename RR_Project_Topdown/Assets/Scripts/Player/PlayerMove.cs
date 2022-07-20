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
    }
    

    [SerializeField] private float speed = 7f;
    [SerializeField] private float attackSpeed = 4f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] public float enableRangeAttackTime = 3f;
    [SerializeField] public GameObject rangeAttackObject;
    [SerializeField] public GameObject cm;
    [SerializeField] public GameObject skillActiveScreen;

    private PlayerPlugIn playerPlugIn;

    private State state;
    private float rangeAttackTimer;


    private void Awake()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
        animate = GetComponent<Animate>();
        playerPlugIn = new PlayerPlugIn();
        playerPlugIn.OnPlugInUnlocked += PlayerPlugIn_OnPlugInUnlocked;
        state = State.Normal;
        DisableSkillActiveScreen();
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
                movementVector.x = Input.GetAxisRaw("Horizontal");
                movementVector.y = Input.GetAxisRaw("Vertical");
                movementVector *= speed;


                animate.horizontal = movementVector.x;
                animate.vertical = movementVector.y;


                if (Input.GetMouseButtonDown(0))
                {
                    DisableRangeAttack();
                    speed = attackSpeed;
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
                    rangeAttackObject.SetActive(true);
                    cm.GetComponent<CursorManager>().SwitchToRangeAttackCursor();
                    UtilsClass.GetMouseWorldPosition();
                    Vector3 mousePosition = GetMousePosition(Input.mousePosition, Camera.main);
                    Vector3 attackDir = (mousePosition - transform.position).normalized;
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
                    animate.SkillActive();
                    state = State.SkillActive;
                }


                break;


            case State.SkillActive:
                skillActiveScreen.SetActive(true);

                break;
        
        
        }

        
        
        

    }

    private void DisableRangeAttack()
    {
        rangeAttackObject.SetActive(false);
        cm.GetComponent<CursorManager>().SwitchToArrowCursor();
    }

    public void DisableSkillActiveScreen()
    {
        skillActiveScreen.SetActive(false);
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
