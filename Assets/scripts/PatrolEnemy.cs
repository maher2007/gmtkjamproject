using UnityEngine;
using System.Collections;
public class PatrolEnemy : Enemy
{
    [Header("Patrol Points")]
    [Tooltip("First point (A) world position.")]
    public Transform pointA;
    [Tooltip("Second point (B) world position.")]
    public Transform pointB;

    [Header("Movement")]
    public float speed = 2f;
    public float waitTimeAtPoint = 0.5f;

    // State flags
    public bool movingToB = true;
    public bool isWaiting = false;

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("SimplePatrol2D: Assign both pointA and pointB.", this);
            enabled = false;
            return;
        }

        // Start at point A
        transform.position = pointA.position;
        StartCoroutine(PatrolLoop());
    }

    private IEnumerator PatrolLoop()
    {
        while (true)
        {
            if (isWaiting)
            {
                yield return null;
                continue;
            }

            Vector3 target = movingToB ? pointB.position : pointA.position;

            // Move toward target
            while ((transform.position - target).sqrMagnitude > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            // Arrived: flip direction after waiting
            yield return StartCoroutine(WaitRoutine());
            movingToB = !movingToB;
        }
    }

    private IEnumerator WaitRoutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        isWaiting = false;
    }

    // Optional: visualize patrol in editor
    private void OnDrawGizmos()
    {
        if (pointA != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointA.position, 0.1f);
        }

        if (pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("player"))
        {
            collision.gameObject.GetComponent<playercontroller>().TakeDamge(5);
        }
    }
}
