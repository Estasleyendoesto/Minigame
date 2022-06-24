using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraM : MonoBehaviour
{
    public Transform player;
    public Rigidbody playerRB;
    public float xmin;
    public float xmax = 240;

    public float oldPosY = 5.2f;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        XDisplacement();
    }

    private void XDisplacement()
    {
        // Limite izquierdo
        if (player.transform.position.x <= xmin)
            return;

        // Limite derecho
        if (player.transform.position.x >= xmax)
            return;

        Vector3 pos = new Vector3(
            player.position.x - transform.position.x,
            0
        );

        transform.Translate(pos);   
    }
}
