using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Animator anim;
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public float speed;
    public float detectionRange;
    public float lostSightTime;  // Tiempo antes de volver a patrullar
    public float distanceDetection;
    public LayerMask playerLayer;

    private Transform target;
    private bool chasingPlayer = false;
    private float lostSightTimer = 0f;

    private Rigidbody2D rb;

    void Start()
    {
        target = pointA;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        anim.SetBool("Run", true);
        FlipDirection();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    public void KillMyself()
    {
        anim.SetBool("Die", true);
    }

    public void DestroyMyself()
    {
        Destroy(gameObject);
    }

    bool DetectPlayer()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward * detectionRange;

        // Dibuja el raycast en la escena (DEBUG)
        Debug.DrawRay(transform.position, direction, Color.green, 3f);

        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, playerLayer))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    void Patrol()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed; // Move towards the target with velocity

        // Switch target when close enough
        if (Vector2.Distance(transform.position, target.position) < distanceDetection)
        {
            target = target == pointA ? pointB : pointA;
            FlipDirection();
        }
    }

    void ChasePlayer()
    {
        Debug.Log("chase");
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    Transform GetClosestPatrolPoint()
    {
        float distanceToA = Vector3.Distance(transform.position, pointA.position);
        float distanceToB = Vector3.Distance(transform.position, pointB.position);
        return distanceToA < distanceToB ? pointA : pointB;
    }


    void FlipDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the character
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }
}
