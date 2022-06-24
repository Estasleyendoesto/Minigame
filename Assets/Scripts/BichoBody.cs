using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoBody : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
            transform.parent.GetComponent<Bicho>().dir.x *= -1;
    }
}
