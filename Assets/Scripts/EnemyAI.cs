using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public Transform friendly;

    public LayerMask whatIsGround, whatIsPlayer, whatIsFriendly;
    public float throwUpForce;
    public float throwForce;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform AIShootPoint;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool friendlyInSightRange, friendlyInAttackRange;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        friendly = GameObject.Find("Friendly").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Checks if player is insight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        friendlyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsFriendly);
        friendlyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsFriendly);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        //friendly
        if (!friendlyInSightRange && !friendlyInAttackRange) Patroling();
        if (friendlyInSightRange && !friendlyInAttackRange) ChaseFriendly ();
        if (friendlyInSightRange && friendlyInAttackRange) AttackFriendly ();
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
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void ChaseFriendly()
    {
        agent.SetDestination(friendly.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, AIShootPoint.position,
                Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            rb.AddForce(transform.up * throwUpForce, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void AttackFriendly()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(friendly);

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

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
