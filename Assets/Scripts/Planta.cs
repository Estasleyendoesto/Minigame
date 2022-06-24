using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planta : MonoBehaviour
{
    public float bot = 0;
    public float top = 2.3f;


    public float speed;

    bool down;
    bool up;

    bool wait;

    float timer;
    public float maxtime;

    public GameObject bullet;
    public GameObject player;

    public GameObject coin;
    public GameObject explosion;
    private AudioSource enemydestroy;

    float disparotime;
    public float maxdisparotime = 1f;
    bool disparoOn;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            enemydestroy.Play();
            Destroy(gameObject);
            Instantiate(coin, transform.position, Quaternion.identity);
        }    
    }

    void Start()
    {
        wait = true;
        enemydestroy = GameObject.Find("enemydestroy").GetComponent<AudioSource>();
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

        if (disparotime < maxdisparotime)
        {
            disparotime += Time.deltaTime;
            disparoOn = false;
        }
        else
        {
            disparotime = 0;
            disparoOn = true;
        }


        if (down)
        {
            if (transform.position.x - player.transform.position.x < 18f)
            {
                if (disparoOn)
                     StartCoroutine( Disparo() );
            }
        }

    }

    public float disparoVidatiempo = 2.1f;

    IEnumerator Disparo()
    {
        GameObject bullet2 = Instantiate( bullet, transform.position, Quaternion.identity );
        PlantaBullet pb = bullet2.GetComponent<PlantaBullet>();

        pb.dir = transform.position - player.transform.position;

        yield return null;


        // float count = 0;

        // Vector3 dir = transform.position - player.transform.position;

        // while (count < disparoVidatiempo)
        // {
        //     bullet2.transform.Translate(
        //         -dir.normalized * 7f * Time.deltaTime
        //     );

        //     count += Time.deltaTime;

        //     yield return null;
        // }

        // Destroy(bullet2);
    }
}
