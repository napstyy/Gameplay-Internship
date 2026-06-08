using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector3 spawnPosition = Vector3.zero;

    private void Start()
    {
        SpawnTestEnemy();
    }

    private void SpawnTestEnemy()
    {
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Cube);
        enemy.name = "TestEnemy";
        enemy.transform.position = spawnPosition;
        enemy.AddComponent<EnemyMovement>();
        enemy.tag = "Enemy";

        var renderer = enemy.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }
}
