using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    public UIController ui;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ui.LoadScene("Floor 1");
        }
    }
}
