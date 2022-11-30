using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    private List<Transform> enemyPos = new List<Transform>();
    private float repeatRate = 5.0f;


    private void Start()
    {
        enemyPos.Clear();

        GameObject[] pos = GameObject.FindGameObjectsWithTag("Spawn Point");

        for (int i = 0; i < pos.Length; i++)
        {
        enemyPos.Add(pos[i].transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("EnemySpawner", 0.2f, repeatRate);
            //Destroy(gameObject, 11);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void EnemySpawner()
    {
        for (int i = 0; i < enemyPos.Count; i++)
        {
        Instantiate(enemy, enemyPos[i].position, enemyPos[i].rotation);
        }
    }
}
