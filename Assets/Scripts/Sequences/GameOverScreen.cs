using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Image blackScreen;

    private Action onCompleteCallback;
    private AudioManager audioManager;

    private int playHash = Animator.StringToHash("GameOver");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        UIFadeUtil.SetCanvasToTransparent(canvasGroup);
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void Play(Action onCompleteCallback = null)
    {
        this.onCompleteCallback = onCompleteCallback;
        StartCoroutine(PlayGameOverSequence());
    }

    private IEnumerator PlayGameOverSequence()
    {
        StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(canvasGroup));
        audioManager.PlaySfxGameOver();

        yield return new WaitForSeconds(0.5f);

        animator.Play(playHash);
    }

    public void ClickedReplayButton()
    {
        StartCoroutine(UIFadeUtil.FadeImageVisibility(true, blackScreen, 
            fadeSpeed: 1f, 
            callback: () => onCompleteCallback?.Invoke()));
    }
}