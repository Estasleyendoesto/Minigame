using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoHead : MonoBehaviour
{
    public GameObject explosion;

    Transform parent;
    GameObject player;

    private AudioSource enemydestroy;

    void Start()
    {
        parent = transform.parent;
        enemydestroy = GameObject.Find("enemydestroy").GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            StartCoroutine( DeathAnimation() );
        }
    }

    IEnumerator DeathAnimation()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        parent.GetComponent<BoxCollider>().enabled = false;
        enemydestroy.Play();
        player.GetComponent<Rigidbody>().AddForce( Vector3.up * 30f, ForceMode.Impulse );

        parent.localScale = new Vector3( 
            1.8f,
            0.1f,
            1.8f
         );

         parent.position = new Vector3(
            parent.position.x,
            parent.position.y - 0.4f,
            parent.position.z
         );


        yield return new WaitForSeconds(0.2f);
        Destroy(parent.gameObject);
    }

}
