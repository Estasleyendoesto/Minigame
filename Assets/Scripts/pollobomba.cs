using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pollobomba : MonoBehaviour
{
    public GameObject explosion;
    private AudioSource enemydestroy;

    void Start()
    {
        enemydestroy = GameObject.Find("enemydestroy").GetComponent<AudioSource>();


    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            enemydestroy.Play();
            Destroy(gameObject);
        }
    }
}
