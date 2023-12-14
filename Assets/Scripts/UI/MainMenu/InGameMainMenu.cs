using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class InGameMainMenu : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 1f;

    private GameManager gm;

    [Header("Canvas Groups")]
    [SerializeField]
    private CanvasGroup rootCanvas;
    [SerializeField]
    private CanvasGroup canvasMainMenu;
    [SerializeField]
    private CanvasGroup canvasInfo;
    [SerializeField]
    private CanvasGroup canvasLevelSelection;

    [Header("Previous Completion Times")]
    [SerializeField]
    private TextMeshProUGUI game1Time;
    [SerializeField]
    private TextMeshProUGUI game2Time;
    [SerializeField]
    private TextMeshProUGUI game3Time;

    private Animator animator;
    private AudioManager audioManager;

    private bool transitioning;

    private int targetRoomIndex;

    private int ZoomInHash = Animator.StringToHash("ZoomIn");
    private int ZoomOutHash = Animator.StringToHash("ZoomOut");

    #region Initialization

    private void Awake()
    {
        animator = GetComponent<Animator>();
        UIFadeUtil.SetCanvasToTransparent(rootCanvas);
        UIFadeUtil.SetCanvasToTransparent(canvasMainMenu);
        UIFadeUtil.SetCanvasToTransparent(canvasLevelSelection);
        UIFadeUtil.SetCanvasToTransparent(canvasInfo);
    }

    private void Start()
    {
        gm = GameManager.Instance;
        audioManager = AudioManager.Instance;

        Debug.Log("convert 30 to minute sec " + StringUtil.SecondsToMinuteSeconds(30));
        Debug.Log("convert 120 to minute sec " + StringUtil.SecondsToMinuteSeconds(120));

        game1Time.text = StringUtil.SecondsToMinuteSeconds(PlayerPrefs.GetInt("Game1Time", 0));
        game2Time.text = StringUtil.SecondsToMinuteSeconds(PlayerPrefs.GetInt("Game2Time", 0));
        game3Time.text = StringUtil.SecondsToMinuteSeconds(PlayerPrefs.GetInt("Game3Time", 0));

        ZoomIn();
    }

    private void ZoomIn()
    {
        if (!transitioning)
        {
            StartCoroutine(ZoomInSequence());
        }
    }

    private IEnumerator ZoomInSequence()
    {
        transitioning = true;

        animator.Play(ZoomInHash);
        StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(rootCanvas, fadeSpeed));
        yield return UIFadeUtil.FadeInCanvasToOpaque(canvasMainMenu, fadeSpeed);

        transitioning = false;
    }

    #endregion

    #region Main menu

    public void MainMenuToLevelSelection()
    {
        audioManager.PlaySfxButtonClick();
        UIFadeUtil.SetCanvasToTransparent(canvasMainMenu);
        UIFadeUtil.SetCanvasToOpaque(canvasLevelSelection);
    }

    public void LevelSelectionToMainMenu()
    {
        audioManager.PlaySfxButtonClick();
        UIFadeUtil.SetCanvasToTransparent(canvasLevelSelection);
        UIFadeUtil.SetCanvasToOpaque(canvasMainMenu);
    }

    public void MainMenuToInfo()
    {
        audioManager.PlaySfxButtonClick();
        UIFadeUtil.SetCanvasToTransparent(canvasMainMenu);
        UIFadeUtil.SetCanvasToOpaque(canvasInfo);
    }

    public void InfoToMainMenu()
    {
        audioManager.PlaySfxButtonClick();
        UIFadeUtil.SetCanvasToTransparent(canvasInfo);
        UIFadeUtil.SetCanvasToOpaque(canvasMainMenu);
    }
    #endregion

    #region Level selection

    public void ClickedEnterLevel(int index)
    {
        if (!transitioning)
        {
            audioManager.PlaySfxStartGame();
            targetRoomIndex = index;
            StartCoroutine(StartGameAndHideMenu());
        }
    }

    private IEnumerator StartGameAndHideMenu()
    {
        transitioning = true;

        animator.Play(ZoomOutHash);

        StartCoroutine(UIFadeUtil.FadeOutcanvasToTransparent(rootCanvas, fadeSpeed));
        yield return UIFadeUtil.FadeOutcanvasToTransparent(canvasLevelSelection, fadeSpeed);

        gm.OnClickStartGameInRoom(targetRoomIndex);
        transitioning = false;
    }

    #endregion
}