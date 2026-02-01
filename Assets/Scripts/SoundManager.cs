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

    float musicVolume = 1f;
    float sfxVolume = 1f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volumes
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        sfxSource.volume = sfxVolume;

        ApplyVolumes();
    }

    void OnEnable()
    {
        ApplyVolumes();
    }


    void ApplyVolumes()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
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
    //public void PlayMusic(string clipName, float volume = 1f)
    //{
    //    AudioClip clip = GetClip(musicClips, clipName);
    //    if (clip == null) return;

    //    if (musicSource.clip == clip) return;

    //    musicSource.clip = clip;
    //    musicSource.volume = volume;
    //    musicSource.loop = true;
    //    musicSource.Play();
    //}
    public void PlayMusic(string clipName)
    {
        AudioClip clip = GetClip(musicClips, clipName);
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;

        // ALWAYS re-apply saved volume
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSource.volume = musicVolume;

        musicSource.Play();
    }


    // ---------------- PLAY SFX ----------------
    //public void PlaySFX(string clipName, float volume = 1f)
    //{
    //    AudioClip clip = GetClip(sfxClips, clipName);
    //    if (clip == null) return;

    //    sfxSource.PlayOneShot(clip, volume);
    //}
    public void PlaySFX(string clipName, float volumeMultiplier = 1f)
    {
        AudioClip clip = GetClip(sfxClips, clipName);
        if (clip == null) return;

        float finalVolume = sfxVolume * volumeMultiplier;

        sfxSource.PlayOneShot(clip, finalVolume);
    }

    // ----------- SLIDER FUNCTIONS -----------
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;

        //sfxSource.volume = value;

        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
}
