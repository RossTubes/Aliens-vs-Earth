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

    private ThirdPersonMovement _thirdPersonMovement;

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
            _thirdPersonMovement.enabled = true;
            GetComponent<CharacterController>().enabled = true;

            GetComponent<FirstPersonMovement>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Sliding>().enabled = false;
            GetComponent<WallRunning>().enabled = false;
            GetComponent<throwing>().enabled = false;
            GetComponent<Grapeling>().enabled = false;
            Debug.Log("3rd Person Cam = " + oldCamPos);
            Debug.Log("3rd Person Cam Rot = " + oldCamRot);
        }
        if (newStyle == CameraStyle.Combat)
        {
            thirdPersonCam.transform.position = oldCamPos;
            thirdPersonCam.transform.rotation = oldCamRot;
            combatCam.SetActive(true);
            GetComponent<ThirdPersonMovement>().enabled = true;
            GetComponent<CharacterController>().enabled = true;

            GetComponent<FirstPersonMovement>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Sliding>().enabled = false;
            GetComponent<WallRunning>().enabled = false;
            GetComponent<throwing>().enabled = false;
            GetComponent<Grapeling>().enabled = false;
        }
        if (newStyle == CameraStyle.TopDown)
        {
            //thirdPersonCam.transform.position = oldCamPos;
            //  thirdPersonCam.transform.rotation = oldCamRot;
            topDownCam.SetActive(true);
            GetComponent<ThirdPersonMovement>().enabled = true;
            GetComponent<CharacterController>().enabled = true;

            GetComponent<FirstPersonMovement>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Sliding>().enabled = false;
            GetComponent<WallRunning>().enabled = false;
            GetComponent<throwing>().enabled = false;
            GetComponent<Grapeling>().enabled = false;
        }

        if (newStyle == CameraStyle.FirstPerson) 
        {
            firstPersonCam.SetActive(true);
            GetComponent<ThirdPersonMovement>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<FirstPersonMovement>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<Sliding>().enabled = true;
            GetComponent<WallRunning>().enabled = true;
            GetComponent<throwing>().enabled = true;
            GetComponent<Grapeling>().enabled = true;
        }
        currentStyle = newStyle;
    }
}
