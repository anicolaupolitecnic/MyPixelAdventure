using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {
    private GameObject gameManager;
    private GameObject sndManager;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpSpeed = 7f;
    private float dirX;

    private int lifes;
    private bool isDead;
    private bool isPlayerReady;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sndManager = GameObject.FindGameObjectWithTag("SoundManager");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        Invoke("InitPlayer", 0.75f);
    }

    void InitPlayer() {
        lifes = 3;
        isDead = false;
        isPlayerReady = isDead = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        isPlayerReady = true;
    }

    void Update() {
        UpdateMovement();
        UpdateAnimator();

        //HACK!
        if (Input.GetKeyDown(KeyCode.P)){
            rb.bodyType = RigidbodyType2D.Static;
            Invoke("CompleteLevel", 0.25f);
        }
    }

    void UpdateMovement() {
        if (isPlayerReady) {
            //GetAxis
            dirX = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(dirX * speed, rb.linearVelocity.y);
            
            if (Input.GetKeyDown("space") && IsGrounded()) {
                sndManager.GetComponent<SoundManager>().PlayFX(0);
                GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0,jumpSpeed);
            }

            /*if ((transform.position.y < -12f) && !isDead){
                KillPlayer();
            }*/
        }
    }

    void UpdateAnimator() {
        if (dirX > 0f) {
            anim.SetBool("run", true);
            transform.localScale = new Vector3(1, 1, 1);
        } else if (dirX < 0f){
            anim.SetBool("run", true);
            transform.localScale = new Vector3(-1, 1, 1);
        } else {
            anim.SetBool("run", false);
        }

        if (rb.linearVelocity.y > .1f) {
            anim.SetBool("jump", true);
            anim.SetBool("fall", false);
        } else if (rb.linearVelocity.y < -.1f) {
            anim.SetBool("jump", false);
            anim.SetBool("fall", true);
        } else {
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
        }
    }

    bool IsGrounded() {
        //return (rb.velocity.y == 0f)?true:false;
        /*
        if (rb.velocity.y == 0f) 
        {
            return true;
        } else {
            return false;
        }
        */
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    void KillPlayer() {
        isDead = true;
        isPlayerReady = false;
        lifes -= 1;
        sndManager.GetComponent<SoundManager>().PlayFX(3);
        anim.SetTrigger("dead");
        rb.bodyType = RigidbodyType2D.Static;
        
        if (lifes>0) {
            Invoke("RestartLevel", 2f);
        } else {
            //TEXT GAMEOVER
            Invoke("GameOver", 2f);
        }
    }

    void CompleteLevel() {
        gameManager.GetComponent<GameManager>().CompleteLevel();
    }

    void RestartLevel() {
        gameManager.GetComponent<GameManager>().RestartLevel();
    }

    void GameOver() {
        gameManager.GetComponent<GameManager>().GameOver();
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.CompareTag("Trap") || c.gameObject.CompareTag("Death") || c.gameObject.CompareTag("Enemy")) {
            KillPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.CompareTag("Finish"))  {
            isPlayerReady = false;
            sndManager.GetComponent<SoundManager>().PlayFX(2);
            rb.bodyType = RigidbodyType2D.Static;
            Invoke("CompleteLevel", 2f);
        }
    }
}