using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperRoca : MonoBehaviour
{
    Rigidbody rb;
    bool arriba;
    private GameObject player;

    public bool ensuelo;

    public float maxtimedown = 0.8f;
    public float upvelocity = 5.8f;
    private float count;

    public AudioSource kickaudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arriba = true;
        kickaudio = GameObject.Find("audiokick").GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (!arriba && ensuelo)
        {
            if (count < maxtimedown)
                count += Time.deltaTime;
            else
            {
                if (transform.position.y < 18f)
                    transform.Translate( Vector3.up * Time.deltaTime * upvelocity );
                else
                {
                    arriba = true;
                    ensuelo = false;
                }
            }   
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (arriba)
            {
                player = other.gameObject;
                rb.AddForce(Vector3.down * 24, ForceMode.Impulse);
                arriba = false;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
         if (
            other.CompareTag("Player") || 
            other.CompareTag("Enemy") || 
            other.CompareTag("Bomb") || 
            other.CompareTag("Powerup") || 
            other.CompareTag("GotaAgua") || 
            other.CompareTag("Bullet") || 
            other.CompareTag("Coin")
            )

            Physics.IgnoreCollision(other, GetComponent<BoxCollider>());

        if (other.CompareTag("Player") && arriba)
        {
            rb.AddForce(Vector3.down * 24, ForceMode.Impulse);
            arriba = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (
            other.collider.CompareTag("Player") || 
            other.collider.CompareTag("Enemy") || 
            other.collider.CompareTag("Bomb") || 
            other.collider.CompareTag("Powerup") || 
            other.collider.CompareTag("GotaAgua") || 
            other.collider.CompareTag("Bullet") || 
            other.collider.CompareTag("Coin")
            )
            return;

        kickaudio.Play();
        ensuelo = true;
        count = 0; 
    }
}