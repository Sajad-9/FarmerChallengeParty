using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioDatabase Database;

    public AudioSource audioSource;
    public AudioSource sfxSource;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this);

        DontDestroyOnLoad(this.gameObject);
        Database?.Initialize();
    }
    public void PlaySFX(AudioClip iSFX)
    {
        sfxSource.clip = iSFX;
        sfxSource.Play();
    }
    public void PlayMusic(AudioClip iMusic)
    {
        audioSource.clip = iMusic;
        audioSource.Play();
    }
    public void PlaySFX(string iSFX)
    {
        AudioClip clip = Database.GetMusic(name);
        if (clip == null) return;

        sfxSource.clip = clip;
        sfxSource.Play();
    }
    public void PlayMusic(string iMusic)
    {
        AudioClip clip = Database.GetSFX(name);
        if(clip == null) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}
