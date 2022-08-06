using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer dangerMark;
    [SerializeField] private LineRenderer laserFire;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float laserLength = 100f;

    private float angle;
    private BigBoy bigBoy;
    private Transform player;
    private Quaternion rotation;
    private Quaternion targetRotation;
    private Vector2 armDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bigBoy = GetComponentInParent<BigBoy>();
        SetStartArmPosition();

        if (armDirection.x >= 0)
        {
            dangerMark.useWorldSpace = false;
            laserFire.useWorldSpace = false;
            UpdateLaser();
        }
        else
        {
            dangerMark.useWorldSpace = true;
            laserFire.useWorldSpace = true;
            UpdateLaser();
        }
    }

    private void Update()
    {

    }

    private void UpdateLaser()
    {
        if (bigBoy.isArmFlipped)
        {
            dangerMark.SetPosition(0, firePoint.position);
            dangerMark.SetPosition(1, firePoint.position + new Vector3(armDirection.x, armDirection.y, 0f) * laserLength);
            laserFire.SetPosition(0, firePoint.position);
            laserFire.SetPosition(1, firePoint.position + new Vector3(armDirection.x, armDirection.y, 0f) * laserLength);
            //StartCoroutine(RotateArm());
        }

    }

    private void SetStartArmPosition()
    {
        Vector2 direction = player.position - transform.position;
        armDirection = direction.normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
    }

    private void RotateArm()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1 * Time.deltaTime);
    }
}

