using System;
using System.Collections;

using UnityEngine;

/// <summary>
/// For fading canvas groups in basic usage.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Fades to transparent when the game loads")]
    private bool fadeOutOnLoad = true;

    [SerializeField]
    private float initialWaitTime = 1f;

    [SerializeField]
    private float fadeInSpeed = 1f;

    [SerializeField]
    private float fadeOutSpeed = 1f;

    private CanvasGroup canvasGroup;

    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;

    #region Monobehavior
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator Start()
    {
        if (fadeOutOnLoad)
        {
            InstantOpaque();
            yield return new WaitForSeconds(initialWaitTime);
            FadeOut();
        }
    }
    #endregion

    #region Public

    public void FadeIn(Action callBack = null)
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }
        fadeInCoroutine = StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(canvasGroup, fadeInSpeed, callBack));
    }

    public void FadeOut(Action callBack = null)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(UIFadeUtil.FadeOutcanvasToTransparent(canvasGroup, fadeOutSpeed, callBack));
    }

    public void InstantOpaque()
    {
        UIFadeUtil.SetCanvasToOpaque(canvasGroup);
    }

    public void InstantTransparent()
    {
        UIFadeUtil.SetCanvasToOpaque(canvasGroup);
    }

    #endregion
}