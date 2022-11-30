using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float speed = 1f;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        //transform.LookAt(Target.gameObject.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
