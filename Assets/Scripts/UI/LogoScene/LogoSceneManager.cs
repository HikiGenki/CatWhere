using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoSceneManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup logoCanvasGroup;
    [SerializeField]
    private int nextSceneIndex = 1;
    [SerializeField]
    private float fadeSpeed = 1f;

    [Space]
    [SerializeField]
    private float initialWait = 0.5f;
    [SerializeField]
    private float stayVisibleDuration = 1.5f;

    private void Awake ()
    {
        logoCanvasGroup.alpha = 0f;
    }

    private IEnumerator Start()
    {
        //Fade in logo
        yield return new WaitForSeconds(initialWait);
        yield return StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(logoCanvasGroup, fadeSpeed));

        //Fade out logo
        yield return new WaitForSeconds(stayVisibleDuration);
        yield return StartCoroutine(UIFadeUtil.FadeOutcanvasToTransparent(logoCanvasGroup, fadeSpeed));

        SceneManager.LoadScene(nextSceneIndex);
    }
}