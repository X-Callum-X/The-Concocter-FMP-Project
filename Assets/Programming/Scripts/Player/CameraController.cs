using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _camera;
    public float cameraSens = 120f;

    private float limitX = 0;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, mouseX * cameraSens * Time.deltaTime, 0));

        limitX += mouseY * cameraSens * Time.deltaTime;
        limitX = Mathf.Clamp(limitX, -90f, 90f);

        _camera.localRotation = Quaternion.Euler(-limitX, 0, 0);
    }
}

