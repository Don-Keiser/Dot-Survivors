using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        TimeManager.Instance.PauseGame();
    }

    public void ResumeGame() 
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        TimeManager.Instance.ResumeGame();
    }

    public void ReturnToMainMenu() 
    {
        TimeManager.Instance.ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }
}
