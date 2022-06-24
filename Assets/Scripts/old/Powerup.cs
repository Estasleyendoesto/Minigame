using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject player;
    private float dir;
    private float speed = 4.2f;
    private Vector3 HitDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        dir = Random.Range(-1, 1);
        rb.AddForce( Vector3.up * 12, ForceMode.Impulse );
    }

    void Update()
    {
        // Desplazamiento
        rb.velocity = new Vector3(
            (dir != 0 ? dir : 1) * speed,
            rb.velocity.y
        );

        if (HitDirection.x < 0)
            dir = 1;
        if (HitDirection.x > 0)
            dir = -1;

        if (transform.position.y < -10)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (
            collider.tag == "Enemy" || 
            collider.tag == "Bomb" || 
            collider.tag == "Bullet"
            )
            Physics.IgnoreCollision(collider.GetComponent<Collider>(), GetComponent<BoxCollider>());
    }

    void OnTriggerStay(Collider other)
    {
        if (
            other.tag == "Enemy" || 
            other.tag == "Bomb" || 
            other.tag == "Bullet"
            )
            Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<BoxCollider>());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Block")
        {
            speed = 8.6f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        CollisionListener(collision.collider);
    }

    private void CollisionListener(Collider collider)
    {
        Vector3 point = collider.ClosestPointOnBounds( transform.position );
        Vector3 dir = (transform.position - point).normalized * -1;
        HitDirection += dir;
        HitDirection.Normalize();
        HitDirection = Vector3Int.RoundToInt(HitDirection);

        // DEBUG
        Debug.DrawLine( transform.position, collider.ClosestPointOnBounds( transform.position ) );
    }

    private void OnCollisionExit(Collision collision)
    {
        HitDirection = Vector3.zero;
    }

    void FixedUpdate()
    {
        // Fall
        if (rb.velocity.y < 0)
            rb.AddForce(Vector3.down * 20);
        // G
        if (rb.velocity.y >= 0)
            rb.AddForce(Vector3.down * 14);
    }
}
