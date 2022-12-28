using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public float attackDamage = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float searchRange = 20f;
    [SerializeField] private float attackCoolTime = 2f;
    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private GameObject ForceField;

    private LivingEntity targetEntity;
    private Animator animator;
    private float attackTime;
    private bool canAttack;
    private Vector3 targetPosition;

    //private bool isStopped;

    SpriteRenderer spriteRenderer;
    Transform _pivot;
    Vector3 _offsetDirection;
    float _distance;

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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attackTime = attackCoolTime;
        canAttack = true;
        SetPivot(player);
        FindObjectOfType<StageManager>().UpgradePetAbility += Pet_UpgradePetAbility;
        FindObjectOfType<StageManager>().UpgradePetCoolTime += Pet_UpgradePetCoolTime;
        FindObjectOfType<StageManager>().UpgradeSetForceFieldActive += Pet_UpgradeSetForceFieldActive;
        //StartCoroutine(RotatePlayer());
        //StartCoroutine(RangeAttack());
    }



    // Update is called once per frame
    void Update()
    {
        if (!hasTarget)
        {
            SerachForEnemy();
            SetDirection();
        }

        if (!hasTarget || !canAttack)
        {
            //Rotate Player
            if (_pivot == null) return;

            Quaternion rotate = Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
            _offsetDirection = (rotate * _offsetDirection).normalized;
            transform.position = _pivot.position + _offsetDirection * _distance;
        }
        else
        {
            //transform.position = _pivot.position + _offsetDirection * _distance;
            RangeAttack();
            //StartCoroutine(RangeAttack());
        }
        CheckCoolTime();
        //Debug.Log("hasTarget: " + hasTarget + " canAttack: " + canAttack);

    }

    public void SetPivot(Transform pivot)
    {
        if (pivot != null)
        {
            _pivot = pivot;
            _offsetDirection = transform.position - pivot.position;
            _distance = _offsetDirection.magnitude;
        }
        else
        {
            _pivot = null;
        }
    }


    private void SerachForEnemy()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, searchRange, whatIsTarget);

        for (int i = 0; i < collider2Ds.Length; i++)
        {
            LivingEntity livingEntity = collider2Ds[i].GetComponent<LivingEntity>();

            if (livingEntity != null && !livingEntity.dead)
            {
                targetEntity = livingEntity;
                targetPosition = targetEntity.transform.position;
                break;
            }
        }

        //if (targetEntity != null) isStopped = true;
    }


    private void RangeAttack()
    {
        attackEffect.SetActive(true);

        attackTime = attackCoolTime;
        canAttack = false;
        targetEntity = null;

    }


    private void SetDirection()
    {
        if (_offsetDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void CheckCoolTime()
    {
        if (!canAttack)
        {
            attackTime -= Time.deltaTime;
            //Debug.Log(attackTime);
            if (attackTime <= 0f)
            {
                canAttack = true;
            }
        }
    }

    private void Pet_UpgradePetAbility(object sender, System.EventArgs e)
    {
        if(playerScriptableObject.latestGauntletDamageRate >= 1.0f)
        {
            attackDamage *= playerScriptableObject.latestGauntletDamageRate;
        }
    }

    private void Pet_UpgradePetCoolTime(object sender, System.EventArgs e)
    {
        attackCoolTime *= playerScriptableObject.petCoolTimeMultiplier;
    }

    private void Pet_UpgradeSetForceFieldActive(object sender, System.EventArgs e)
    {
        ForceField.SetActive(true);
    }


}
