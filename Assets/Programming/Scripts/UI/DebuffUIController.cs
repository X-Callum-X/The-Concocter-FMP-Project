using UnityEngine;
using UnityEngine.UI;

public class DebuffUIController : MonoBehaviour
{
    [Header("References")]
    public Image fireDebuff;
    public Image iceDebuff;
    public Image poisonDebuff;

    [Header("Variables")]
    [HideInInspector] public bool isOnFire = false;
    [HideInInspector] public bool isOnIce = false;
    [HideInInspector] public bool isPoisoned = false;

    private void Update()
    {
        if (isOnFire)
        {
            fireDebuff.gameObject.SetActive(true);
        }
        else
        {
            fireDebuff.gameObject.SetActive(false);
        }

        if (isOnIce)
        {
            iceDebuff.gameObject.SetActive(true);
        }
        else
        {
            iceDebuff.gameObject.SetActive(false);
        }

        if (isPoisoned)
        {
            poisonDebuff.gameObject.SetActive(true);
        }
        else
        {
            poisonDebuff.gameObject.SetActive(false);
        }
    }
}
