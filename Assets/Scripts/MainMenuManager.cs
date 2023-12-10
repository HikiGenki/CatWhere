using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum MenuStates
{
    MAIN,
    INFO
}

public class MainMenuManager : MonoBehaviour
{
    #region Fields

    private MenuStates currentState = MenuStates.MAIN;

    [Space(10)]
    [Header("References")]
    [SerializeField]
    private CanvasGroup menuCanvas;
    [SerializeField] 
    private CanvasGroup infoCanvas;
    [SerializeField] 
    private CanvasGroupFader blackFader;

    [Space(10)]
    [Header("Black screen fading")]
    [SerializeField] 
    private float initialWait = 0.5f;
    [SerializeField] 
    private float sceneLoadFadeSpeed = 2f;
    [SerializeField] 
    private float fadeSpeed = 30f;

    private Dictionary<MenuStates, CanvasGroup> canvases = new Dictionary<MenuStates, CanvasGroup>();
    private bool inTransition = false;

    #endregion

    #region Monobehavior
    private void Awake()
    {
        menuCanvas.alpha = 0f;

        canvases.Add(MenuStates.MAIN, menuCanvas);
        canvases.Add(MenuStates.INFO, infoCanvas);

        foreach (var canvas in canvases)
        {
            UIFadeUtil.SetCanvasToTransparent(canvas.Value);
        }
    }

    private IEnumerator Start()
    {
        //Fade in menu canvas group
        yield return new WaitForSeconds(initialWait);
        StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(menuCanvas, sceneLoadFadeSpeed));
    }
    #endregion

    #region Public
    public void ToMain()
    {
        if (!inTransition)
        {
            StartCoroutine(MenuChange(MenuStates.MAIN));
        }
    }

    public void ToInfo()
    {
        if (!inTransition)
        {
            StartCoroutine(MenuChange(MenuStates.INFO));
        }
    }

    public void ToQuitGame()
    {
        if (!inTransition)
        {
            StartCoroutine(QuitGameCoroutine());
        }
    }

    public void ToTargetLevel(string levelName)
    {
        if (!inTransition)
        {
            StartCoroutine(GoToLevel(levelName));
        }
    }

    #endregion

    #region Scene transitioning

    private IEnumerator MenuChange(MenuStates targetState, float waitTimeBetweenFading = 0f)
    {
        if (currentState != targetState && !inTransition)
        {
            inTransition = true;

            StartCoroutine(FadeOutCurrentCanvas(fadeSpeed));
            yield return new WaitForSeconds(waitTimeBetweenFading);
            StartCoroutine(FadeInNewCanvas(targetState, fadeSpeed));

            currentState = targetState;

            inTransition = false;
        }
    }

    private IEnumerator GoToLevel(string levelName)
    {
        if (!inTransition)
        {
            inTransition = true;
            blackFader.FadeIn();
            yield return FadeOutCurrentCanvas(sceneLoadFadeSpeed);
            SceneManager.LoadScene(levelName);
        }
    }

    IEnumerator FadeOutCurrentCanvas(float speed)
    {
        yield return StartCoroutine(UIFadeUtil.FadeOutcanvasToTransparent(canvases[currentState], speed));
    }

    IEnumerator FadeInNewCanvas(MenuStates targetState, float speed)
    {
        yield return StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(canvases[targetState], speed));
    }

    IEnumerator QuitGameCoroutine()
    {
        yield return StartCoroutine(FadeOutCurrentCanvas(sceneLoadFadeSpeed));

        Application.Quit();
    }
    #endregion
}