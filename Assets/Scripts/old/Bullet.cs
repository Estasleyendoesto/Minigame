using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private Player player;
    private Vector3 HitDirection;

    private float lifeTime;
    private int maxBounces;
    private float gravityForce;
    private float bounceForce;
    private float velocity;
    private float initialElevation;
    private float initialVelocity;
    private Vector3 initialDirection;

    private float elapsedTime;
    private int bouncesCounter;

    public AudioSource wateraudio;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        lifeTime         = player.bulletLifeTime;
        maxBounces       = player.bulletMaxBounces;
        gravityForce     = player.bulletGravityForce;
        bounceForce      = player.bulletBounceForce;
        velocity         = player.bulletVelocity;
        initialElevation = player.bulletInitialElevation;
        initialVelocity  = player.highSpeed;
        initialDirection = player.GetOnionModel().forward;
    }

    void Start()
    {
        rb.AddForce(Vector3.up * initialElevation, ForceMode.Force);
        wateraudio = GameObject.Find("wateraudio").GetComponent<AudioSource>();
        wateraudio.Play();
    }

    void Update()
    {
        BulletDuration();
        BulletDisplacement();
        Gravity();      
    }

    void OnCollisionEnter(Collision collision)
    {
        // IGNORE
        if (collision.collider.tag == "Player" || collision.collider.tag == "Bullet") return;

        // COLLISION
        CollisionListener(collision.collider);
        if (HitDirection.x != 0)
            Destroy(gameObject);

        // BOUNCES COUNTER
        BulletBounces();

        // BOUNCE FORCE
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }

    private void OnCollisionExit(Collision collision)
    {
        HitDirection = Vector3.zero;
    }

    private void Gravity()
    {
        rb.AddForce(Vector3.down * gravityForce, ForceMode.Impulse);
    }

    private void BulletDisplacement()
    {
        if (velocity != 0)
            rb.velocity = new Vector3(
                -initialDirection.x * (initialVelocity + velocity),
                rb.velocity.y,
                rb.velocity.z);
    }

    private void BulletDuration()
    {
        if (lifeTime == 0) return;

        if (elapsedTime < lifeTime)
            elapsedTime += Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void BulletBounces()
    {
        if (maxBounces <= 0) return;
        
        if (bouncesCounter < maxBounces - 1)
            bouncesCounter++;
        else
            Destroy(gameObject);
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
}