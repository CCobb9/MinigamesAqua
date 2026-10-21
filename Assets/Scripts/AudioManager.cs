
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Background Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.5f;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxSource;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    [Header("Looping SFX")]
    [SerializeField] private AudioSource loopSfxSource;

    [Header("Sound Library")]
    [SerializeField] private AudioLibrary soundLibrary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError(
                "AudioManager: Asigna los AudioSource en el Inspector.",
                this
            );
            return;
        }

        if (loopSfxSource != null)
        {
            loopSfxSource.playOnAwake = false;
            loopSfxSource.loop = true;
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
    }

    private void Start()
    {
        if (backgroundMusic != null)
            PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }


    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PlaySFX(string soundName)
    {
        if (sfxSource == null || soundLibrary == null)
        {
            Debug.LogWarning("AudioManager: Falta asignar el AudioSource o la biblioteca.");
            return;
        }

        if (soundLibrary.TryGetSound(soundName, out AudioLibrary.Sound sound))
        {
            sfxSource.PlayOneShot(sound.clip, sfxVolume * sound.volume);
        }
        else
        {
            Debug.LogWarning($"AudioManager: No existe el sonido '{soundName}'.");
        }
    }


    public void PlayLoopSFX(string soundName)
    {
        if (loopSfxSource == null || soundLibrary == null)
            return;

        if (!soundLibrary.TryGetSound(
            soundName, out AudioLibrary.Sound sound))
            return;

        if (loopSfxSource.isPlaying &&
            loopSfxSource.clip == sound.clip)
            return;

        loopSfxSource.Stop();
        loopSfxSource.clip = sound.clip;
        loopSfxSource.volume = sfxVolume * sound.volume;
        loopSfxSource.loop = true;
        loopSfxSource.Play();
    }

    public void StopLoopSFX(string soundName)
    {
        if (loopSfxSource == null || soundLibrary == null)
            return;

        if (!soundLibrary.TryGetSound(
            soundName, out AudioLibrary.Sound sound))
            return;

        if (loopSfxSource.clip == sound.clip)
            loopSfxSource.Stop();
    }


    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }
}
