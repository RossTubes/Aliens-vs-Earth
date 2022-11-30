using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static States;
using static ThirdPersonMovement;

public class FollowPlayer : MonoBehaviour
{

    public Transform enemy;
    public Transform zombie;
    public Transform player;

    //attacking
    public float throwUpForce;
    public float throwForce;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform AIShootPoint;
    public float timeBetweenAttacks;

    //navmesh agent
    public Transform target; //target pos to stand
    public NavMeshAgent agent;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, enemyInSightRange, enemyInAttackRange, protect;
    public bool isToggled = false;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public LayerMask whatIsGround, whatIsPlayer, whatIsEnemy;
    // Start is called before the first frame update


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if player is insight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

        if (!enemyInSightRange && !enemyInAttackRange && !protect) Patroling();
        if (enemyInSightRange && !enemyInAttackRange) ChaseEnemy();
        if (enemyInSightRange && enemyInAttackRange) Attack();

        if (Input.GetKeyDown(KeyCode.Alpha1)) Patroling();
        if(Input.GetKeyDown(KeyCode.Alpha2) && playerInSightRange)
        {
            isToggled = !isToggled;
        }

        if (isToggled)
        {
            Protect();
        }
        else
        {
            Patroling();
        }
    }

    private void Patroling()
    {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    public void Protect()
    {
        agent.SetDestination(target.position);
        protect = true;
    }

    private void ChaseEnemy()
    {
        agent.SetDestination(enemy.position);
        agent.SetDestination(zombie.position);
    }

    private void Attack()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(enemy);
        transform.LookAt(zombie);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, AIShootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            rb.AddForce(transform.up * throwUpForce, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
