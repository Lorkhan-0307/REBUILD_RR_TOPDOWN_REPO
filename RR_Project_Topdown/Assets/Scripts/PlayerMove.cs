using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;



[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMove : MonoBehaviour
{

    Rigidbody2D rgbd2d;
    Animate animate;
    Vector3 movementVector;

    [SerializeField] private float speed = 7f;

    

    private void Awake()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
        animate = GetComponent<Animate>();
    }


    // Update is called once per frame
    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");


        

        animate.horizontal = movementVector.x;
        animate.vertical = movementVector.y;
        

        movementVector *= speed;

        if(Input.GetMouseButtonDown(0))
        {
            
            UtilsClass.GetMouseWorldPosition();
            Vector3 mousePosition = GetMousePosition(Input.mousePosition, Camera.main);
            Vector3 attackDir = (mousePosition - transform.position).normalized;
            CMDebug.TextPopupMouse("" + attackDir);
        }

        
    }


    public static Vector3 GetMousePosition(Vector3 screenPosition, Camera WorldCamera)
    {
        Vector3 worldPosition = WorldCamera.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0;
        return worldPosition;
    }

    private void FixedUpdate()
    {
        rgbd2d.velocity = movementVector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("NextRoom"))
        {
            Debug.Log("Get Next Room");
            RoomManager.Instance.NextStage();
        }
    }
}
