using UnityEngine;

public class PlayButton : MonoBehaviour
{
    //This script can be used later to also add a continue button, but for now it is only used to trigger the change of scene when the play button is pressed in the main menu
    public void OnPressPlay()
    {
        ManagerScene.Instance.OnChangeSceneTriggered(1);
    }
}
