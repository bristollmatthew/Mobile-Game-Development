using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private GameObject PauseButton;
    private InputAction Pause;

    [SerializeField] private GameObject ResumeButton;
    private InputAction Resume;
    public bool isPaused;

    [SerializeField] private GameObject QuitButton;
    private InputAction Quit;

    [SerializeField] private GameObject SettingsButton;
    [SerializeField] private GameObject CloseSettingsButton;
    private InputAction Settings;

    [SerializeField] private GameObject RestScoreButton;
    private InputAction ResetScore;


    public GameObject PausePanel;
    public GameObject SettingsPanel;


    private GameSaveScript SavegamePointer;

    private void Start()
    {
        SavegamePointer = GameSaveScript.First;
        Pause = inputActions.FindAction("Pause");
        Resume = inputActions.FindAction("Resume");
        Quit = inputActions.FindAction("Quit");
        Settings = inputActions.FindAction("Settings");
        ResetScore = inputActions.FindAction("ResetScore");
    }
    void Update()
    {
        if (Pause.WasPressedThisFrame())
        {
            PauseGame();
        }
        if (Resume.WasPressedThisFrame())
        {
            ResumeGame();
        }
        if (Quit.WasPressedThisFrame())
        {
            QuitGame();
        }
        if (Settings.WasPressedThisFrame())
        {
            SettingsMenu();
        }
        if (ResetScore.WasPressedThisFrame())
        {
            SavegamePointer.ClearSave();
        }

    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void PauseGame()
    {
        isPaused = true;
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SettingsMenu()
    {
        if (SettingsPanel.activeSelf == true)
        {
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(true);
        }
        else
        {
            SettingsPanel.SetActive(true);
            PausePanel.SetActive(false);
        }
    }

}
