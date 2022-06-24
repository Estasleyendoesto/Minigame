using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadBall : MonoBehaviour
{
    public float bot = 0;
    public float top = 2.3f;

    public float speed;

    bool down;
    bool up;
    bool wait;
    float timer;
    public float maxtime;


    void Start()
    {
        wait = true;
    }

    void Update()
    {
        if (transform.position.y >= top)
        {
            down = true;
            up = false;
            wait = true;
        }
        if (transform.position.y <= bot)
        {
            up = true;
            down = false;
            wait = true;
        }

        
        if (up)
        {
            if (timer < maxtime)
                timer += Time.deltaTime;
            else
            {
                wait = false;
                timer = 0;
            }

            if (!wait)
                transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if (down)
        {
            if (timer < maxtime)
                timer += Time.deltaTime;
            else
            {
                wait = false;
                timer = 0;
            }

            if (!wait)
                transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        transform.GetChild(0).Rotate(
            new Vector3(
                1.0f,
                0.5f,
                0.3f
            ) * 180f * Time.deltaTime
        );
    }

}
