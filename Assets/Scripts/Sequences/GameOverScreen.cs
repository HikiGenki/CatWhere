using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private TextMeshProUGUI completionTime;
    [SerializeField]
    private TextMeshProUGUI roomsCompleted;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Image blackScreen;

    private Action onCompleteCallback;
    private AudioManager audioManager;
    private GameManager gm;

    private int playHash = Animator.StringToHash("GameOver");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        UIFadeUtil.SetCanvasToTransparent(canvasGroup);
    }

    private void Start()
    {
        gm = GameManager.Instance;
        audioManager = AudioManager.Instance;
    }

    public void PlayGameWon(Action onCompleteCallback = null)
    {
        audioManager.PlaySfxGameWon();
        completionTime.text = "Time: " + StringUtil.SecondsToMinuteSeconds(gm.GameTime);
        roomsCompleted.text = "";

        StartCoroutine(PlayGameOverSequence());
    }

    public void PlayEndlessGameWon(Action onCompleteCallback = null)
    {
        audioManager.PlaySfxGameWon();
        completionTime.text = "Time: " + StringUtil.SecondsToMinuteSeconds(gm.GameTime);
        roomsCompleted.text = gm.RoomsCompleted + "Rooms completed";

        StartCoroutine(PlayGameOverSequence());
    }

    public void PlayGameOver(Action onCompleteCallback = null)
    {
        audioManager.PlaySfxGameOver();
        completionTime.text = "";
        roomsCompleted.text = "";

        StartCoroutine(PlayGameOverSequence());
    }

    private IEnumerator PlayGameOverSequence()
    {
        StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(canvasGroup));

        yield return new WaitForSeconds(0.5f);

        animator.Play(playHash);
    }

    public void ClickedReplayButton()
    {
        StartCoroutine(UIFadeUtil.FadeImageVisibility(true, blackScreen, 
            fadeSpeed: 1f, 
            callback: onCompleteCallback != null ? 
            onCompleteCallback : 
            () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)));
    }
}