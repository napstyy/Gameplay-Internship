using UnityEngine;

public class AntMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolRadius = 2f;
    public float destinationTolerance = 0.2f;
    public float detectionRadius = 5f;
    public float groupRadius = 3f;
    public int groupThreshold = 2;
    public float fleeDistance = 6f;
    public float targetInterval = 2f;
    public Vector3 homePosition;

    private Transform player;
    private Vector3 patrolTarget;
    private float nextTargetTime;
    private enum State { Patrol, Chase, Flee }
    private State currentState = State.Patrol;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (homePosition == Vector3.zero)
            homePosition = transform.position;
        SetNewPatrolTarget();
    }

    private void Update()
    {
        bool canSee = player != null && CanSeePlayer();
        int nearbyCount = CountNearbyAnts();

        if (canSee)
        {
            if (nearbyCount >= groupThreshold)
                currentState = State.Chase;
            else
                currentState = State.Flee;
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
            case State.Flee:
                FleeFromPlayer();
                break;
        }
    }

    private void Patrol()
    {
        if (Time.time >= nextTargetTime || Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(patrolTarget.x, 0f, patrolTarget.z)) <= destinationTolerance)
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

    private void FleeFromPlayer()
    {
        if (player == null)
            return;

        Vector3 away = (transform.position - player.position).normalized;
        Vector3 target = transform.position + away * fleeDistance;
        MoveTowards(target);
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 horizontalTarget = new Vector3(target.x, transform.position.y, target.z);
        Vector3 direction = (horizontalTarget - transform.position).normalized;
        Vector3 nextPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = nextPosition;
        if (direction != Vector3.zero)
            transform.forward = direction;
    }

    private void SetNewPatrolTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        patrolTarget = homePosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
        nextTargetTime = Time.time + targetInterval;
    }

    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.sqrMagnitude > detectionRadius * detectionRadius)
            return false;

        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, toPlayer.normalized, out RaycastHit hit, detectionRadius))
        {
            return hit.transform == player;
        }

        return false;
    }

    private int CountNearbyAnts()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, groupRadius);
        int count = 0;
        foreach (var c in cols)
        {
            if (c.gameObject == this.gameObject) // include self
            {
                count++;
                continue;
            }
            if (c.GetComponent<AntMovement>() != null)
                count++;
        }

        return count;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(homePosition == Vector3.zero ? transform.position : homePosition, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, groupRadius);
    }
}
