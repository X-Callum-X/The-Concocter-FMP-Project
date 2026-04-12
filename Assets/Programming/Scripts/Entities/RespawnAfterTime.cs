using System.Collections;
using UnityEngine;

public class RespawnAfterTime : MonoBehaviour
{
    public GameObject objToRespawn;

    public float timeTakenToRespawn;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BossPlatform"))
        {
            StartCoroutine(RespawnObject());
        }
    }

    private IEnumerator RespawnObject()
    {
        Debug.Log("Respawning...");

        yield return new WaitForSeconds(timeTakenToRespawn);

        Instantiate(objToRespawn, transform.position, Quaternion.identity);

        Debug.Log("Spawned");

    }
}
