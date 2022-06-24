using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicho : MonoBehaviour
{
    Rigidbody rb;

    bool awake;
    public Vector3 dir;

    public float speed;

    public GameObject coin;
    public GameObject explosion;
    private AudioSource enemydestroy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemydestroy = GameObject.Find("enemydestroy").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (awake)
            transform.Translate( Vector3.right * -Mathf.Sign(dir.x) * Time.deltaTime * speed );

        rb.AddForce(Vector3.down * 20f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            awake = true;
            dir = (transform.position - other.transform.position).normalized;
        }

        if (other.CompareTag("Bullet"))
        {
            StartCoroutine( OnDeath() );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            awake = false;
        }
    }

    IEnumerator OnDeath()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        enemydestroy.Play();
        Destroy(gameObject);
        Instantiate(coin, transform.position, Quaternion.identity);
        yield return null;
    }
}
