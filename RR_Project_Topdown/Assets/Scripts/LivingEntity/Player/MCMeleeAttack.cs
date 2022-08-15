using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCMeleeAttack : MonoBehaviour
{
    /*
    [SerializeField] private Sprite[] FXTextureArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;

    [SerializeField] private List<FXAnimation> FXAnimationList;
    */

    [SerializeField] private GameObject FrontAttackCollider;
    [SerializeField] private GameObject BackAttackCollider;
    [SerializeField] private GameObject SideLAttackCollider;
    [SerializeField] private GameObject SideRAttackCollider;
    [SerializeField] private Animator skillFXanimator;

    [SerializeField] PlayerScriptableObject playerScriptableObject;

    private int currentFrame;
    private float frameTimer;



    Animator animator;
    

    public enum AttackType
    {
        Front,
        Back,
        SideL,
        SideR
    }
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        FrontAttackCollider.SetActive(false);
        BackAttackCollider.SetActive(false);
        SideLAttackCollider.SetActive(false);
        SideRAttackCollider.SetActive(false);
    }

    [System.Serializable]
    public class FXAnimation
    {
        public AttackType attackType;
        public Sprite[] spriteArray;
        public float frameRate;
        public Vector2 offset;
    }

    // Update is called once per frame
    private void Update()
    {
        /*
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            //SETANIMATION
        }*/
    }


    public void PlayAttackFXAnimation(int dir)
    {
        switch (dir)
        {
            case 8:
                animator.SetTrigger("Back");
                if (skillFXanimator.isActiveAndEnabled)
                {
                    skillFXanimator.SetTrigger("Back");
                }
                break;
            case 4:
                animator.SetTrigger("SideL");
                if (skillFXanimator.isActiveAndEnabled)
                {
                    skillFXanimator.SetTrigger("SideL");
                }
                break;
            case 6:
                animator.SetTrigger("SideR");
                if (skillFXanimator.isActiveAndEnabled)
                {
                    skillFXanimator.SetTrigger("SideR");
                }
                break;
            case 2:
                animator.SetTrigger("Front");
                if (skillFXanimator.isActiveAndEnabled)
                {
                    skillFXanimator.SetTrigger("Front");
                }
                break;


            default:
                break;

        }
    }

    public void PlaySkillFXAnimation()
    {
        animator.SetTrigger("SkillActive");
    }



}
