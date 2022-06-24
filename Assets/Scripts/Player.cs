using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
        Parámetros
    - - - - - - - - - - - - - - - - - - - - - - - - - - - */
    [Header("Parámetros")]
    public int lives;
    public int coins;
    public int maxCoins;
    public enum State {
        normal,
        big,
        super,
        dead
    }
    public State playerState;

    [Header("Movimiento")]
    public float normalSpeed;
    public float normalSpeedAcceleration;
    public float normalSpeedDeceleration;
   
    [Header("Movimiento Acelerado")]
    public float highSpeed;
    public float highSpeedAcceleration;
    public float highSpeedDeceleration;

    [Header("Rotación del personaje")]
    public float rotationSpeed;

    [Header("Salto")]
    public float jumpTime;
    public float jumpForce;
    public float jumpForceExtra;
    public float fallForce;
    public float gravityForce;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public float bulletFireRate;
    public float bulletLifeTime;
    public int bulletMaxBounces;
    public float bulletVelocity;
    public float bulletInitialElevation;
    public float bulletGravityForce;
    public float bulletBounceForce;


    /*
        Variables de control
    - - - - - - - - - - - - - - - - - - - - - - - - - - - */
    private Rigidbody rb;
    private Transform onionModel;
    public Vector3 MovementDirection;
    public Vector3 HitDirection;

    public float currentSpeed;

    private bool isJumping;
    private float jumpTimeCounter;

    private bool bulletAvailable;
    private float fireRateCounter;

    private StageManager stagemanager;
    public GameObject gameovermenu;
    public GameObject superfire;

    public bool stageclear;

    public GameObject inmortal;

    private bool inmortalState;
    public GameObject shield;
    
    /*
        Inputs
    - - - - - - - - - - - - - - - - - - - - - - - - - - - */
    public bool highSpeedKey;
    private bool jumpKey;
    private bool fireKey;

    /*
        Audio
    - - - - - - - - - - - - - - - - - - - - - - - - - - - */
    public AudioSource evolutionsound;
    public AudioSource audiojump;
    public AudioSource audiopowerup;
    public AudioSource audiostomp;
    public AudioSource audiofalls;
    public AudioSource audiolose;
    public AudioSource audiogameover;
    public AudioSource audioflagpole;


    /* - - - - - - - - - - - - - - - - - - - - - - - - - - */

    public bool checkout;
    public GameObject flagpoleparticles;
    public MeshRenderer meshRenderer;

    private bool canJump;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        onionModel = transform.GetChild(0);
        playerState = State.normal;

        evolutionsound = GameObject.Find("evolutionaudio").GetComponent<AudioSource>();
        audiojump = GameObject.Find("audiojump").GetComponent<AudioSource>();
        audiopowerup = GameObject.Find("audiopowerup").GetComponent<AudioSource>();
        audiostomp = GameObject.Find("stompaudio").GetComponent<AudioSource>();
        audiofalls = GameObject.Find("audiofalls").GetComponent<AudioSource>();
        audiolose = GameObject.Find("audiolose").GetComponent<AudioSource>();
        audiogameover = GameObject.Find("audiogameover").GetComponent<AudioSource>();
        audioflagpole = GameObject.Find("audioflagpole").GetComponent<AudioSource>();

        stagemanager = GameObject.Find("StageManager").GetComponent<StageManager>();

        meshRenderer = transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>();

        canJump = true;
    }

    private void Update()
    {
        if (stageclear && HitDirection.y < 0)
            return;

        InputListener();
        PlayerMovement();
        PlayerRotation();

        if (HitDirection.y < 0)
            rb.velocity = new Vector3(
                rb.velocity.x,
                0
            );
    }

    private void FixedUpdate()
    {
        if (stageclear && HitDirection.y < 0)
            return;

        PlayerJumping();
        PlayerFiring();
    }

    private void OnCollisionExit(Collision collision)
    {
        HitDirection = Vector3.zero;
    }

    private void OnCollisionStay(Collision collision)
    {
        CollisionListener(collision.collider);
        BlockCollisionListener(collision);
        EnemyCollisionListener(collision);
        PowerUpCollisionListener(collision.collider);

        if (collision.collider.CompareTag("superroca"))
        {
            if (HitDirection.x == 0)
            {
                PlayerIsDead("Enemy");
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        CoinCollisionListener(collider);
        HoleCollisionListener(collider);
        PowerUpCollisionListener(collider);
        BombCollisionListener(collider);

        if (collider.CompareTag("checkout"))
        {
            if (!checkout)
            {
                audioflagpole.Play();
                Instantiate(flagpoleparticles, transform.position, Quaternion.identity);
                Instantiate(superfire, collider.transform.position, Quaternion.identity);
            }

            checkout = true;
        }

        if (collider.CompareTag("Enemy"))
        {
            if (playerState == State.normal)
                PlayerIsDead("Enemy");

            if (playerState == State.big)
                PlayerIsNormal(collider.gameObject);

            if (playerState == State.super)
                PlayerIsBig(true);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        WaterCollisionListener(collider);
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - */
    private void InputListener()
    {
         MovementDirection = Vector3.zero;

        if (Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.leftArrowKey.isPressed)
            MovementDirection = Vector3.right;
        if (Keyboard.current.leftArrowKey.isPressed && !Keyboard.current.rightArrowKey.isPressed)
            MovementDirection = Vector3.left;

        highSpeedKey = Keyboard.current.shiftKey.isPressed;

        if (canJump)
        jumpKey      = Keyboard.current.upArrowKey.isPressed;

        fireKey      = Keyboard.current.spaceKey.isPressed;
    }

    private void CollisionListener(Collider collider)
    {
        Vector3 point = collider.ClosestPointOnBounds( transform.position );
        Vector3 dir = (transform.position - point).normalized * -1;
        HitDirection += dir;
        HitDirection.Normalize();
        HitDirection = Vector3Int.RoundToInt(HitDirection);

        // DEBUG
        Debug.DrawLine( transform.position, collider.ClosestPointOnBounds( transform.position ), Color.red );
    }

    private void PlayerRotation()
    {
        if (MovementDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(-MovementDirection, Vector3.up);
            onionModel.rotation = Quaternion.RotateTowards(onionModel.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (
                onionModel.rotation.eulerAngles.y != 90 &&
                onionModel.rotation.eulerAngles.y != 270
            )
            {
                if (Mathf.Abs(currentSpeed) > 0)
                {
                    if (currentSpeed > normalSpeed)
                        currentSpeed += normalSpeedDeceleration * -Mathf.Sign(currentSpeed);
                    if (currentSpeed > highSpeed)
                        currentSpeed += highSpeedDeceleration * -Mathf.Sign(currentSpeed);
                }
                else
                    currentSpeed = 0;

                rb.velocity = new Vector3(
                    currentSpeed,
                    rb.velocity.y,
                    rb.velocity.z
                );
            }
        }
    }

    private void PlayerMovement()
    {
        if (!highSpeedKey)
            MovementDynamic(
                0, normalSpeed, normalSpeedAcceleration, normalSpeedDeceleration
            );
        else
            MovementDynamic(
                0, highSpeed, highSpeedAcceleration, highSpeedDeceleration
            );

        if (HitDirection.x < 0 && currentSpeed < 0)
            currentSpeed = 0;
        if (HitDirection.x > 0 && currentSpeed > 0)
            currentSpeed = 0;

        rb.velocity = new Vector3(
            currentSpeed,
            rb.velocity.y,
            rb.velocity.z
        );
    }

    private void MovementDynamic(
        float minSpeed,
        float maxSpeed,
        float acceleration,
        float deceleration
    )
    {
        if (MovementDirection != Vector3.zero)
        {
            if (Mathf.Abs(currentSpeed) <= maxSpeed)
            {
                currentSpeed += acceleration * MovementDirection.x;
                currentSpeed = Mathf.Abs(currentSpeed) < maxSpeed ? currentSpeed : maxSpeed * MovementDirection.x;
            }
            else
                currentSpeed -= acceleration * Mathf.Sign(currentSpeed);
        }
        else
        {
            if ( Mathf.Abs(currentSpeed) > minSpeed)
                currentSpeed += deceleration * -Mathf.Sign(currentSpeed);
            if (Mathf.Abs(currentSpeed) < acceleration)
                currentSpeed = 0;
        }
    }
    
    private void PlayerJumping()
    {
        if (isJumping)
        {
            if (jumpKey)
            {
                if (jumpTimeCounter < jumpTime)
                    jumpTimeCounter += Time.deltaTime;
                else
                {
                    jumpTimeCounter = 0;
                    isJumping = false;
                }

                if (rb.velocity.y >= 0)
                    rb.AddForce(Vector3.up * jumpForceExtra, ForceMode.Acceleration);
            }
        }

        if (jumpKey && HitDirection.y < 0 && rb.velocity.y <= 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping  = true;
            audiojump.Play();
        }
        
        if (rb.velocity.y >= 0)
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Force);

        if (rb.velocity.y < 0)
            rb.AddForce(Vector3.down * fallForce, ForceMode.Force);
    }

    private void PlayerFiring()
    {
        if (playerState == State.normal || playerState == State.big)
            return;

        if (!bulletAvailable)
        {
            if (fireRateCounter < bulletFireRate)
                fireRateCounter += Time.deltaTime;
            else
                bulletAvailable = true;
        }
        else
        {
            if (fireKey)
            {
                if (
                    onionModel.rotation.eulerAngles.y == 90 ||
                    onionModel.rotation.eulerAngles.y == 270
                )
                {
                    Vector3 respawn = new Vector3(
                        transform.position.x - onionModel.transform.forward.x,
                        transform.position.y,
                        transform.position.z
                    );
                    GameObject bullet = Instantiate(bulletPrefab, respawn, Quaternion.identity);
                    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
                }

                bulletAvailable = false;
                fireRateCounter = 0;
            }
        }
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - */
    void BombCollisionListener(Collider collider)
    {
        if (collider.CompareTag("Bomb"))
        {
            if (playerState == State.normal)
                PlayerIsDead("Bomb");
            if (playerState == State.big)
                PlayerIsNormal(collider.gameObject);
            if (playerState == State.super)
                PlayerIsBig(true);
        }
    }

    void BlockCollisionListener(Collision collision)
    {
        if (collision.transform.tag == "Block")
        {
            if (HitDirection.y > 0 && HitDirection.x == 0)
                collision.gameObject.GetComponent<Block>().ChangeBlockState(gameObject);
        }
    }

    void CoinCollisionListener(Collider collider)
    {
        if (collider.transform.tag == "Coin")
        {
            coins += 1;
            if (coins >= maxCoins)
            {
                lives += 1;
                coins = 0;
            }
        }
    }

    void HoleCollisionListener(Collider collider)
    {
        if (collider.transform.tag == "Hole")
            PlayerIsDead("Hole");
    }

    void EnemyCollisionListener(Collision collision)
    {
        if (collision.transform.tag != "Enemy")
            return;

        if (HitDirection.x != 0 || HitDirection.y >= 0)
        {
            if (playerState == State.normal)
                PlayerIsDead("Enemy");

            if (playerState == State.big)
                PlayerIsNormal(collision.gameObject);

            if (playerState == State.super)
                PlayerIsBig(true);

            return;
        }

        if (HitDirection.y < 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine( WaitJump() );
        }
    }

    IEnumerator WaitJump()
    {
        canJump = false;
        yield return new WaitForSeconds(0.1f);
        canJump = true;
    }

    void PowerUpCollisionListener(Collider collider)
    {
        if (collider.CompareTag("Abono"))
        {
            if (collider.transform.position.z > 0)
            {
            Destroy(collider.gameObject);
            if (playerState != State.super)
                PlayerIsSuper();
            }
        }
    }

    void WaterCollisionListener(Collider collider)
    {
         if (collider.CompareTag("GotaAgua"))
        {
            Physics.IgnoreCollision(collider.GetComponent<Collider>(), GetComponent<Collider>());
            Destroy(collider.gameObject);
            if (playerState == State.normal || playerState == State.super)
                PlayerIsBig();
        }
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - */
    public void PlayerIsDead(string cause)
    {
        lives -= 1;
        if (lives <= 0)
        {
            stagemanager.canPause = false;
            audiogameover.Play();
            Debug.Log("Game Over");
            playerState = State.dead;
            gameObject.SetActive(false);
            gameovermenu.SetActive(true); 
            StopCoroutine( stagemanager.timecountercor );
            stagemanager.timecountercor = null;
        }
        else
        {
            switch (cause)
            {
                case "Hole":
                    StartCoroutine(PlayerRespawn(0));
                    break;
                case "Enemy":
                    StartCoroutine(PlayerRespawn(0));
                    break;
                case "Bomb":
                    StartCoroutine(PlayerRespawn(0));
                    break;
            }
        }
    }

    void PlayerIsNormal(GameObject enemy)
    {
        shield.SetActive(false);
        playerState = State.normal;
        transform.localScale = new Vector3(0.75f, 0.5f, 0.75f);
        // meshRenderer.material.color = new Color(1, 1, 1);
        meshRenderer.material.color = new Color32(255, 255, 255, 255);
        StartCoroutine( Invencible(enemy) );
        audiofalls.Play();
        Debug.Log("El jugador ha vuelto al estado normal...");
    }

    void PlayerIsBig(bool returning = false)
    {
        playerState = State.big;
        transform.localScale = Vector3.one;

        // Parameters (if there are)
        // bulletFireRate = 0.8f;

        // Coroutine shieldf;
        Color c = new Color32(89, 139, 210, 255);
        meshRenderer.material.color = c;

        shield.SetActive(true);

        if (returning)
        {
            // audiofalls.Play();
            PlayerIsNormal(null);
            Debug.Log("El jugador ha vuelto al modo grande...");
        }
        else
        {
            evolutionsound.Play();
            // shieldf = StartCoroutine( OnShield() );    
            Debug.Log("El jugador evoluciona al modo grande...");
        }
    }

    void PlayerIsSuper()
    {
        shield.SetActive(false);

        playerState = State.super;
        transform.localScale = Vector3.one;

        Color c = new Color32(210, 88, 88, 255);
        meshRenderer.material.color = c;

        // Parameters (if there are)
        // bulletFireRate = 0.2f;

        audiostomp.Play();
        Debug.Log("El jugador evoluciona al modo abono...");
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - */
    IEnumerator PlayerRespawn(float time)
    {
        audiolose.Play();
        yield return new WaitForSeconds(time);
        PlayerIsNormal(null);

        if (checkout)
            transform.position = new Vector3(137.72f, 4, 0);
        else
            transform.position = new Vector3(-5.5f, 4, 0);
    }

    IEnumerator Invencible(GameObject enemy)
    {
        float counter = 0;

        while (counter < 3)
        {
            if (enemy != null)
            {
                Physics.IgnoreCollision(enemy.GetComponent<Collider>(), GetComponent<Collider>());

            }
            counter += Time.deltaTime;
            yield return null;
        }

        if (enemy != null)
        {
            Physics.IgnoreCollision(enemy.GetComponent<Collider>(), GetComponent<Collider>(), false);
        }
    }

    /* - - - - - - - - - - - - - - - - - - - - - - - - - - */
    public Transform GetOnionModel() { return onionModel; }
}
