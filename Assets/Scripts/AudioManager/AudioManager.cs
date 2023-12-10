using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    protected AudioSource bgmPlayer;

    [SerializeField]
    protected AudioSource sfxPlayer;

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

    protected void PlaySoundEffect(AudioClip clip)
    {
        sfxPlayer.PlayOneShot(clip);
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

    public void SetBackgroundMusicVolume(float volume)
    {
        if (bgmPlayer != null)
        {
            bgmPlayer.volume = volume;
        }
    }

    public void SetSoundEffectVolume(float volume)
    {
        if (sfxPlayer != null)
        {
            sfxPlayer.volume = volume;
        }
    }
}