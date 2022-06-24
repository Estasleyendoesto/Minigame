using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoEnemigo : MonoBehaviour
{
    public GameObject player;
    public GameObject bollet;
    public GameObject banana;
    
    private float distance = 4.0f;
    private float angle;

    void Start()
    {
        StartCoroutine( Shot() );
    }

    void Update()
    {
        transform.position = new Vector3(
            player.transform.position.x + 20f,
            0,
            player.transform.position.z
        );
    }

    IEnumerator Shot()
    {
        while (true)
        {
            yield return new WaitForSeconds( Random.Range(2, 7) );
            if (transform.position.x < 230 && player.transform.position.x > 40)
                StartCoroutine( Bullet() );
        }

    }

    IEnumerator Bullet()
    {
        float ydis = transform.position.y - distance < 0 ? 0 : distance;

        GameObject bullet = Instantiate(
            bollet,
            new Vector3(
                transform.position.x,
                transform.position.y + Random.Range(ydis, distance),
                -2
            ),
            Quaternion.identity
        );

        bullet.transform.localScale = Vector3.one * 0.1f;
        bullet.transform.GetChild(0).rotation = Quaternion.Euler(0, -90, 0);

        float xr = Random.value < 0.5 ? 0 : 1;        
        float yr = Random.value < 0.5 ? 0 : 1;        
        float zr = Random.value < 0.5 ? 0 : 1;

        Transform child =  bullet.transform.GetChild(0);

        float time = 0;

        while (time < 5.0f)
        {
            if (bullet != null)
            {
                bullet.transform.Translate( Vector3.left * Random.Range(5, 11) * Time.deltaTime );

                child.Rotate( new Vector3(
                    xr, yr, zr
                ) * Random.Range(60, 500) * Time.deltaTime);
            }
            
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(bullet);
    }
}
