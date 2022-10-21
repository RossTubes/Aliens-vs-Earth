using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapeling : MonoBehaviour
{
    [Header("References")]
    private FirstPersonMovement Fpm;
    public Transform cam;
    public Transform grappleGunTip;
    public LayerMask WhatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappeling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overShootYAxis;
    private Vector3 grapplePoint;

    [Header("CoolDown")]
    public float grappelingCoolDown;
    private float grappelingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    public bool grappeling;
    public GameObject pickupObj;
    private void Start()
    {
        Fpm = GetComponent<FirstPersonMovement>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(grappleKey))
        {
            if (!pickupObj.GetComponent<PickUpController>().equipped)
                return;
            StartGrapple();
        }

        if (grappelingCdTimer > 0)
        {
            grappelingCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (grappeling)
            lr.SetPosition(0, grappleGunTip.position);
    }
    private void StartGrapple()
    {
        if (grappelingCdTimer > 0) return;

        grappeling = true;

        Fpm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, WhatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(executeGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void executeGrapple()
    {
        Fpm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overShootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overShootYAxis;

        Fpm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }
    public void StopGrapple()
    {
        Fpm.freeze = false;

        grappeling = false;

        grappelingCdTimer = grappelingCoolDown;

        lr.enabled = false;
    }
}
