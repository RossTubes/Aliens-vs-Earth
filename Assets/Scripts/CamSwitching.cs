using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThirdPersonMovement;

public class CamSwitching : MonoBehaviour
{
    //Camera controllers
    public Transform cam;
    private Vector3 oldCamPos;
    private Quaternion oldCamRot;

    //CameraStyles
    public Transform combatLookAt;
    public Transform firstPerson;
    public CameraStyle currentStyle;
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;
    public GameObject glasses;

    private ThirdPersonMovement _thirdPersonMovement;
    private CharacterController _characterController;
    private FirstPersonMovement _firstPersonMovement;
    private Sliding _sliding;
    private WallRunning _wallrun;
    private throwing _throwing;
    private Grapeling _grapeling;

    // Start is called before the first frame update
    public enum CameraStyle
    {
        Basic,
        Combat,
        FirstPerson,
        TopDown
    }

    private void Awake()
    {
        _thirdPersonMovement = GetComponent<ThirdPersonMovement>();
        _characterController = GetComponent<CharacterController>();
        _firstPersonMovement = GetComponent<FirstPersonMovement>();
        _sliding = GetComponent<Sliding>();
        _wallrun = GetComponent<WallRunning>();
        _throwing = GetComponent<throwing>();
        _grapeling = GetComponent<Grapeling>();
        glasses = GameObject.FindGameObjectWithTag("glasses");
    }

    // Update is called once per frame
    void Update()
    {
        // switch styles
        if (Input.GetKeyDown(KeyCode.Z)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.X)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.T)) SwitchCameraStyle(CameraStyle.TopDown);
        if (Input.GetKeyDown(KeyCode.C)) SwitchCameraStyle(CameraStyle.FirstPerson);
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
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
            Camera.main.orthographic = false;
            _thirdPersonMovement.enabled = true;
            _characterController.enabled = true;
            _firstPersonMovement.enabled = false;
            

            GetComponent<CapsuleCollider>().enabled = false;
            _sliding.enabled = false;
            _wallrun.enabled = false;
            _throwing.enabled = false;
            _grapeling.enabled = false;
            Debug.Log("3rd Person Cam = " + oldCamPos);
            Debug.Log("3rd Person Cam Rot = " + oldCamRot);
        }
        if (newStyle == CameraStyle.Combat)
        {
            thirdPersonCam.transform.position = oldCamPos;
            thirdPersonCam.transform.rotation = oldCamRot;
            combatCam.SetActive(true);
            Camera.main.orthographic = false;
            _thirdPersonMovement.enabled = true;
            _characterController.enabled = true;
            _firstPersonMovement.enabled = false;


            GetComponent<CapsuleCollider>().enabled = false;
            _sliding.enabled = false;
            _wallrun.enabled = false;
            _throwing.enabled = false;
            _grapeling.enabled = false;
        }
        if (newStyle == CameraStyle.TopDown)
        {
            //thirdPersonCam.transform.position = oldCamPos;
            //  thirdPersonCam.transform.rotation = oldCamRot;
            topDownCam.SetActive(true);
            Camera.main.orthographic = true;
            _thirdPersonMovement.enabled = true;
            _characterController.enabled = true;
            _firstPersonMovement.enabled = false;
            _thirdPersonMovement.enabled = true;
            _characterController.enabled = true;
            _firstPersonMovement.enabled = false;


            GetComponent<CapsuleCollider>().enabled = false;
            _sliding.enabled = false;
            _wallrun.enabled = false;
            _throwing.enabled = false;
            _grapeling.enabled = false;
        }

        if (newStyle == CameraStyle.FirstPerson) 
        {
            firstPersonCam.SetActive(true);
            Camera.main.orthographic = false;
            glasses.SetActive(false);
            _thirdPersonMovement.enabled=false;
            _characterController.enabled=false;
            _firstPersonMovement.enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            _sliding.enabled=true;
            _wallrun.enabled = true;
            _throwing.enabled = true;
            _grapeling.enabled = true;
        }
        currentStyle = newStyle;
    }
}
