using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;

    public GameObject pauseMenuObj;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseGame();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            UnpauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuObj.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuObj.SetActive(false);
    }
}
