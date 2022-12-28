using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private float shootRange;
    private Transform bigBoy;
    private BigBoy bigBoy2;
    private IEnumerator updatePath;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bigBoy = animator.GetComponent<Transform>();
        bigBoy2 = animator.GetComponent<BigBoy>();
        updatePath = bigBoy2.UpdatePath();
        bigBoy2.StartCoroutine(updatePath);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(bigBoy.position, shootRange, whatIsTarget);

        for (int i = 0; i < collider2Ds.Length; i++)
        {
            LivingEntity livingEntity = collider2Ds[i].GetComponent<LivingEntity>();
            //Debug.Log(animator.GetComponent<BigBoy>().canRangeAttack);

            if (livingEntity != null && !livingEntity.dead && animator.GetComponent<BigBoy>().canRangeAttack)
            {
                animator.SetBool("isRangeAttack", true);
            }
        }

        if (bigBoy2.changePhase)
        {
            animator.SetBool("changePhase", true);
            animator.SetBool("isWalking", false);
            bigBoy2.changePhase = false;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bigBoy2.StopCoroutine(updatePath);
    }

}
