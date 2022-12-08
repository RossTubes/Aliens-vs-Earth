using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static CamSwitching;
using Unity.Mathematics;

public class ThirdPersonMovement : MonoBehaviour
{
    //First Person movement
    public float sensX;
    public float sensY;

    public Transform playerObj;
    public Transform player;
    public Transform orientation;

    float xRotation;
    float yRotation;

    //Camera controllers
    public CharacterController Controller;
    public Transform cam;
    private CamSwitching camSwitch;
    public Transform cameraTransform;

    [SerializeField]
    private float rotationSpeed = .8f;

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
    private bool isGrounded;
    private void Start()
    {
        //makes the mouse invisiable
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camSwitch = transform.GetComponent<CamSwitching>();
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
       // direction = direction.x * cameraTransform.forward.normalized + direction.z * cameraTransform.forward;

        //rotate playerObj to camera view
        //quaternion rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);//checks wich way is point 0 degrees to see what is forward

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

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
