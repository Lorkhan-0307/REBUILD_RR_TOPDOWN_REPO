using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{

    Animator animator;

    public float horizontal;
    public float vertical;

    [SerializeField] public GameObject fx;


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
        fx.SetActive(true);
        switch (dir)
        {
            case 8:
                animator.SetTrigger("BackAttack");
                fx.GetComponent<MCAttack>().PlayAttackFXAnimation(dir);
                break;
            case 4:
                animator.SetTrigger("SideLAttack");
                fx.GetComponent<MCAttack>().PlayAttackFXAnimation(dir);
                break;
            case 6:
                animator.SetTrigger("SideRAttack");
                fx.GetComponent<MCAttack>().PlayAttackFXAnimation(dir);
                break;
            case 2:
                animator.SetTrigger("FrontAttack");
                fx.GetComponent<MCAttack>().PlayAttackFXAnimation(dir);
                break;


            default:
                break;

        }
    }

    public void SkillActive()
    {
        animator.SetTrigger("SkillActive");
        fx.GetComponent<MCAttack>().PlaySkillFXAnimation();
    }



}
