using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour
{

    public GameObject player;

    void Update()
    {
        Vector3 dir = (player.transform.position - transform.position);
        float angle = Mathf.Atan2( dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler( 0, angle - 180, 0);
    }
}
