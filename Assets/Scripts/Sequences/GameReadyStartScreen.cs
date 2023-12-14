using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameReadyStartScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    
    [SerializeField]
    private Animator animator;

    private AudioSource audioSource;
    private Action onCompleteCallback;

    private int playHash = Animator.StringToHash("Ready");
    private int startHash = Animator.StringToHash("Start");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        UIFadeUtil.SetCanvasToTransparent(canvasGroup);
    }

    public void Play(Action onCompleteCallback = null)
    {
        this.onCompleteCallback = onCompleteCallback;
        StartCoroutine(PlayGameStartSequence());
    }

    private IEnumerator PlayGameStartSequence()
    {
        StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(canvasGroup));

        yield return new WaitForSeconds(0.5f);

        audioSource.Play();
        animator.Play(playHash);

        yield return new WaitForSeconds(1f);

        animator.Play(startHash);

        StartCoroutine(UIFadeUtil.FadeOutcanvasToTransparent(canvasGroup));

        yield return new WaitForSeconds(0.5f);

        onCompleteCallback?.Invoke();
    }
}