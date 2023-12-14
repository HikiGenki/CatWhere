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
    private Gameplay gameplay;

    [SerializeField]
    private GameReadyStartScreen readyStartScreen;

    [SerializeField]
    private GameOverScreen gameOverScreen;

    private HUD hud;
    private AudioManager audioManager;

    private float timer = 1f;
    private float currentTimerSpeed;

    public static bool GameRunning;

    private Item activeItem => gameplay.ActiveItem;

    #region Unity Events

    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Init
        currentTimerSpeed = settings.timerSpeed;
    }

    private IEnumerator Start()
    {
        hud = HUD.Instance;
        audioManager = AudioManager.Instance;

        ResetTimer();

        yield return null;
    }

    private void Update()
    {
        UpdateTimer();
    }

    #endregion

    #region Main gameplay

    public void GoToRoom(int index)
    {
        GameRunning = false;
        gameplay.ShowRoom(index, OnRoomLoadingFinished);
        readyStartScreen.Play();
    }

    private void UpdateTimer()
    {
        if (!GameRunning)
        {
            return;
        }

        hud.UpdateBar(timer);
        hud.ToggleTimerPulse((timer < 0.3f));

        if (timer <= 0f)
        {
            GameOver();
        }

        timer -= currentTimerSpeed * Time.deltaTime;
    }

    private void ShowNewItem()
    {
        if (gameplay.ShowNewItem())
        {
            hud.DisplayGoalItem(activeItem);
            ResetTimer();
        }
    }


    public void ClickedOnItem(Item clickedItem)
    {
        if (!GameRunning)
        {
            return;
        }

        if (clickedItem == activeItem)
        {
            audioManager.PlaySfxCorrectGuess();

            Debug.Log("Clicked on correct item");
            currentTimerSpeed += settings.timerSpeedIncrease;
            gameplay.FoundItem();
            ShowNewItem();
            
        }
        else
        {
            audioManager.PlaySfxWrongGuess();
            Debug.Log("Clicked on WRONG item");
        }
    }

    #endregion

    #region Start sequence

    private void OnRoomLoadingFinished()
    {
        hud.RevealTimerGroup(StartTimer);
    }

    private void StartTimer()
    {
        GameRunning = true;
        ShowNewItem();
    }

    #endregion

    #region GameOver
    public void GameOver()
    {
        Debug.Log("Game over");
        audioManager.PlaySfxGameOver();

        GameRunning = false;
        gameOverScreen.Play(GoToNextLevel);
    }

    private void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    #region Util

    private void ResetTimer()
    {
        timer = 1f;
        hud.UpdateBar(timer);
    }
  
    #endregion
}