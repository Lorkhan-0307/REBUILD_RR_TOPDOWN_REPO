using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject LaserFire;
    [SerializeField] private Transform firePosition;

    private LineRenderer lr;
    private Transform LaserTransform;
    private Transform player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lr = LaserFire.GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    private void Update()
    {
        //Debug.Log(lr.enabled);

        if (lr.enabled)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(firePosition.position, firePosition.right);
            Debug.DrawRay(firePosition.position, firePosition.right, new Color(0, 1, 0));

            if (hit2D)
            {
                //Debug.Log(hit2D);
                Debug.Log(hit2D.transform.tag);
                if (hit2D.transform.CompareTag("Wall"))
                {
                    Debug.Log("wall");
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(firePosition.position, player.position);
        //Gizmos.DrawRay(firePosition.position, firePosition.forward);
    }

}

