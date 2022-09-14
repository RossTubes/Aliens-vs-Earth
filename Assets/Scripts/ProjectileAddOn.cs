using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddOn : MonoBehaviour
{
    public int damage;

    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;

        // check if you hit an enemy
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(damage);

            // destroy projectile after kill
            Destroy(gameObject);
        }

        // make sure projectile sticks to surface
        rb.isKinematic = true;

        // make sure projectile moves with target (makes the knife go flat in ground)
        //transform.SetParent(collision.transform);
    }
}
