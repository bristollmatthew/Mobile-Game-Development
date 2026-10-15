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
    private bool isPaused;

    [SerializeField] private GameObject QuitButton;
    private InputAction Quit;

    public GameObject MenuUI;

    private void Start()
    {
        Pause = inputActions.FindAction("Pause");
        Resume = inputActions.FindAction("Resume");
        Quit = inputActions.FindAction("Quit");
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
        MenuUI.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        isPaused = true;
        MenuUI.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
