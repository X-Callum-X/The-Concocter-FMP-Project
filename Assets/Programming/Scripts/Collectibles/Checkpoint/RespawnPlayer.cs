using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public GameObject playerObj;

    public GameObject UIScreen;

    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerMovement = playerObj.GetComponent<PlayerMovement>();
        playerHealth = playerObj.GetComponent<PlayerHealth>();
    }


    public void Respawn()
    {
        UIScreen.gameObject.SetActive(false);

        playerObj.transform.position = playerMovement.placeToRespawn;

        playerHealth.currentHealth = playerHealth.maxHealth;

        playerHealth.UpdateHealthUI();

        Time.timeScale = 1.0f;
    }
}
