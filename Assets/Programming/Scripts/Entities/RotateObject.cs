using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private void Update()
    {
        gameObject.transform.Rotate(gameObject.transform.rotation.x, -0.5f, gameObject.transform.rotation.z);
    }
}
