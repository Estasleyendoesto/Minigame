using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    public GameObject playerGameObject;
    public GameObject fishermanGo;
    public float timeToRespawn; 
    public int totaltime;

    private Player player;


    public GameObject lifescounter;
    public GameObject coinscounter;
    public GameObject timecounter;

    private int currtime;

    public Coroutine timecountercor;

    public GameObject pausemenu;
    public AudioSource audiopause;

    private bool triggerp;
    public bool canPause;
    

    void Start()
    {
        currtime = totaltime;

        player = playerGameObject.GetComponent<Player>();
        StartCoroutine( FishermanRespawn() );

        lifescounter = GameObject.Find("vidascounter");
        coinscounter = GameObject.Find("coinscounter");
        timecounter = GameObject.Find("timecounter");

        timecountercor = StartCoroutine( TimeCounter() );

        audiopause = GameObject.Find("audiopause").GetComponent<AudioSource>();
        triggerp = false;
        canPause = true;
    }

    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                triggerp = !triggerp;

                if (triggerp)
                {
                    Time.timeScale = 0;
                    audiopause.Play();
                    pausemenu.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    audiopause.Play();
                    pausemenu.SetActive(false);
                }
            }
        }


        TextMeshProUGUI lifes = lifescounter.GetComponent<TextMeshProUGUI>();
        lifes.text = "Trys x " + player.lives;

        TextMeshProUGUI coins = coinscounter.GetComponent<TextMeshProUGUI>();
        coins.text = "Coins x " + player.coins;

        TextMeshProUGUI timer = timecounter.GetComponent<TextMeshProUGUI>();
        timer.text = currtime + "";
    }

    IEnumerator FishermanRespawn()
    {
        yield return new WaitForSeconds(5.2f);
        Instantiate(fishermanGo);
    }

    public IEnumerator TimeCounter()
    {
        while (currtime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currtime--;
        }

        player.lives = 1;
        player.GetComponent<Player>().PlayerIsDead("");
    }
}
