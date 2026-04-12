using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public int timeTakenToDestroy;

    private float destroyTimer;

    private void Update()
    {
        destroyTimer += Time.deltaTime;

        if (destroyTimer >= timeTakenToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
