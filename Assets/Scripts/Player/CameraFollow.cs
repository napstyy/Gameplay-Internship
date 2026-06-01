using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 3f;
    public float distance = 5f;
    public float height = 2f;

    private float _yaw;
    private float _pitch;

    private void Update()
    {

        if (ManagerScene.Instance != null && ManagerScene.Instance.GetGameState() == GameState.Play)
        {
            _yaw += Input.GetAxis("Mouse X") * sensitivity;
            _pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            _pitch = Mathf.Clamp(_pitch, -20f, 60f);

            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            transform.position = target.position + rotation * new Vector3(0f, height, -distance);
            transform.LookAt(target.position + Vector3.up * height);
        }
    }
}