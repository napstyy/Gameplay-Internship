using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public int antCount = 6;
    public float spawnRadius = 6f;
    public Vector3 spawnCenter = Vector3.zero;
    public float minScale = 0.2f;
    public float maxScale = 0.5f;
    public float minSpeed = 1.5f;
    public float maxSpeed = 3f;
    public float patrolRadius = 1.5f;
    public float detectionRadius = 5f;
    public float groupRadius = 3f;
    public int groupThreshold = 2;
    public float fleeDistance = 6f;

    private void Start()
    {
        SpawnAnts();
    }

    private void SpawnAnts()
    {
        for (int i = 0; i < Mathf.Max(1, antCount); i++)
        {
            SpawnAnt(i);
        }
    }

    private void SpawnAnt(int index)
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = spawnCenter + new Vector3(randomCircle.x, 0f, randomCircle.y);

        GameObject ant = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        ant.name = $"Ant_{index}";
        ant.transform.position = spawnPosition;
        float scale = Random.Range(minScale, maxScale);
        ant.transform.localScale = new Vector3(scale, scale, scale);

        Rigidbody rb = ant.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        Collider collider = ant.GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = true;

        var antMovement = ant.AddComponent<AntMovement>();
        antMovement.moveSpeed = Random.Range(minSpeed, maxSpeed);
        antMovement.patrolRadius = patrolRadius;
        antMovement.detectionRadius = detectionRadius;
        antMovement.groupRadius = groupRadius;
        antMovement.groupThreshold = groupThreshold;
        antMovement.fleeDistance = fleeDistance;
        antMovement.homePosition = spawnPosition;

        ant.tag = "Enemy";

        var renderer = ant.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.Lerp(Color.black, Color.gray, Random.value);
        }
    }
}
