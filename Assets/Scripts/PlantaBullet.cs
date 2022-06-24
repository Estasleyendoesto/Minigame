using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaBullet : MonoBehaviour
{
    public Vector3 dir;
    private float count;
    public float speed = 7f;
    public float disparoVidatiempo = 2f;

    void Update()
    {
        if (count < disparoVidatiempo)
        {
            transform.Translate(
                -dir.normalized * speed * Time.deltaTime
            );
            count += Time.deltaTime;
        }
        else
            Destroy(gameObject);
        
    }
}
