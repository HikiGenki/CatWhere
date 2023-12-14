using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [Header("Goal")]
    [SerializeField]
    private TextMeshProUGUI goalItemText;

    [SerializeField]
    private Animator clockAnimator;

    [SerializeField]
    private Image goalItemImage;

    [Header("Timer")]
    [SerializeField]
    private CanvasGroup timerGroup;

    [SerializeField]
    private TextMeshProUGUI hoverItemName;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Image timerBar;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Pulsing")]
    private float pulseSpeed = 5f;
    private Color pulseColor = Color.red;
    private Color startColor;

    private GameManager GM;
    private StringBuilder sb;

    //Timer pulse
    private bool pulseOn;
    private float pulseTimer;

    //Animator hash
    private readonly int PlayHash = Animator.StringToHash("Play");
    private readonly int StopHash = Animator.StringToHash("Stop");

    #region Unity Events

    private void Awake()
    {
        Instance = this;

        sb = new StringBuilder();

        startColor = timerBar.color;
        timerBar.fillAmount = 1f;

        goalItemText.text = "";
        goalItemImage.enabled = false;

        UIFadeUtil.SetCanvasToTransparent(timerGroup);
    }

    private void Start()
    {
        GM = GameManager.Instance;
    }

    private void Update()
    {
        if (pulseOn && GameManager.GameRunning)
        {
            pulseTimer += Time.deltaTime;
            UpdateTimerPulseColor();
        }
    }

    #endregion

    #region Timer bar

    public void RevealHUDGroup(Action callback = null)
    {
        StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(timerGroup, 1f, callback));
    }

    public void HideHUDGroup(Action callback = null)
    {
        StartCoroutine(UIFadeUtil.FadeOutcanvasToTransparent(timerGroup, 1f, callback));
    }

    private void UpdateTimerPulseColor()
    {
        timerBar.color = Color.Lerp(startColor, pulseColor, 
            Mathf.Abs(Mathf.Sin(pulseTimer * pulseSpeed)));
    }

    public void ToggleCountDownTimerPulse(bool setActive)
    {
        if (!pulseOn && setActive)
        {
            clockAnimator.Play(PlayHash);
            pulseOn = true;
            pulseTimer = 0f;
            UpdateTimerPulseColor();
        }

        if (pulseOn && !setActive)
        {
            clockAnimator.CrossFade(StopHash, 1f);
            pulseOn = false;
            pulseTimer = 0f;
            UpdateTimerPulseColor();
        }
    }

    public void UpdateCountDownBar(float percentage)
    {
        timerBar.fillAmount = percentage;
    }

    #endregion

    #region Text text
    
    public void SetTimerText(int sec, int miliSect)
    {
        sb.Clear();
        sb.Append(sec.ToString("D2")).
            Append(":").
            Append(miliSect.ToString());
        timerText.text = sb.ToString();
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    #endregion

    public void DisplayHoverItemName(string name)
    {
        hoverItemName.text = name;
    }

    public void DisplayGoalItem(Item item)
    {
        goalItemText.text = item.gameObject.name;

        goalItemImage.enabled = true;
        goalItemImage.sprite = item.Button.spriteState.highlightedSprite;
    }

    public void AddScore(int valueToAdd, int totalScore)
    {
        scoreText.text = totalScore.ToString();
    }
}