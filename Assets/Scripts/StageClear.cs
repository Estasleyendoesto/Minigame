using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear : MonoBehaviour
{

    public GameObject particles;
    public AudioSource winaudio;
    public AudioSource ohyeahudio;
    public AudioSource yayaudio;
    public AudioSource stageclearaudio;
    public AudioSource audionice;

    public float amplitude = 1.0f;
    public float frequency = 1.0f;
    public float height;

    private Transform barrita;

    public float scalespeed;


    public AudioSource audioexplosion;
    public GameObject explosionparticle;
    public GameObject blast;

    public GameObject black;

    public GameObject stageclearmenu;
    public GameObject stagemanager;


    void Start()
    {
        barrita = transform.GetChild(0);

        yayaudio = GameObject.Find("yay").GetComponent<AudioSource>();
        stageclearaudio = GameObject.Find("audiostageclear").GetComponent<AudioSource>();

        audioexplosion = GameObject.Find("audioexplosion").GetComponent<AudioSource>();
        audionice = GameObject.Find("audionice").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (barrita == null)
            return;

        barrita.transform.position = new Vector3(
    	    transform.position.x,
            height + amplitude * Mathf.Sin(Time.time * frequency)
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            if (barrita == null)
                return;

            stagemanager.GetComponent<StageManager>().canPause = false;
            StopCoroutine(stagemanager.GetComponent<StageManager>().timecountercor);
            Instantiate(particles, barrita.transform.position, Quaternion.identity);
            stageclearaudio.Play();
            yayaudio.Play();
            Destroy(barrita.gameObject);

            animationfinal(other.gameObject);
        }
    }

    void animationfinal(GameObject player)
    {
        Player obj = player.GetComponent<Player>();
        obj.stageclear = true;

        StartCoroutine(Yoyo(player));
    }

    IEnumerator Yoyo(GameObject player)
    {
        bool finish = false;

        float rotationvelocity = 1;

        float speed = 10;

        float xpos = player.transform.position.x;

        while(!finish)
        {
            if (rotationvelocity < 60000f && rotationvelocity > 0)
                rotationvelocity += Time.deltaTime * 8000;

            Transform hijo = player.transform.GetChild(0);
            hijo.Rotate(Vector3.up * Time.deltaTime * (420.8f + rotationvelocity) );

            if (player.GetComponent<Player>().HitDirection.y >= 0)
                player.transform.Translate(Vector3.right * speed * Time.deltaTime);
            else
                finish = true;
            yield return null;
        }

        Debug.Log("EXPLOOOOOOOOOOOOOOOOSION!");
        audioexplosion.Play();

        Instantiate(explosionparticle, new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -2
        ), Quaternion.identity);

        Instantiate(blast, new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -4
        ), Quaternion.identity);

        StartCoroutine( DrawBlack(player.transform) );
    }

    IEnumerator DrawBlack(Transform player)
    {
        GameObject n = Instantiate(
            black,
            new Vector3(
            player.position.x,
            player.position.y,
            -2
            ),
            Quaternion.identity
        );

        n.transform.rotation = Quaternion.Euler(
            -90,
            0,
            0
        );

        float ct = 0;
        while (ct < 1f)
        {
            n.transform.localScale = new Vector3(
                n.transform.localScale.x + Time.deltaTime * scalespeed,
                n.transform.localScale.y,
                n.transform.localScale.z + Time.deltaTime * scalespeed
            );

            ct += Time.deltaTime;
            yield return null;
        }

        GameObject.Find("timecounter").SetActive(false);
        audionice.Play();
        Time.timeScale = 0;
        stageclearmenu.SetActive(true);
    }
}
