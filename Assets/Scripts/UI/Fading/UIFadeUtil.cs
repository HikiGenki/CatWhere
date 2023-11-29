using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public static class UIFadeUtil
{
    #region Fading of Canvas

    public static IEnumerator FadeInCanvasToOpaque(CanvasGroup canvasGroup, float fadeSpeed, Action callback = null)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        callback?.Invoke();
    }

    public static IEnumerator FadeOutcanvasToTransparent(CanvasGroup canvasGroup, float fadeSpeed, Action callback = null)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        callback?.Invoke();
    }

    public static void SetCanvasToTransparent(CanvasGroup canvasGroup)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
    }

    public static void SetCanvasToOpaque(CanvasGroup canvasGroup)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1f;
    }
    #endregion

    #region Fading of Image (a UI component)

    public static IEnumerator FadeImageVisibility(bool fadeToVisible, Image image, float fadeSpeed, Action callback = null)
    {
        Color c = image.color;

        if (fadeToVisible)
        {
            while (c.a < 1f)
            {
                c.a += fadeSpeed * Time.deltaTime;
                image.color = c;
                yield return null;
            }

            c.a = 1f;
            image.color = c;
            callback?.Invoke();
        }
        else //Fade to clear
        {
            while (c.a > 0f)
            {
                c.a -= fadeSpeed * Time.deltaTime;
                image.color = c;
                yield return null;
            }

            c.a = 0f;
            image.color = c;
            callback?.Invoke();
        }
    }

    public static void SetImageAlphaVisibility(bool setToVisible, Image image)
    {
        Color c = image.color;
        if (setToVisible)
        {
            c.a = 1f;
            image.color = c;
        }
        else
        {
            c.a = 0f;
            image.color = c;
        }
    }

    #endregion
}