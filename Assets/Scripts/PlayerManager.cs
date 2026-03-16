using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject sndManager;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpSpeed = 7f;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction jumpAction;

    private float dirX;
    private float touchDirX = 0f;
    private int lifes;
    private bool isDead;
    private bool isPlayerReady;

    // ── Setup ────────────────────────────────────────────────────────

    void Awake()
    {
        var playerMap = inputActions.FindActionMap("Player", throwIfNotFound: true);
        moveAction = playerMap.FindAction("Move", throwIfNotFound: true);
        jumpAction = playerMap.FindAction("Jump", throwIfNotFound: true);
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += OnJump;
    }

    void OnDisable()
    {
        jumpAction.performed -= OnJump;
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sndManager = GameObject.FindGameObjectWithTag("SoundManager");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        Invoke("InitPlayer", 0.75f);
    }

    void InitPlayer()
    {
        lifes = 3;
        isDead = false;
        isPlayerReady = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // ── Update ───────────────────────────────────────────────────────

    void Update()
    {
        // Combina input físic (teclat/gamepad) i botons tàctils UI
        float actionDirX = moveAction.ReadValue<Vector2>().x;
        dirX = actionDirX != 0f ? actionDirX : touchDirX;

        UpdateMovement();
        UpdateAnimator();

        // HACK
        if (Input.GetKeyDown(KeyCode.P))
        {
            rb.bodyType = RigidbodyType2D.Static;
            Invoke("CompleteLevel", 0.25f);
        }
    }

    // ── Input callbacks ──────────────────────────────────────────────

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isPlayerReady || !IsGrounded()) return;
        sndManager.GetComponent<SoundManager>().PlayFX(0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
    }

    // ── Botons tàctils UI ────────────────────────────────────────────
    // Els botons de la UI criden aquests mètodes via UnityEvent (OnPointerDown/Up)

    public void Jump()
    {
        if (!isPlayerReady || !IsGrounded()) return;
        sndManager.GetComponent<SoundManager>().PlayFX(0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
    }

    public void MoveLeft() { if (isPlayerReady) touchDirX = -1f; }
    public void MoveRight() { if (isPlayerReady) touchDirX = 1f; }
    public void StopMoving() { touchDirX = 0f; }

    // ── Moviment i animació ──────────────────────────────────────────

    void UpdateMovement()
    {
        if (!isPlayerReady) return;
        rb.linearVelocity = new Vector2(dirX * speed, rb.linearVelocity.y);
    }

    void UpdateAnimator()
    {
        if (dirX > 0f) { anim.SetBool("run", true); transform.localScale = new Vector3(1, 1, 1); }
        else if (dirX < 0f) { anim.SetBool("run", true); transform.localScale = new Vector3(-1, 1, 1); }
        else { anim.SetBool("run", false); }

        if (rb.linearVelocity.y > .1f) { anim.SetBool("jump", true); anim.SetBool("fall", false); }
        else if (rb.linearVelocity.y < -.1f) { anim.SetBool("jump", false); anim.SetBool("fall", true); }
        else { anim.SetBool("jump", false); anim.SetBool("fall", false); }
    }

    bool IsGrounded() =>
        Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

    // ── Mort i nivell ────────────────────────────────────────────────

    void KillPlayer()
    {
        isDead = true;
        isPlayerReady = false;
        lifes -= 1;
        sndManager.GetComponent<SoundManager>().PlayFX(3);
        anim.SetTrigger("dead");
        rb.bodyType = RigidbodyType2D.Static;

        if (lifes > 0) Invoke("RestartLevel", 2f);
        else Invoke("GameOver", 2f);
    }

    void CompleteLevel() => gameManager.GetComponent<GameManager>().CompleteLevel();
    void RestartLevel() => gameManager.GetComponent<GameManager>().RestartLevel();
    void GameOver() => gameManager.GetComponent<GameManager>().GameOver();

    // ── Col·lisions ──────────────────────────────────────────────────
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Collectible"))
        {
            GameManager.Instance.CollectItem();
            c.gameObject.GetComponent<Animator>().SetTrigger("collected");
            sndManager.GetComponent<SoundManager>().PlayFX(1);
            Destroy(c.gameObject);
        }

        if (c.gameObject.CompareTag("Finish"))
        {
            isPlayerReady = false;
            sndManager.GetComponent<SoundManager>().PlayFX(2);
            rb.bodyType = RigidbodyType2D.Static;
            Invoke("CompleteLevel", 2f);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Trap") ||
            c.gameObject.CompareTag("Death") ||
            c.gameObject.CompareTag("Enemy"))
            KillPlayer();
    }
}
