using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneditas : MonoBehaviour
{
    
    // public float amplitude = 1.0f;
    // public float frequency = 1.0f;

    public float rotationSpeed = 20f;

    Transform child;

    void Start()
    {
        child = transform.GetChild(0);
    }

    void Update()
    {
        //  child.transform.position = new Vector3(
        //     0,
        //     0,
        //     amplitude * Mathf.Sin(Time.time * frequency)
        // );
        transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
