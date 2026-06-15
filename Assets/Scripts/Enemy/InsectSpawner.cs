using UnityEngine;

public class InsectSpawner : MonoBehaviour
{
    public int insectCount = 5;
    public float spawnRadius = 10f;
    public Vector3 spawnCenter = Vector3.zero;
    public float minHeight = 0.5f;
    public float maxHeight = 1.2f;
    public float minScale = 0.18f;
    public float maxScale = 0.32f;
    public float minSpeed = 1f;
    public float maxSpeed = 5f;

    private void Start()
    {
        SpawnInsects();
    }

    private void SpawnInsects()
    {
        for (int i = 0; i < Mathf.Max(1, insectCount); i++)
        {
            SpawnInsect(i);
        }
    }

    private void SpawnInsect(int index)
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = spawnCenter + new Vector3(randomCircle.x, Random.Range(minHeight, maxHeight), randomCircle.y);

        GameObject insect = GameObject.CreatePrimitive(PrimitiveType.Cube);
        insect.name = $"Insect_{index}";
        insect.transform.position = spawnPosition;
        float scale = Random.Range(minScale, maxScale);
        insect.transform.localScale = Vector3.one * scale;

        Rigidbody rb = insect.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        Collider collider = insect.GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = true;

        var insectMovement = insect.AddComponent<InsectMovement>();
        insectMovement.moveSpeed = Random.Range(minSpeed, maxSpeed);
        insectMovement.flyHeight = spawnPosition.y;
        insectMovement.maxFlyHeight = maxHeight;
        insect.tag = "Insect";

        var renderer = insect.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.Lerp(Color.green, Color.yellow, Random.value);
        }
    }
}
