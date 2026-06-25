using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerCheckpoint : MonoBehaviour //attached to player and used to save last acquired checkpoints and load them
{
    //THIS SCRIPT IS ATTACHED TO PLAYER AND CONTAINS LAST CHECKPOINT WHICH IS UPDATED BY CHECKPOINT PREFAB
    public Vector3 lastCheckpoint;

    private void Awake()
    {
        lastCheckpoint = transform.position; //default checkpoint on player spawn
    }

    public void loadCheckpoint() //used by killzone to teleport to last acquired checkpoint 
    {
        CharacterController cc = GetComponent<CharacterController>(); //Controller needs to be disabled to teleport.

        cc.enabled = false;
        transform.position = lastCheckpoint;
        cc.enabled = true;

        Debug.Log("Player teleported to last checkpoint");
    }
}
