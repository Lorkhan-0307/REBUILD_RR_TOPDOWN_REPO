using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserArm : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    private float angle;
    private BigBoy bigBoy;
    private Transform player;
    private Quaternion rotation;
    private Vector2 armDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bigBoy = GetComponentInParent<BigBoy>();
        SetStartArmPosition();
    }

    private void Update()
    {
        RotateArm();
    }

    private void SetStartArmPosition()
    {
        bigBoy.SetArmPosition();
        Vector2 direction = player.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(gameObject.name == "LaserArm")
        {
            rotation.eulerAngles = new Vector3(0, 0, angle);
        }
        else if (gameObject.name == "LaserArm2")
        {
            rotation.eulerAngles = new Vector3(0, 0, angle + 30);
        }
        else
        {
            rotation.eulerAngles = new Vector3(0, 0, angle + 60);
        }

        transform.rotation = rotation;
    }

    private void RotateArm()
    {
        Vector2 direction = player.position - transform.position;
        armDirection = direction.normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(gameObject.name == "LaserArm")
        {
            rotation.eulerAngles = new Vector3(0, 0, angle);
        }
        else if(gameObject.name == "LaserArm2")
        {
            rotation.eulerAngles = new Vector3(0, 0, angle + 30);
        }
        else
        {
            rotation.eulerAngles = new Vector3(0, 0, angle + 60);
        }


        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
