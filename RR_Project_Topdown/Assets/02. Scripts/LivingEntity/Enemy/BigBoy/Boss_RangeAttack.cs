using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RangeAttack : StateMachineBehaviour
{
    private BigBoy bigBoy;
    private IEnumerator rangeAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bigBoy = animator.GetComponent<BigBoy>();
        //rangeAttack = bigBoy.RangeAttack();
        //bigBoy.StartCoroutine(rangeAttack);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //bigBoy.RangeAttack();
        if (bigBoy.changePhase)
        {
            animator.SetBool("changePhase", true);
            animator.SetBool("isRangeAttack", false);
            bigBoy.changePhase = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
