using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RushAttack : StateMachineBehaviour
{

    private BigBoy bigBoy;
    private IEnumerator rushAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bigBoy = animator.GetComponent<BigBoy>();
        rushAttack = bigBoy.RushAttack();
        bigBoy.StartCoroutine(rushAttack);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (bigBoy.changePhase)
        {
            bigBoy.StopCoroutine(rushAttack);
            bigBoy.isRushing = false;
            animator.SetBool("changePhase", true);
            animator.SetBool("isRushing", false);
            bigBoy.changePhase = false;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
