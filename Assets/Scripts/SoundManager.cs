//using UnityEngine;

////public enum SoundType
////{
////    //BackgroundMusic,
////    MainMenuMusic,
////    PillFillMusic,
////    RefillRushMusic,
////    MixAFixMusic,

////    // SFX
////    YesSound,
////    NoSound,
////    OnPouring,
////    OnReturning,
////    OnDragBegin,
////    OnDragEnd,
////    LevelComplete,
////    Congratulations
////}

////[RequireComponent(typeof(AudioSource))]
////public class SoundManager : MonoBehaviour
////{
////    private static SoundManager instance;

////    [Header("Audio Clips (Order = SoundType enum)")]
////    public AudioClip[] soundList;
////    //private AudioSource audioSource;

////    [Header("Audio Sources")]
////    public AudioSource musicSource;
////    public AudioSource sfxSource;


////    private void Awake()
////    {
////        if (instance != null && instance != this)
////        {
////            Destroy(gameObject);
////            return;
////        }

////        instance = this;
////        DontDestroyOnLoad(gameObject);
////    }

////    // Start is called once before the first execution of Update after the MonoBehaviour is created
////    void Start()
////    {
////        AudioSource[] sources = GetComponents<AudioSource>();

////        if (sources.Length < 2)
////        {
////            musicSource = gameObject.AddComponent<AudioSource>();
////            sfxSource = gameObject.AddComponent<AudioSource>();
////        }
////        else
////        {
////            musicSource = sources[0];
////            sfxSource = sources[1];
////        }

////        musicSource.loop = true;
////    }

////    public static void PlayMusic(SoundType sound, float volume = 1f)
////    {
////        if (instance.musicSource.clip == instance.soundList[(int)sound])
////            return; // already playing

////        instance.musicSource.clip = instance.soundList[(int)sound];
////        instance.musicSource.volume = volume;
////        instance.musicSource.Play();
////    }

////    public static void PlaySFX(SoundType sound, float volume = 1f)
////    {
////        instance.sfxSource.PlayOneShot(instance.soundList[(int)sound], volume);
////    }


////    //public static void PlaySound(SoundType sound, float volume = 1)
////    //{
////    //    instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
////    //}
////}

//public enum MusicType
//{
//    MainMenuMusic,
//    PillFillMusic,
//    RefillRushMusic,
//    MixAFixMusic
//}

//public enum SFXType
//{
//    YesSound,
//    NoSound,
//    OnPouring,
//    OnReturning,
//    OnDragBegin,
//    OnDragEnd,
//    LevelComplete,
//    Congratulations
//}

//public class SoundManager : MonoBehaviour
//{
//    public static SoundManager instance;

//    //[Header("Audio Clips (Order = SoundType enum)")]
//    //public AudioClip[] soundList;

//    [Header("Music Clips")]
//    public AudioClip[] musicClips;

//    [Header("SFX Clips")]
//    public AudioClip[] sfxClips;

//    [Header("Audio Sources")]
//    public AudioSource musicSource;
//    public AudioSource sfxSource;

//    private void Awake()
//    {
//        if (instance != null)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        instance = this;
//        DontDestroyOnLoad(gameObject);
//    }

//    //public static void PlayMusic(SoundType sound, float volume = 1f)
//    //{
//    //    if (instance == null)
//    //    {
//    //        Debug.LogError("SoundManager not found in scene!");
//    //        return;
//    //    }

//    //    AudioClip clip = instance.soundList[(int)sound];

//    //    if (instance.musicSource.clip == clip)
//    //        return;

//    //    instance.musicSource.clip = clip;
//    //    instance.musicSource.volume = volume;
//    //    instance.musicSource.loop = true;
//    //    instance.musicSource.Play();
//    //}

//    //public static void PlaySFX(SoundType sound, float volume = 1f)
//    //{
//    //    if (instance == null) return;

//    //    instance.sfxSource.PlayOneShot(instance.soundList[(int)sound], volume);
//    //}

//    public static void PlayMusic(MusicType music, float volume = 1f)
//    {
//        if (instance == null) return;

//        AudioClip clip = instance.musicClips[(int)music];

//        if (instance.musicSource.clip == clip)
//            return;

//        instance.musicSource.clip = clip;
//        instance.musicSource.volume = volume;
//        instance.musicSource.loop = true;
//        instance.musicSource.Play();
//    }

//    public static void PlaySFX(SFXType sfx, float volume = 1f)
//    {
//        if (instance == null) return;

//        instance.sfxSource.PlayOneShot(
//            instance.sfxClips[(int)sfx],
//            volume
//        );
//    }

//}


using UnityEngine;

[System.Serializable]
public class NamedAudio
{
    public string name;
    public AudioClip clip;
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Music Clips")]
    public NamedAudio[] musicClips;

    [Header("SFX Clips")]
    public NamedAudio[] sfxClips;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---------------- FIND CLIP BY NAME ----------------
    AudioClip GetClip(NamedAudio[] list, string clipName)
    {
        foreach (var item in list)
        {
            if (item.name == clipName)
                return item.clip;
        }

        Debug.LogWarning("Sound not found: " + clipName);
        return null;
    }

    // ---------------- PLAY MUSIC ----------------
    public void PlayMusic(string clipName, float volume = 1f)
    {
        AudioClip clip = GetClip(musicClips, clipName);
        if (clip == null) return;

        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ---------------- PLAY SFX ----------------
    public void PlaySFX(string clipName, float volume = 1f)
    {
        AudioClip clip = GetClip(sfxClips, clipName);
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }
}
