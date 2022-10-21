using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GameObject weaponObj;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, fpscam, gunContainer;
    public MeshRenderer PLS;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {

        //Setup
        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
            slotFull = false;

            if (!weaponObj.GetComponent<Swining>() || !weaponObj.GetComponent<Gun>() || !weaponObj.GetComponent<Grapeling>()) 
                Debug.Log("MONKEEE");
            weaponObj.GetComponent<Swining>().enabled = false;
            weaponObj.GetComponent<Grapeling>().enabled = false;
            weaponObj.GetComponent<Gun>().enabled = false;

        }
        if (equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;

            if (!weaponObj.GetComponent<Swining>() || !weaponObj.GetComponent<Gun>() || !weaponObj.GetComponent<Grapeling>())
                return;
            weaponObj.GetComponent<Swining>().enabled = true;
            weaponObj.GetComponent<Gun>().enabled = true;
            weaponObj.GetComponent<Grapeling>().enabled = true;
            PLS.enabled = true;
        }
    }
    private void Update()
    {
        //Check if player is in range and pressed E 
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();
        slotFull = true;

        //Drop if equipped and Q pressed
        if (equipped && Input.GetKeyDown(KeyCode.Q)) Drop();
        slotFull = false;
        
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //make rigidbody kinematic and boxcollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //enable gun script
        if (!weaponObj.GetComponent<Swining>() || !weaponObj.GetComponent<Gun>() || !weaponObj.GetComponent<Grapeling>())
            return;
        weaponObj.GetComponent<Swining>().enabled = true;
        weaponObj.GetComponent<Gun>().enabled = true;
        weaponObj.GetComponent<Grapeling>().enabled = true;
    }//render uitzetten

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //set parent to null
        transform.SetParent(null);

        //make rigidbody kinematic and boxcollider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        //gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //add forces
        rb.AddForce(fpscam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpscam.up * dropUpwardForce, ForceMode.Impulse);
        //add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

         
        //enable gun script
        if (!weaponObj.GetComponent<Swining>() || !weaponObj.GetComponent<Gun>() || !weaponObj.GetComponent<Grapeling>())
            return;
        weaponObj.GetComponent<Swining>().enabled = false;
        weaponObj.GetComponent<Gun>().enabled = false;
        weaponObj.GetComponent<Grapeling>().enabled = false;
        Debug.Log("Becky is een idioot");

    }
}
