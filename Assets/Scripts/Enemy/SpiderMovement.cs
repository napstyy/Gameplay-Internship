using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float patrolRadius = 3f;
    public float destinationTolerance = 0.2f;
    public float detectionRadius = 6f;
    public float targetInterval = 2.5f;
    public Vector3 homePosition;
    public float maxChaseDistanceFromHome = 8f;

    private Transform player;
    private Vector3 patrolTarget;
    private float nextTargetTime;
    private enum State { Patrol, Chase, ReturnHome }
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

        switch (currentState)
        {
            case State.Patrol:
                if (canSee && IsWithinHomeChaseRange(player.position))
                {
                    currentState = State.Chase;
                }
                else
                {
                    Patrol();
                }
                break;
            case State.Chase:
                if (!canSee || !IsWithinHomeChaseRange(player.position))
                {
                    currentState = State.ReturnHome;
                }
                else
                {
                    ChasePlayer();
                }
                break;
            case State.ReturnHome:
                ReturnHome();
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

    private void ReturnHome()
    {
        float dist = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(homePosition.x, 0f, homePosition.z));
        if (dist <= destinationTolerance)
        {
            currentState = State.Patrol;
            SetNewPatrolTarget();
            return;
        }

        MoveTowards(homePosition);
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

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, toPlayer.normalized, out RaycastHit hit, detectionRadius))
        {
            return hit.transform == player;
        }

        return false;
    }

    private bool IsWithinHomeChaseRange(Vector3 targetPos)
    {
        float distFromHome = Vector3.Distance(new Vector3(homePosition.x, 0f, homePosition.z), new Vector3(targetPos.x, 0f, targetPos.z));
        return distFromHome <= maxChaseDistanceFromHome;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterManager characterManager = collision.gameObject.GetComponent<CharacterManager>();
            if (characterManager != null)
            {
                characterManager.TakeDamage(10f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(homePosition == Vector3.zero ? transform.position : homePosition, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(homePosition == Vector3.zero ? transform.position : homePosition, maxChaseDistanceFromHome);
    }
}
