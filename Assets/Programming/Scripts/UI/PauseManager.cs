using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [HideInInspector] public bool isPaused;

    public GameObject pauseMenuObj;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && Time.deltaTime != 0)
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

        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuObj.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;

        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuObj.SetActive(false);
    }
}
