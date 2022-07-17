using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] public float enableRangeAttackTime = 3f;
    [SerializeField] GameObject cs;
    PlayerMove pm;
    private float timer;
    Transform parentTransform;

    private void Awake()
    {
        timer = enableRangeAttackTime;
        pm = GetComponentInParent<PlayerMove>();
        parentTransform = GetComponentInParent<Transform>();
        cs.SetActive(true);
        
    }

    private void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        cs.transform.position = difference;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ+90);
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


}
