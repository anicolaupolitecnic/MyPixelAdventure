using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public float speed;    // Speed of the projectile
    public float lifetime; // Time before destruction

    private Rigidbody2D rb;
    private float dirX;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on the projectile.");
            return;
        }

        dirX = GameObject.Find("Player").GetComponent<PlayerManagerProves2D>().facingDirection;

        // Set the initial velocity
        rb.linearVelocity = dirX * transform.right * speed; // Use 'right' for 2D movement

        // Destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Si choca con un enemigo
        {
            Destroy(other.gameObject); // Destruir el enemigo
            Destroy(gameObject); // Destruir el proyectil
        }
    }
}
