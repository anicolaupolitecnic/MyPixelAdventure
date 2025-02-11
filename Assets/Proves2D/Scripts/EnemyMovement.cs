using UnityEditor.Animations;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Animator anim;
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;
    public float lostSightTime = 2f;  // Tiempo antes de volver a patrullar
    public float distanceDetection = 10f;
    public LayerMask playerLayer;

    private Transform target;
    private bool chasingPlayer = false;
    private float lostSightTimer = 0f;

    void Start()
    {
        target = pointA;
        anim = GetComponent<Animator>();
        anim.SetBool("Run", true);
        FlipDirection();
    }

    void Update()
    {
        //if (DetectPlayer())
        //{
        //    chasingPlayer = true;
        //    lostSightTimer = 0f;  // Reiniciar el temporizador
        //}
        //else if (chasingPlayer)
        //{
        //    lostSightTimer += Time.deltaTime;
        //    if (lostSightTimer >= lostSightTime)
        //    {
        //        chasingPlayer = false;
        //        target = GetClosestPatrolPoint(); // Volver a patrullar desde el punto más cercano
        //    }
        //}

        //if (chasingPlayer)
        //{
        //    ChasePlayer();
        //}
        //else
        {
            Patrol();
        }
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
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < distanceDetection)
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
        transform.Rotate(0, 180, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }
}
