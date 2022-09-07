using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float searchRange = 20f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float attackCoolTime = 2f;

    private LivingEntity targetEntity;
    //private bool isStopped;

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
        StartCoroutine(RotatePlayer());
        StartCoroutine(RangeAttack());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hasTarget);
        /*if (!hasTarget || !isStopped)
        {
            SerachForEnemy();
            RotatePlayer();
        }*/
        SerachForEnemy();
    }

    private IEnumerator RotatePlayer()
    {
        while(!hasTarget)
        {
            transform.RotateAround(player.position, Vector3.forward, rotateSpeed * Time.deltaTime);
            yield return null;
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
                break;
            }
        }

        //if (targetEntity != null) isStopped = true;
    }

    private IEnumerator RangeAttack()
    {
        while (hasTarget)
        {
            Debug.Log("Range attack");
            //isStopped = false;
            yield return new WaitForSeconds(attackCoolTime);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, searchRange);
    }

}
