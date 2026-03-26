using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public bool horizontal;
    public bool verticalLeft;
    public bool verticalRight;

    private void Update()
    {
        if (horizontal)
        {
            gameObject.transform.Rotate(gameObject.transform.rotation.x, -90f * Time.deltaTime, gameObject.transform.rotation.z);
        }
        
        if (verticalLeft)
        {
            gameObject.transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, -90f * Time.deltaTime);
        }

        if (verticalRight)
        {
            gameObject.transform.Rotate(-90f * Time.deltaTime, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
        }
    }
}
