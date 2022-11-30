using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform player;
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider player)
    {
        player.transform.position = respawnPoint.transform.position;
    }
}
