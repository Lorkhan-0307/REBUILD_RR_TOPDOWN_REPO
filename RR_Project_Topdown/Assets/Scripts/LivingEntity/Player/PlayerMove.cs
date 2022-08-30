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
        //����
        Physical,
        //ȭ��
        Fire,
        //����
        Ice,
        //�ν�
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

        //�ʱ� �Ӽ��� ������ ����
        currentElement = Element.Physical;

        EnableElementAttack(Element.Fire);


    }

    #region Manage PlugInTree
    private void PlayerPlugIn_OnPlugInUnlocked(object sender, PlayerPlugIn.OnPlugInUnlockedEventArgs e)
    {
        switch (e.plugInType)
        {
            //�ܼ� ������ ���� 15%
            case PlayerPlugIn.PlugInType.GauntletAttack_1:
                UpgradeAttackDamage(1.15f);
                break;
            //������ 30% ����, ���ݼӵ� 10% ����
            case PlayerPlugIn.PlugInType.GauntletAttack_2:
                UpgradeAttackSpeed(0.9f);
                UpgradeAttackDamage(1.3f);
                break;
            //���ݼӵ� 15% ����, ������ 25% ����
            case PlayerPlugIn.PlugInType.GauntletAttack_3:
                UpgradeAttackSpeed(1.15f);
                UpgradeAttackDamage(0.75f);
                //Gauntlet++
                break;
            //���ݷ� 50% ����, �������� OR ���ݼӵ� 10% ����[�����]
            case PlayerPlugIn.PlugInType.GauntletAttack_4:
                UpgradeAttackSpeed(1.1f);
                UpgradeAttackDamage(1.5f);
                //Gauntlet++
                break;

            // �Ӽ� ���� �ø��� ����

            /*
             * �� �Ӽ������� �ϳ��� ���� �������� �� �� ����.
             * 
             * ȭ��
             * ��Ʈ ������ ��ø �Ұ�, ��Ʈ�� �°� �ִ� �ο����Դ� �ٽñ� ���ʵǴ� ����
             * 1.���ݿ� ȭ�� ������ �߰�[��Ʈ ȭ�� ������ �߰�, �������� ª�� ��Ʈ��] 
             * 2.ȭ���� ��Ʈ ������ ����
             * 3.���� ���� ��ġ�� ȭ���� ���� ����?
             * 4.[ȭ�� ������ �� ����� ����, ����]
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
             * �ñ�
             * 1.���ݿ� �ñ� ������ �߰�[������] �ټ� Ÿ�ݽ� �ӹ�
             * 2.������ �ð� ����
             * 3.Ÿ�ݽ� ���� Ȯ���� ������ ���� ����
             * 4. �ӹ� �� ������ ���� �� �߰�������
             * �ش� �ӹ��� �Ѿ��� �ӹ��� ����.
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
             * ����(��, �ν�)
             * 1.���ݿ� �ν� ������ �߰�[��Ʈ �ν� ������ �߰�, �ν� ���ÿ� ���� ��Ʈ ������ ��ȭ]
             * 2.�ν� ��Ʈ ������ ���ӽð� ��ȭ
             * 3.���ݽ� ���� Ȯ���� �ν� ���� ����, �ν� ���� ���� ������ ���� 3�ʿ� 1ȸ�� �ν� ������ ���δ�.
             * 4. [�ν� ������ ��ø ������ ����?]
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

            //ü�� ���� �÷����� ���ξ��� �̵��ӵ� ������ ������ ���?
            /*Utility Plugin ���� �̸� ����
             * 1. ü�� ��ȭ
             * 2. �ǵ� ����
             * 3. �̵��ӵ� ����
             * 4. ü�� ��ȭ �� ���ؽ� ȭ�� ��ü�� ������ ���� ������
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
                //�̵��ӵ� ����
                break;
            case PlayerPlugIn.PlugInType.Utility_4:
                //ü�� ��ȭ �� ���ؽ� ȭ�� ��ü�� ������ ���� ������
                break;


            //���Ⱑ ��ȯ�� �����ΰ���?

            /*��ȯ���� ��� 
             * 1. ��ȯ�� ����
             * 2. ��ȯ�� Melee ���� ��ȭ
             * 3. ��ȯ�� Debuff ��ȭ
             * 4. ��ȯ�� Damage&�Ӽ����� �÷��̾� ��ȭ ���¿� ���缭 ��ȭ
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
        //�ַ� 1.1 or 1.2�� �������. 1.2 ���� ���� �̻� ���׷��̵�� �̻�����.
        animate.attackSpeed *= multiplier;
        Debug.Log(Mathf.Round((attackCooldownTime /= multiplier) * 100));
        attackCooldownTime = Mathf.Round((attackCooldownTime /= multiplier)*100) * 0.01f ;
        Debug.Log(attackCooldownTime);
    }

    //���� M �� ü���� 5�� �����Ǿ� ����
    //�ʱ� ���� ���� �뷫 3~4���� �ǰݽ� ������� ������ ����
    //OB�� MeleeAttack �� 10����, M�� ü���� 35�� ����
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
