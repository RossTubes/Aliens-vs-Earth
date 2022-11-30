using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class States : MonoBehaviour
{
    public FriendlyStates currentStyle;
    NavMeshAgent nav;
    // Start is called before the first frame update

    public enum FriendlyStates
    {
        Idle,
        Protect,
        Patroling,
        Attack
    }

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStyle == FriendlyStates.Idle)

            switch (currentStyle)
            {
                case FriendlyStates.Idle:
                    //alles wat hij moet doen bij idle behaviour
                    //hey ik zie een enemy
                    Debug.Log("ik ben idle");
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        currentStyle = FriendlyStates.Idle;
                    }
                    break;
                case FriendlyStates.Protect:
                    //alles wat hij moet doen bij protect behaviour
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                        Debug.Log("Protect");
                    {
                        currentStyle = FriendlyStates.Protect;
                        Debug.Log("Protect");

                    }
                    break;
                case FriendlyStates.Patroling:
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        currentStyle = FriendlyStates.Patroling;
                    }
                    break;
                case FriendlyStates.Attack:
                    if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        currentStyle = FriendlyStates.Attack;
                    }
                    break;
                default:
                    break;
            }
    }
}
