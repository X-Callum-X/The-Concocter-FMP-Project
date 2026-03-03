using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject waypointA;
    public GameObject waypointB;
    public GameObject platform;

    public float speed = 10f;
    public float delay = 1f;

    private Vector3 targetPosition;

    private void Start()
    {
        platform.transform.position = waypointA.transform.position;
        targetPosition = waypointB.transform.position;

        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            while ((targetPosition - platform.transform.position).sqrMagnitude > 0.01f)
            {
                platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            targetPosition = targetPosition == waypointA.transform.position ? waypointB.transform.position : waypointA.transform.position;
            
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player")
            collision.transform.SetParent(platform.transform);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
