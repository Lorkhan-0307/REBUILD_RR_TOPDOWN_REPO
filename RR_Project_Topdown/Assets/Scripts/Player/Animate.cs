using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{

    Animator animator;

    public float horizontal;
    public float vertical;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

    }

    public void PlayAttackAnimation(int dir)
    {
        switch(dir)
        {
            case 8:
                animator.SetTrigger("BackAttack");
                break;
            case 4:
                animator.SetTrigger("SideLAttack");
                break;
            case 6:
                animator.SetTrigger("SideRAttack");
                break;
            case 2:
                animator.SetTrigger("FrontAttack");
                break;


            default:
                break;

        }
    }



}
