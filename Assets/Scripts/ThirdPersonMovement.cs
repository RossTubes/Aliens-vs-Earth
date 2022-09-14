using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //First Person movement
   /* public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;*/

    //Camera controllers
    public CharacterController Controller;
    public Transform cam;

    //characterControls
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float speed = 6f;
    public float gravity = 6f;
    public float jumpHeight = 3f;

    //jumping mechanics
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    //CameraStyles
    public Transform combatLookAt;
    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat,
        FirstPerson,
        TopDown
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //makes the mouse invisiable
    }
    void Update()
    {
       /*if(currentStyle == CameraStyle.FirstPerson)
        {
            float mouseX = Input.GetAxisRaw("mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90f);

            //rotate cam and rotation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }*/




        if(currentStyle == CameraStyle.Basic)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);//checks wich way is point 0 degrees to see what is forward

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            else if(currentStyle == CameraStyle.Combat)
            {

            }
        }


        //Jumping
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //creates a tiny sphere to colide with the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -10f;
        }
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;


        Controller.Move(velocity * Time.deltaTime);

    }
    private bool IsGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            Debug.Log("jump one");
            return true;
        }
        else
            return false;
    }
}
