using System;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Vector2 throwForce = new Vector2(5f, 10f);
    public float lifetime; // Time before destruction
    private float time;
    private bool isExploding;

    private Rigidbody2D rb;
    private float dirX;

    private Animator anim;

    void Start()
    {
        isExploding = false; 
        time = Time.time;
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on the projectile.");
            return;
        }
        transform.Rotate(0f, 30f, 0f);

        dirX = GameObject.Find("Player").GetComponent<PlayerManagerProves2D>().facingDirection;
        throwForce = new Vector2(throwForce.x * dirX, throwForce.y);
        // Set the initial velocity
        rb.AddForce(throwForce, ForceMode2D.Impulse); // Apply impulse force

        // Destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!isExploding)
            if ((Time.time - time) > (lifetime - (lifetime/2)))
                Boom();
    }

    private void Boom()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        isExploding = true;
        anim.Play("Boom");
        rb.bodyType = RigidbodyType2D.Kinematic;
        this.GetComponent<CircleCollider2D>().isTrigger = true;
        this.GetComponent<CircleCollider2D>().radius *= 2f;
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isExploding && col.transform.tag == "Enemy") // Si choca con un enemigo
        {
            Destroy(col.gameObject); // Destruir el enemigo
        }
    }

    public void DestroyIt()
    {
        Destroy(gameObject); // Destruir el proyectil
    }
}
