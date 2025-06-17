using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private ParallaxSettingsUI pSUI;

    public void PlayGame()
    {
        if (pSUI != null)
        {
            pSUI.SaveParallaxSettings();
        }
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
