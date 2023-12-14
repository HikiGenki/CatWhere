using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [Header("HUD")]
    [SerializeField]
    private TextMeshProUGUI goalItemText;

    [SerializeField]
    private Image goalItemImage;

    [Space]
    [SerializeField]
    private CanvasGroup timerGroup;

    [SerializeField]
    private TextMeshProUGUI hoverItemName;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Image timerBar;

    [Header("Pulsing")]
    private float pulseSpeed = 5f;
    private Color pulseColor = Color.red;

    private GameManager GM;

    //Timer pulse
    private bool pulseOn;
    private float pulseTimer = TimerStartValue;

    //The lowest point on sin curve
    private const float TimerStartValue = Mathf.PI * 1.5f;

    #region Unity Events

    private void Awake()
    {
        Instance = this;
        Reset();
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
            pulseTimer += Time.deltaTime * pulseSpeed;
            UpdateTimerPulseColor();
        }
    }

    #endregion

    #region Timer bar

    public void RevealTimerGroup(Action callback = null)
    {
        StartCoroutine(UIFadeUtil.FadeInCanvasToOpaque(timerGroup, 1f, callback));
    }

    private void UpdateTimerPulseColor()
    {
        timerBar.color = Color.Lerp(Color.white, pulseColor, Mathf.Sin(pulseTimer) * 0.5f + 0.5f);
    }

    public void ToggleTimerPulse(bool setActive)
    {
        if (!pulseOn && setActive)
        {
            pulseOn = true;
            UpdateTimerPulseColor();
        }

        if (pulseOn && !setActive)
        {
            pulseOn = false;
            pulseTimer = TimerStartValue;
            UpdateTimerPulseColor();
        }
    }

    public void UpdateBar(float percentage)
    {
        timerBar.fillAmount = percentage;
    }

    #endregion

    public void Reset()
    {
        timerBar.fillAmount = 1f;
        pulseTimer = TimerStartValue;

        goalItemText.text = "";
        goalItemImage.enabled = false;
    }

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