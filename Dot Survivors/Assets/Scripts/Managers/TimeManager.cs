using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private int freezeCount = 0;
    private bool isLevelUpPaused = false;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseGame() 
    {
        freezeCount++;
        Time.timeScale = 0f;
    }

    public void ResumeGame() 
    {
        if (freezeCount > 0) 
        {
            freezeCount--;
        }

        if (freezeCount == 0) 
        {
            Time.timeScale = 1f;
        }
    }

    public void PauseForLevelUp()
    {
        if (!isLevelUpPaused)
        {
            isLevelUpPaused = true;
            PauseGame();
        }
    }

    public void ResumeFromLevelUp()
    {
        if (isLevelUpPaused)
        {
            isLevelUpPaused = false;
            ResumeGame();
        }
    }
}
