using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    //THIS SCRIPT IS ATTACHED TO A CHECKPOINT PREFAB AND UPDATES PLAYER'S LAST KNOWN CHECKPOINT

    [SerializeField] private MeshRenderer[] editorIndicators; // transparent meshes to debug checkpoint location in edit mode (deletes itself on awake)

    private void Awake()
    {
        foreach (MeshRenderer spriteRenderer in editorIndicators) //delete debug on awake
        {
            Destroy(spriteRenderer);
        }

    }
    void OnTriggerEnter(Collider col) //update player's last checkpoint
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerCheckpoint>().lastCheckpoint = transform.position;
            Debug.Log("New Checkpoint Acquired");
        }
    }
}

