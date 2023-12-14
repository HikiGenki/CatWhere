using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField]
    protected AudioSource bgmPlayer;

    [SerializeField]
    protected AudioSource sfxPlayer;

    [Header("Audio Clips")]
    [SerializeField]
    protected AudioClip sfxButtonPress;

    [SerializeField]
    protected AudioClip sfxStartGame;

    [SerializeField]
    protected AudioClip sfxCorrectGuess;

    [SerializeField]
    protected AudioClip sfxWrongGuess;

    [SerializeField]
    protected AudioClip sfxGameOver;

    [Space]
    [SerializeField]
    protected float volumnChangeSpeed = 5f;

    protected bool isBGMPlaying;
    protected Coroutine coroutine;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSoundEffectVolume(float volume)
    {
        if (sfxPlayer != null)
        {
            sfxPlayer.volume = volume;
        }
    }

    #region BGM

    public void SetBackgroundMusicVolume(float volume)
    {
        if (bgmPlayer != null)
        {
            bgmPlayer.volume = volume;
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(PlayNewBGM(clip));
    }

    protected IEnumerator PlayNewBGM(AudioClip newClip)
    {
        if (bgmPlayer.clip != null)
        {
            if (bgmPlayer.clip == newClip)
            {
                yield break;
            }

            //Dim music
            while (bgmPlayer.volume > 0f)
            {
                bgmPlayer.volume -= Time.deltaTime * volumnChangeSpeed;
                yield return null;
            }

            bgmPlayer.volume = 0f;
        }

        bgmPlayer.clip = newClip;
        bgmPlayer.Play();
    }

    protected void StopActiveCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
    #endregion

    #region Sfx
    public void PlaySfxStartGame() => PlaySoundEffect(sfxStartGame);
    public void PlaySfxCorrectGuess() => PlaySoundEffect(sfxCorrectGuess);
    public void PlaySfxWrongGuess() => PlaySoundEffect(sfxWrongGuess);
    public void PlaySfxButtonClick() => PlaySoundEffect(sfxButtonPress);
    public void PlaySfxGameOver() => PlaySoundEffect(sfxGameOver);
    
    protected void PlaySoundEffect(AudioClip clip)
    {
        sfxPlayer.PlayOneShot(clip);
    }

    #endregion
}