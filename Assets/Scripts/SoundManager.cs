using UnityEngine;

public enum SoundType
{
    //BackgroundMusic,
    MainMenuMusic,
    PillFillMusic,
    RefillRushMusic,
    MixAFixMusic,

    // SFX
    YesSound,
    NoSound,
    LevelComplete,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public AudioClip[] soundList;
    //private AudioSource audioSource;

    private AudioSource musicSource;
    private AudioSource sfxSource;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length < 2)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            musicSource = sources[0];
            sfxSource = sources[1];
        }

        musicSource.loop = true;
    }

    public static void PlayMusic(SoundType sound, float volume = 1f)
    {
        if (instance.musicSource.clip == instance.soundList[(int)sound])
            return; // already playing

        instance.musicSource.clip = instance.soundList[(int)sound];
        instance.musicSource.volume = volume;
        instance.musicSource.Play();
    }

    public static void PlaySFX(SoundType sound, float volume = 1f)
    {
        instance.sfxSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }


    //public static void PlaySound(SoundType sound, float volume = 1)
    //{
    //    instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    //}
}
