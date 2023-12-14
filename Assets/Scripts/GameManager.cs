using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance;

    [SerializeField]
    private GameSettings settings;

    [SerializeField]
    private RoomsManager roomsManager;

    [SerializeField]
    private GameReadyStartScreen readyStartScreen;

    [SerializeField]
    private GameOverScreen gameOverScreen;

    private HUD hud;
    private AudioManager audioManager;

    private bool isEndlessMode;
    public static bool GameRunning;

    private int roomsCompleted = 0;
    private float gameTime = 1f;
    private float countDownTimer = 1f;
    private float currentCountDownTimerSpeed;
    private int currentRoomIndex;

    private Item activeItem => roomsManager.ActiveItem;

    public int GameTime => Mathf.FloorToInt(gameTime);
    public int RoomsCompleted => roomsCompleted;

    #region Unity Events

    private void Awake()
    {
        instance = this;

        //Init
        currentCountDownTimerSpeed = settings.countDownTimerSpeed;
    }

    private IEnumerator Start()
    {
        hud = HUD.Instance;
        audioManager = AudioManager.Instance;

        Reset();

        yield return null;
    }

    private void Update()
    {
        UpdateTimer();
    }

    #endregion

    private void Reset()
    {
        SetCountDownTimer(1f);
        roomsCompleted = 0;
    }

    #region Button click

    public void OnClickStartGameInRoom(int roomIndex)
    {
        OnShowRoom(roomIndex);
    }

    public void OnClickStartGameInRapidMode()
    {
        isEndlessMode = true;
        OnClickStartGameInRoom(0);
    }

    #endregion

    #region Game Start Sequence

    private void OnShowNextRoom()
    {
        OnShowRoom(roomsManager.NextRoomIndex);
    }

    private void OnShowRoom(int roomIndex)
    {
        GameRunning = false;

        roomsManager.ShowRoom(roomIndex,
            (roomsCompleted == 0) ? PlayGameStartSequence : RevealHUD);
    }

    private void PlayGameStartSequence()
    {
        readyStartScreen.Play(RevealHUD);
    }

    private void RevealHUD()
    {
        hud.RevealHUDGroup(OnGamePlayStart);
    }

    #endregion

    #region Main gameplay

    public void ClickedOnItem(Item clickedItem)
    {
        if (!GameRunning)
        {
            return;
        }

        if (clickedItem == activeItem)
        {
            audioManager.PlaySfxCorrectGuess();

            currentCountDownTimerSpeed += settings.timerSpeedIncrease;
            roomsManager.FoundItem();
            ShowNewItem();

        }
        else
        {
            audioManager.PlaySfxWrongGuess();
        }
    }

    private void UpdateTimer()
    {
        if (!GameRunning)
        {
            return;
        }

        hud.ToggleCountDownTimerPulse(countDownTimer < 0.45f);

        if (countDownTimer <= 0f)
        {
            GameOver();
        }

        SetCountDownTimer(countDownTimer - currentCountDownTimerSpeed * Time.deltaTime);
    }

    private void ShowNewItem()
    {
        if (roomsManager.TryShowNewItem())
        {
            hud.DisplayGoalItem(activeItem);
            SetCountDownTimer(1f);
        }
        else //Found all items
        {
            roomsCompleted++;
            hud.HideHUDGroup();

            if (isEndlessMode)
            {
                OnShowNextRoom();
            }
            else
            {
                SingleRoomModeGameWon();
            }
        }
    }

    #endregion

    #region Start sequence

    private void OnGamePlayStart()
    {
        GameRunning = true;

        ShowNewItem();
    }

    #endregion

    #region GameOver

    private void SingleRoomModeGameWon()
    {
        Debug.Log("Game won");
        GameRunning = false;
        gameOverScreen.PlayGameWon(ReloadGame);
    }

    private void EndlessGameModeRoomCompletion()
    {
        Debug.Log("Rapid Game Won");
        GameRunning = false;
        OnShowNextRoom();
    }

    private void GameOver()
    {
        if (isEndlessMode && roomsCompleted > 0)
        {
            gameOverScreen.PlayEndlessGameWon(ReloadGame);
        }
        GameRunning = false;
        Debug.Log("Game over");

        audioManager.PlaySfxGameOver();

        gameOverScreen.PlayGameOver(ReloadGame);
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    #region Util

    private void SetCountDownTimer(float value)
    {
        countDownTimer = value;
        hud.UpdateCountDownBar(countDownTimer);
    }

    private bool firstTimeStartingGame => !isEndlessMode || roomsCompleted == 0;

    #endregion
}