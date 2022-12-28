using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] private Transform pfBullet;
    [SerializeField] private GameObject firePosition;
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    private float bulletCountTimer = 0f;


    PlayerMove pm;
    private float timer;
    Transform parentTransform;
    public float bulletCount;

    private void Awake()
    {
        timer = playerScriptableObject.enableRangeAttackTime;
        pm = GetComponentInParent<PlayerMove>();
        parentTransform = GetComponentInParent<Transform>();
        bulletCount = playerScriptableObject.maxBulletCount;
        
    }
    

    private void Update()
    {
        if(bulletCount < playerScriptableObject.maxBulletCount)
        {
            bulletCountTimer += Time.deltaTime;
            if(bulletCountTimer >= playerScriptableObject.reloadBulletTime)
            {
                bulletCount += 1;
                bulletCountTimer = 0f;
            }
        }
        
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

    public void PlayerShootProjectiles_OnShoot(Vector3 mousePosition)
    {
        bulletCount -= 1;
        Quaternion diff = this.transform.rotation;
        //diff = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z+90);
        Vector3 attackDir = (mousePosition - firePosition.transform.position).normalized;

        Transform bulletTransform =  Instantiate(pfBullet, firePosition.transform.position, diff);
        bulletTransform.GetComponent<Projectiles>().Setup(attackDir);

    }


}
