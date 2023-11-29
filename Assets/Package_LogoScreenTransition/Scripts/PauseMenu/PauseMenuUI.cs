using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuUI : MonoBehaviour
{
    public static bool IsPaused = false;

    [SerializeField] string sc_MainMenuName;
    [SerializeField] float timeBeforeInitialFadeOut = 2.5f; //Need to wait for Generic black fader to fade it.

    //Canvas groups
    [SerializeField] CanvasGroup pauseMenu;
    [SerializeField] CanvasGroup quitConfirmMenu;

    [SerializeField] Scene MenuScene;
    [SerializeField] CanvasGroupFader blackFader;

    bool inSceneTransition = true;

    #region Public
    public void ToStartMenu()
    {
        if (!inSceneTransition)
        {
            inSceneTransition = true;
            blackFader.FadeIn(() => SceneManager.LoadScene(sc_MainMenuName));
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

        //Pause
        if (IsPaused)
        {
            UIFadeUtil.SetCanvasToOpaque(pauseMenu);
            Time.timeScale = 0f;
        }
        //Unpause
        else
        {
            Time.timeScale = 1f;
            UIFadeUtil.SetCanvasToTransparent(pauseMenu);
        }
    }
    #endregion

    #region Mono
    void Awake()
    {
        //Hide pause menu.
        UIFadeUtil.SetCanvasToTransparent(pauseMenu);
        UIFadeUtil.SetCanvasToTransparent(quitConfirmMenu);
    }

    IEnumerator Start ()
    {
        //Wait time before allowing for pausing. 
        yield return new WaitForSeconds(timeBeforeInitialFadeOut);
        inSceneTransition = false;
    }

    void Update()
    {
        if (!inSceneTransition && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    #endregion

    #region Scene transition
    #endregion
}
