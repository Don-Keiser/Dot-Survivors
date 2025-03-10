using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
