using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject hud;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            hud.SetActive(true);
            pauseMenu.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
        else
        {
            hud.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void ResumeGame()
    {
        hud.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title Screen");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Ensure time scale is reset
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
