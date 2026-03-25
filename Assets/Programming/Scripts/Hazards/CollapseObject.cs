using System.Collections;
using UnityEngine;

public class CollapseObject : MonoBehaviour
{
    bool isFalling;

    float fallSpeed = 0;

    public float timeToFall = 0;

    private float fallTimer = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isFalling = true;

            Destroy(gameObject, 5);
        }
    }

    private void Update()
    {
        if (isFalling)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= timeToFall)
            {
                fallSpeed += Time.deltaTime / 20;
                transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, transform.position.z);
            }
        }
    }
}
