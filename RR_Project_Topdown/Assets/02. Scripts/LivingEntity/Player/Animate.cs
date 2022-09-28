using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{

    Animator animator;

    public float horizontal;
    public float vertical;
    public float attackSpeed = 1f;
    [SerializeField] public GameObject fx;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        attackSpeed = 1f;
    }

    private void Update()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

    }

    public void PlayAttackAnimation(int dir)
    {
        fx.SetActive(true);
        animator.SetFloat("AttackSpeed", attackSpeed);
        fx.GetComponent<Animator>().SetFloat("AttackSpeed", attackSpeed);

        switch (dir)
        {
            case 8:
                animator.SetTrigger("BackAttack");
                fx.GetComponent<MCMeleeAttack>().PlayAttackFXAnimation(dir);
                break;
            case 4:
                animator.SetTrigger("SideLAttack");
                fx.GetComponent<MCMeleeAttack>().PlayAttackFXAnimation(dir);
                break;
            case 6:
                animator.SetTrigger("SideRAttack");
                fx.GetComponent<MCMeleeAttack>().PlayAttackFXAnimation(dir);
                break;
            case 2:
                animator.SetTrigger("FrontAttack");
                fx.GetComponent<MCMeleeAttack>().PlayAttackFXAnimation(dir);
                break;


            default:
                break;

        }
    }

    public void SkillActive()
    {
        animator.SetTrigger("SkillActive");
        fx.GetComponent<MCMeleeAttack>().PlaySkillFXAnimation();
    }

    public void SkillDisactive()
    {
        animator.SetTrigger("SkillDisactive");

    }

    public void HurtAnimationActive()
    {
        animator.SetTrigger("Hurt");
    }


}
