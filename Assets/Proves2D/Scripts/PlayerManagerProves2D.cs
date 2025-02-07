using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManagerProves2D : MonoBehaviour {
    private GameManagerProves2D gameManager;

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D col;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpSpeed = 7f;

    private GameObject projectile;
    [SerializeField] private GameObject projectilePrefab;

    private PlayerInput playerInput;
    private bool isMoving = false;
    private float dirX = 0;
    [HideInInspector] public float facingDirection = 1;

    private int lifes;
    private bool isDead;
    private bool isPlayerReady;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerProves2D>();
        playerInput = gameManager.GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();

        Invoke("InitPlayer", 0.75f);
    }

    void InitPlayer() {
        lifes = 3;
        isDead = false;
        isPlayerReady = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        isPlayerReady = true;
    }

    void Update() {
        UpdateMovement();
        UpdateAnimator();
    }

    void UpdateMovement() {
        if (isPlayerReady) {
            if (dirX! != 0)
                facingDirection = dirX;

            //GetAxis
            rb.linearVelocity = new Vector2(dirX * speed, rb.linearVelocity.y);
        }
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, jumpSpeed);
        }
    }

    public void Move(InputAction.CallbackContext callbackContent)
    {
        if (callbackContent.performed)
        {
            isMoving = true;
            dirX = callbackContent.ReadValue<Vector2>().x;
        }
        else
        {
            dirX = 0;
            rb.linearVelocity *= dirX;
            isMoving = false;
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void ThrowBomb(InputAction.CallbackContext callbackContent)
    {
        if (callbackContent.performed)
        {
            Vector3 pos = new Vector3(this.transform.position.x + (facingDirection * (this.GetComponent<Renderer>().bounds.size.x / 16)), this.transform.position.y - this.GetComponent<Renderer>().bounds.size.y / 4, this.transform.position.z);
            projectile = Instantiate(projectilePrefab, pos, this.transform.rotation);
        }
    }

    void UpdateAnimator() {
        if (dirX > 0f)
        {
            anim.SetBool("Run", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dirX < 0f)
        {
            anim.SetBool("Run", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if (rb.linearVelocityY > .1f) {
            anim.SetBool("Jump", true);
            anim.SetBool("Fall", false);
        } else if (rb.linearVelocityY < -.1f) {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
        } else {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
        }
    }

    bool IsGrounded() {
        //return (rb.linearVelocityY == 0f)?true:false;
        
        if (rb.linearVelocity.y == 0f) 
        {
            return true;
        } else {
            return false;
        }
        
        //return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    void KillPlayer() {
        isDead = true;
        isPlayerReady = false;
        lifes -= 1;
        //anim.SetTrigger("Die");
        //rb.bodyType = RigidbodyType2D.Static;
        
        if (lifes>0) {
            Invoke("RestartLevel", 2f);
        } else {
            //TEXT GAMEOVER
            Invoke("GameOver", 2f);
        }
    }

    void CompleteLevel() {
        //gameManager.GetComponent<GameManager>().CompleteLevel();
    }

    void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GameOver() {
        //gameManager.GetComponent<GameManager>().GameOver();
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.CompareTag("Enemy"))
        {
            Destroy(c.gameObject); // Destruir el enemigo
            anim.SetTrigger("Die");
            KillPlayer();
        }
        
    }

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.CompareTag("Trap") || c.gameObject.CompareTag("Death") || c.gameObject.CompareTag("Enemy"))
        {
            KillPlayer();
        }
        if (c.gameObject.CompareTag("Finish"))  {
            isPlayerReady = false;
            //sndManager.GetComponent<SoundManager>().PlayFX(2);
            rb.bodyType = RigidbodyType2D.Static;
            Invoke("CompleteLevel", 2f);
        }
    }
}