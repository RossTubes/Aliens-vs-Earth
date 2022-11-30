using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
   // public Vector3 oldCamPos;
   // public Quaternion oldCamRot;

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
  //  public Transform combatLookAt;
  //  public Transform firstPerson;
  //  public CameraStyle currentStyle;
   // public GameObject firstPersonCam;
   // public GameObject thirdPersonCam;
  //  public GameObject combatCam;
   // public GameObject topDownCam;

   /* public enum CameraStyle
    {
        Basic,
        Combat,
        FirstPerson,
        TopDown
    }*/
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //makes the mouse invisiable
    }
    void Update()
    {
        // switch styles
       // if (Input.GetKeyDown(KeyCode.Z)) SwitchCameraStyle(CameraStyle.Basic);
       // if (Input.GetKeyDown(KeyCode.X)) SwitchCameraStyle(CameraStyle.Combat);
       // if (Input.GetKeyDown(KeyCode.T)) SwitchCameraStyle(CameraStyle.TopDown);
       // if (Input.GetKeyDown(KeyCode.C)) SwitchCameraStyle(CameraStyle.FirstPerson);

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

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        /*switch (currentStyle)
        {
            case CameraStyle.Basic:

                break;
            case CameraStyle.Combat:
                break;
            case CameraStyle.FirstPerson:
                break;
            default:
                break;
        }*/

        /* if(currentStyle == CameraStyle.Basic)
         {


             else if (currentStyle == CameraStyle.FirstPerson)
             {
                 GetComponent<ThirdPersonMovement>().enabled = false;
                 GetComponent<FirstPerson>().enabled = true;
                 GetComponent<CharacterController>().enabled = true;
                 GetComponent<CapsuleCollider>().enabled = true;



                  Vector3 dirToFirstPersonLookAt = firstPerson.position - new Vector3(transform.position.x, firstPerson.position.y, transform.position.z);
                  orientation.forward = dirToFirstPersonLookAt.normalized;

                  playerObj.forward = dirToFirstPersonLookAt.normalized;

                  float mouseX = Input.GetAxisRaw("mouse X") * Time.deltaTime * sensX;
                  float mouseY = Input.GetAxisRaw("mouse Y") * Time.deltaTime * sensY;

                  yRotation += mouseX;

                  xRotation += mouseY;
                  xRotation = Mathf.Clamp(xRotation, -90, 90f);

                  //rotate cam and rotation
                  transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                  orientation.rotation = Quaternion.Euler(0, yRotation, 0);
             }
         } else if (currentStyle == CameraStyle.Combat)
             {
                 GetComponent<ThirdPersonMovement>().enabled = true;
                 GetComponent<CharacterController>().enabled = true;

                 Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                 orientation.forward = dirToCombatLookAt.normalized;

                 playerObj.forward = dirToCombatLookAt.normalized;
         
    }*/

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

    /*private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        topDownCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        firstPersonCam.SetActive(false);

        if (newStyle == CameraStyle.Basic)
        {
            oldCamPos = thirdPersonCam.transform.position;
            oldCamRot = thirdPersonCam.transform.rotation;
            thirdPersonCam.SetActive(true);
            Debug.Log("3rd Person Cam = " + oldCamPos);
            Debug.Log("3rd Person Cam Rot = " + oldCamRot);
        }
        if (newStyle == CameraStyle.Combat)
        {
            thirdPersonCam.transform.position = oldCamPos;
            thirdPersonCam.transform.rotation = oldCamRot;
            combatCam.SetActive(true);
        }
        if (newStyle == CameraStyle.TopDown)
        {
            //thirdPersonCam.transform.position = oldCamPos;
          //  thirdPersonCam.transform.rotation = oldCamRot;
            topDownCam.SetActive(true);
        }

        if (newStyle == CameraStyle.FirstPerson) combatCam.SetActive(false);
        {
            
            firstPersonCam.SetActive(true);

        }

        currentStyle = newStyle;
    }*/
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
