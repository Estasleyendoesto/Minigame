using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockTypes {
        Destructible,
        Indestructible,
        Surprise,
        Struct
    };
    public BlockTypes blockType;
    private GameObject player;

    public int blockcontent = 3;
    
    public GameObject coinPrefab;
    public GameObject niordoPrefab;
    public Material structMaterial;
    public GameObject brickPrefab;
    public GameObject coinParticles;
    public GameObject waterPrefab;
    private bool destroy;

    public AudioSource block1;
    public AudioSource block2;
    public AudioSource blockdestroy;
    public AudioSource audiocoin;
    public AudioSource audiopowerup;
    

    void Start()
    {   
        block1 = GameObject.Find("audioblock1").GetComponent<AudioSource>();
        block2 = GameObject.Find("audioblock2").GetComponent<AudioSource>();
        blockdestroy = GameObject.Find("blockdestroy").GetComponent<AudioSource>();
        audiocoin = GameObject.Find("AudioCoin").GetComponent<AudioSource>();
        audiopowerup = GameObject.Find("audiopowerup").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (destroy)
            Destroy(gameObject);
    }

    public void ChangeBlockState(GameObject player)
    {
        this.player = player;

        switch (blockType)
        {
            case BlockTypes.Destructible:
                OnDestructibleBlock();
                break;
            case BlockTypes.Indestructible:
                OnIndestructibleBlock();
                break;
            case BlockTypes.Surprise:
                OnSurpriseBlock();
                break;
            case BlockTypes.Struct:
                OnStructBlock();
                break;
        }
    }

    void OnDestructibleBlock()
    {
        Player p = player.GetComponent<Player>();
        if (p.playerState == Player.State.normal)
        {
            block2.Play();
            return;
        }
        
        blockdestroy.Play();
        StartCoroutine( OnDestroying() );
    }

    void OnSurpriseBlock()
    {
        StartCoroutine( OnBouncing() );
        block1.Play();

        float type = 0;

        switch (blockcontent)
        {
            case 0:
                type = 0;
                break;
            case 1:
                type = 1;
                break;
            case 2:
                type = 2;
                break;
            case 3:
                type = Random.Range(0, 3);
                break;
        }

        switch (type)
        {
            case 0:
                StartCoroutine(CoinLauncher());
                break;
            case 1:
                GameObject water = Instantiate(waterPrefab, 
                new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), 
                Quaternion.identity);
                audiopowerup.Play();
                break;
            case 2:
                StartCoroutine(NiordoLauncher());
                break;
        }
    }

    void OnIndestructibleBlock()
    {
        block2.Play();
        Debug.Log("Choque contra un bloque indestructible");
    }

    void OnStructBlock()
    {
        block2.Play();
        Debug.Log("Choque contra un bloque indestructible golpeado");
    }

    IEnumerator NiordoLauncher()
    {
        GameObject niordo = Instantiate(niordoPrefab, transform.position, Quaternion.identity);
        audiopowerup.Play();

        float mass = 1;
        float gravity = -2.0f;
        float force = 140;
        float speedY = 0;
        float gAccel = gravity / mass;
        float acceleration;

        while (speedY >= -0.1f)
        {
            acceleration = force / mass;
            speedY += (gAccel + acceleration) * Time.deltaTime;
            
            if (niordo != null)
                niordo.transform.Translate(Vector3.up * speedY * Time.deltaTime);
            
            force = 0; 

            yield return null;
        }

        niordo.transform.position = new Vector3(
                    niordo.transform.position.x,
                    niordo.transform.position.y,
                    0.01f
                );

        while (true)
        {
            if (niordo != null)
            {
                
                niordo.transform.Rotate(Vector3.up * Time.deltaTime * 62.8f);
            }
            yield return null;
        }
    }

    IEnumerator CoinLauncher()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        audiocoin.Play();

        float mass = 1;
        float gravity = -78.2f;
        float force = 960;
        float speedY = 0;
        float gAccel = gravity / mass;
        float acceleration;

        while (speedY >= -0.1f)
        {
            acceleration = force / mass;
            speedY += (gAccel + acceleration) * Time.deltaTime;
            coin.transform.Translate(Vector3.up * speedY * Time.deltaTime);
            force = 0;

            yield return null;
        }

        player.GetComponent<Player>().coins++;
        Instantiate(coinParticles, coin.transform.position, Quaternion.identity);
        Destroy(coin);
    }

    IEnumerator OnBouncing()
    {
        float mass = 1;
        float gravity = -1.8f;
        float force = 9;
        float speedY = 0;
        float gAccel = gravity / mass;
        float acceleration;
        Vector3 oldPos = transform.position;

        while (transform.position.y >= oldPos.y)
        {
            acceleration = force / mass;
            speedY += (gAccel + acceleration) * Time.deltaTime;
            transform.Translate(Vector3.up * speedY);
            force = 0;

            yield return null;
        }

        transform.position = oldPos;
        blockType = BlockTypes.Struct;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = structMaterial;
    }

    IEnumerator OnDestroying()
    {
        float mass = 1;
        float gravity = -1.8f;
        float force = 9;
        float speedY = 0;
        float gAccel = gravity / mass;
        float acceleration;

        while (speedY >= 0)
        {
            acceleration = force / mass;
            speedY += (gAccel + acceleration) * Time.deltaTime;
            transform.Translate(Vector3.up * speedY);
            force = 0;

            yield return null;
        }

        transform.localScale = Vector3.zero;
        transform.GetComponent<Collider>().enabled = false;
        StartCoroutine( OnPropulsion() );
        StartCoroutine( OnPropulsion(-1) );
    }

    IEnumerator OnPropulsion(float horizontal = 1)
    {
        float mass = 1;
        float gravity = -28.2f;
        float force = 590;
        float speedY = 0;
        float gAccel = gravity / mass;
        float acceleration;
        float speedX = 5.4f * horizontal;

        GameObject brick = Instantiate(brickPrefab, transform.position, Quaternion.identity);
        brick.GetComponent<Collider>().enabled = false;

        while (speedY >= -3.5f)
        {
            acceleration = force / mass;
            speedY += (gAccel + acceleration) * Time.deltaTime;
            brick.transform.Translate(new Vector3(
                speedX * Time.deltaTime,
                speedY * Time.deltaTime
            ));
            brick.transform.localScale += Vector3.one * 2.2f * Time.deltaTime;
            force = 0;

            yield return null;
        }

        Destroy(brick);
        destroy = true;
    }
}
