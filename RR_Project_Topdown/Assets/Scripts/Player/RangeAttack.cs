using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] public float enableRangeAttackTime = 3f;
    [SerializeField] private Transform pfBullet;
    [SerializeField] private GameObject firePosition;


    PlayerMove pm;
    private float timer;
    Transform parentTransform;

    private void Awake()
    {
        timer = enableRangeAttackTime;
        pm = GetComponentInParent<PlayerMove>();
        parentTransform = GetComponentInParent<Transform>();


    }

    private void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        /*
        if(rotationZ < -90 || rotationZ > 90)
        {
            if(parentTransform.transform.eulerAngles.y == 0)
            {
                transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
            }
            else if(parentTransform.transform.eulerAngles.y == 180)
            {

            }
            
        }*/




    }

    public void PlayerShootProjectiles_OnShoot(Vector3 shootDir)
    {
        Quaternion diff = this.transform.rotation;
        //diff = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z+90);

        Transform bulletTransform =  Instantiate(pfBullet, firePosition.transform.position, diff);
        bulletTransform.GetComponent<Projectiles>().Setup(shootDir);

    }


}
