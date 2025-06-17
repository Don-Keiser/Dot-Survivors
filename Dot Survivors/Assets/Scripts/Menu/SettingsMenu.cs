using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject parallaxSettingsUI;
    [SerializeField] private GameObject graphicsSettingsUI;
    [SerializeField] private GameObject audioSettingsUI;
    [SerializeField] private GameObject controlsSettingsUI;

    [Header("Buttons")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button parallaxButton;
    [SerializeField] private Button graphicsButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button controlsButton;

    [Header("Current Settings Panel")]
    [SerializeField] private GameObject currentSettingsPanel;

    [SerializeField] private ParallaxSettingsUI pSUI;

    private void Start()
    {
        settingsButton.onClick.AddListener(OpenSettingsMenu);
        backButton.onClick.AddListener(BackToMainMenu);
        parallaxButton.onClick.AddListener(OpenParallaxSettings);
        graphicsButton.onClick.AddListener(OpenGraphicsSettings);
        audioButton.onClick.AddListener(OpenAudioSettings);
        controlsButton.onClick.AddListener(OpenControlsSettings);
        currentSettingsPanel = parallaxSettingsUI;
    }

    private void OpenSettingsMenu()
    {
        settingsMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);
        currentSettingsPanel.SetActive(true);
    }

    private void BackToMainMenu()
    {
        pSUI.SaveParallaxSettings();
        settingsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    private void OpenParallaxSettings()
    {
        currentSettingsPanel.SetActive(false);
        currentSettingsPanel = parallaxSettingsUI;
        currentSettingsPanel.SetActive(true);
    }

    private void OpenGraphicsSettings()
    {
        currentSettingsPanel.SetActive(false);
        currentSettingsPanel = graphicsSettingsUI;
        currentSettingsPanel.SetActive(true);
    }

    private void OpenAudioSettings()
    {
        currentSettingsPanel.SetActive(false);
        currentSettingsPanel = audioSettingsUI;
        currentSettingsPanel.SetActive(true);
    }

    private void OpenControlsSettings()
    {
        currentSettingsPanel.SetActive(false);
        currentSettingsPanel = controlsSettingsUI;
        currentSettingsPanel.SetActive(true);
    }
}