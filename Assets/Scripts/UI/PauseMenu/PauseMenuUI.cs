using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static bool IsPaused = false;

    [SerializeField] 
    private int mainMenuSceneIndex = 1;

    //Canvas groups
    [SerializeField] 
    private CanvasGroup pauseMenu;
    [SerializeField] 
    private CanvasGroup quitConfirmMenu;

    [SerializeField] 
    private CanvasGroupFader blackFader;

    private bool inSceneTransition = false;

    private void Awake()
    {
        //Hide pause menu.
        UIFadeUtil.SetCanvasToTransparent(pauseMenu);
        UIFadeUtil.SetCanvasToTransparent(quitConfirmMenu);
    }

    private void Update()
    {
        if (!inSceneTransition && 
            (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            TogglePause();
        }
    }

    public void ToStartMenu()
    {
        if (!inSceneTransition)
        {
            inSceneTransition = true;
            blackFader.FadeIn(ToMainMenu);
        }
    }

    public void ToQuitConfirm()
    {
        UIFadeUtil.SetCanvasToTransparent(pauseMenu);
        UIFadeUtil.SetCanvasToOpaque(quitConfirmMenu);
    }

    public void QuitConfirm(bool doQuit)
    {
        if (doQuit)
        {
            Application.Quit();
        }
        else
        {
            UIFadeUtil.SetCanvasToTransparent(quitConfirmMenu);
            UIFadeUtil.SetCanvasToOpaque(pauseMenu);
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            UIFadeUtil.SetCanvasToOpaque(pauseMenu);
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            UIFadeUtil.SetCanvasToTransparent(pauseMenu);
        }
    }

    private void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}