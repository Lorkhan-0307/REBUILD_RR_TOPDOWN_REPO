using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCAttack : MonoBehaviour
{
    [SerializeField] private Sprite[] FXTextureArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;

    [SerializeField] private List<FXAnimation> FXAnimationList;

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
    

    // Start is called before the first frame update
    private void Start()
    {
        
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    public void PlayAttackAnimation(int dir)
    {
        switch (dir)
        {
            case 8:
                animator.SetTrigger("Back");
                break;
            case 4:
                animator.SetTrigger("SideL");
                break;
            case 6:
                animator.SetTrigger("SideR");
                break;
            case 2:
                animator.SetTrigger("Front");
                break;


            default:
                break;

        }
    }

}
