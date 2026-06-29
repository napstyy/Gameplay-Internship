using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    public int spiderCount = 3;
    public float spawnRadius = 6f;
    public Vector3 spawnCenter = Vector3.zero;
    public float minScale = 0.5f;
    public float maxScale = 1.2f;
    public float minSpeed = 1.5f;
    public float maxSpeed = 3.5f;
    public float patrolRadius = 3f;
    public float detectionRadius = 6f;
    public float maxChaseDistanceFromHome = 8f;

    private void Start()
    {
        SpawnSpiders();
    }

    private void SpawnSpiders()
    {
        for (int i = 0; i < Mathf.Max(1, spiderCount); i++)
        {
            SpawnSpider(i);
        }
    }

    private void SpawnSpider(int index)
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = spawnCenter + new Vector3(randomCircle.x, 0f, randomCircle.y);

        GameObject spider = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spider.name = $"Spider_{index}";
        spider.transform.position = spawnPosition;
        float scale = Random.Range(minScale, maxScale);
        spider.transform.localScale = Vector3.one * scale;

        Rigidbody rb = spider.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        Collider collider = spider.GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = true;

        var spiderMovement = spider.AddComponent<SpiderMovement>();
        spiderMovement.moveSpeed = Random.Range(minSpeed, maxSpeed);
        spiderMovement.patrolRadius = patrolRadius;
        spiderMovement.detectionRadius = detectionRadius;
        spiderMovement.homePosition = spawnPosition;
        spiderMovement.maxChaseDistanceFromHome = maxChaseDistanceFromHome;

        spider.tag = "Enemy";

        var renderer = spider.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.black;
        }
    }
}
