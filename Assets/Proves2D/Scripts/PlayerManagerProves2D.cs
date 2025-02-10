using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class PlayerManagerProves2D : MonoBehaviour {
    private GameManagerProves2D gameManager;
    private HUDManager hudManager;

    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpSpeed = 7f;

    private GameObject projectile;
    [SerializeField] private GameObject projectilePrefab;

    private PlayerInput playerInput;
    private bool isMoving = false;
    private float dirX = 0;
    [HideInInspector] public float facingDirection;

    private bool isDead;
    private bool isPlayerReady;
    private bool isAttacking;

    private void Awake()
    {
        facingDirection = 1;
    }

    void Start() {
        isAttacking = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerProves2D>();
        hudManager = GameObject.Find("Canvas").GetComponent<HUDManager>();
        
        playerInput = gameManager.GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Invoke("InitPlayer", 0.75f);
    }

    void InitPlayer() {
        gameManager.numLives = 3;
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
            if (!isDead)
                rb.linearVelocity = new Vector2(dirX * speed, rb.linearVelocity.y);
        }
    }

    public void Jump()
    {
        if (IsGrounded() && !isDead)
        {
            rb.linearVelocity = new Vector2(0, jumpSpeed);
        }
    }

    public void Move(InputAction.CallbackContext callbackContent)
    {
        if (!isDead) { 
            if (callbackContent.performed)
            {
                isMoving = true;
                dirX = callbackContent.ReadValue<Vector2>().x;
            }
            else
            {
                dirX = 0;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                isMoving = false;
            }
        }
    }

    public void MoveLeft()
    {
        isMoving = true;
        dirX = -1;
    }

    public void MoveRight()
    {
        isMoving = true;
        dirX = 1;
    }

    public void StopMoving()
    {
        dirX = 0;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isMoving = false;
    }


    public void Attack(InputAction.CallbackContext callbackContent)
    {
        if (callbackContent.performed)
            MakeAnAttack();
    }
    public void Attack()
    {
        MakeAnAttack();
    }

    private void MakeAnAttack()
    { 
        isAttacking = true;
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

    public void ThrowBomb()
    {
        Vector3 pos = new Vector3(this.transform.position.x + (facingDirection * (this.GetComponent<Renderer>().bounds.size.x / 16)), this.transform.position.y - this.GetComponent<Renderer>().bounds.size.y / 4, this.transform.position.z);
        projectile = Instantiate(projectilePrefab, pos, this.transform.rotation);
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
        gameManager.numLives -= 1;
        
        if (gameManager.numLives > 0) {
            Invoke("RestartLevel", 2f);
        } else {
            //TEXT GAMEOVER
            Invoke("GameOver", 2f);
        }
        hudManager.UpdateHUD();
    }

    void CompleteLevel() {
        //gameManager.GetComponent<GameManager>().CompleteLevel();
    }

    void RestartLevel() {
        this.transform.position = gameManager.spawnPoint.position;
        anim.SetBool("Die", false);
        isDead = false;
        isPlayerReady = true;
    }

    void GameOver() {
        //gameManager.GetComponent<GameManager>().GameOver();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Enemy"))
        {
            if (!isAttacking)
            {
                Destroy(c.gameObject); // Destruir el enemigo
                anim.SetTrigger("Die");
                KillPlayer();
            }
            else
            {
                Destroy(c.gameObject); // Destruir el enemigo
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            KillPlayer();
        }
    }

    private void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isAttacking)
            {
                Destroy(collision.gameObject); // Destruir el enemigo
            }
        }
    }

    public void AttackIsOver()
    {
        isAttacking = false;
    }
}