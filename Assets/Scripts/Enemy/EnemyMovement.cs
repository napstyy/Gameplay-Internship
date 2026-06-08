using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float patrolRadius = 5f;
    public float destinationTolerance = 0.2f;
    public float detectionRadius = 6f;
    public LayerMask obstacleMask;

    private Transform player;
    private Vector3 patrolTarget;
    private float nextTargetTime;
    private float targetInterval = 2.5f;
    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SetNewPatrolTarget();
    }

    private void Update()
    {
        if (player != null && CanSeePlayer())
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Patrol;
        }

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                ChasePlayer();
                break;
        }
    }

    private void Patrol()
    {
        if (Time.time >= nextTargetTime || Vector3.Distance(transform.position, patrolTarget) <= destinationTolerance)
        {
            SetNewPatrolTarget();
        }

        MoveTowards(patrolTarget);
    }

    private void ChasePlayer()
    {
        if (player == null)
            return;

        MoveTowards(player.position);
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    private void SetNewPatrolTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        patrolTarget = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        nextTargetTime = Time.time + targetInterval;
    }

    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.sqrMagnitude > detectionRadius * detectionRadius)
            return false;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, toPlayer.normalized, out RaycastHit hit, detectionRadius, ~obstacleMask))
        {
            return hit.transform == player;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
