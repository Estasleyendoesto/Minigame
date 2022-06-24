using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inmortal : MonoBehaviour
{

    private Transform parent;

    void Start()
    {
        parent = transform.parent;
    }

    void OnCollisionEnter(Collision other)
    {
        NoDmg(other.collider);
    }

    void OnTriggerEnter(Collider other)
    {
        NoDmg(other);
    }

    private void OnTriggerStay(Collider other)
    {
        NoDmg(other);
    }

    void NoDmg(Collider collider)
    {
        // Debug.Log(collider.tag);
        if (collider.CompareTag("superroca"))
            Physics.IgnoreCollision(parent.GetComponent<Collider>(), collider);
    }
}
