using UnityEngine;

public class InsectMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float patrolRadius = 5f;
    public float destinationTolerance = 0.2f;
    public float detectionRadius = 6f;
    public float targetInterval = 2.5f;
    public float flyHeight = 1f;
    public float maxFlyHeight = 2f;
    public float flyBobSpeed = 1.5f;
    public float flyBobAmount = 0.12f;
    public float verticalSpeed = 1.5f;
    public LayerMask obstacleMask;

    private Transform player;
    private Vector3 patrolTarget;
    private float nextTargetTime;
    private float baseHeight;
    private float targetHeight;
    private float heightChangeTimer;
    private float lastDamageTime;
    private float damageCooldown = 0.5f;
    private float damageAmount = 5f;
    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        baseHeight = flyHeight;
        targetHeight = flyHeight;
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

    private void MoveTowards(Vector3 target)
    {
        Vector3 horizontalTarget = new Vector3(target.x, transform.position.y, target.z);
        Vector3 direction = (horizontalTarget - transform.position).normalized;
        Vector3 nextPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        
        // Update vertical movement
        heightChangeTimer -= Time.deltaTime;
        if (heightChangeTimer <= 0)
        {
            targetHeight = Random.Range(flyHeight * 0.7f, maxFlyHeight);
            heightChangeTimer = Random.Range(2f, 4f);
        }
        
        // Smoothly interpolate to target height
        baseHeight = Mathf.Lerp(baseHeight, targetHeight, verticalSpeed * Time.deltaTime);
        
        float bob = Mathf.Sin(Time.time * flyBobSpeed) * flyBobAmount;
        nextPosition.y = baseHeight + bob;
        
        // Ensure we don't exceed max height
        nextPosition.y = Mathf.Min(nextPosition.y, maxFlyHeight);
        
        transform.position = nextPosition;

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

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterMovement characterMovement = collision.gameObject.GetComponent<CharacterMovement>();
            float playerY = collision.transform.position.y;
            float insectY = transform.position.y;
            
            if (characterMovement != null && playerY > insectY && characterMovement.IsJumping())
            {
                Die();
                return;
            }
            
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                CharacterManager characterManager = collision.gameObject.GetComponent<CharacterManager>();
                if (characterManager != null)
                {
                    characterManager.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
