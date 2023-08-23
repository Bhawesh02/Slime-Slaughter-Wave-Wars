
using UnityEngine;

public class SoundService : MonoSingletonGeneric<SoundService>
{
    [Header("Sound Source")]
    [SerializeField]
    private AudioSource backgroundSound;
    [SerializeField]
    private AudioSource sfxSound;

    [Header("SFX Audio Clips")]
    
    public AudioClip Click;
    public AudioClip Hurt;
    public AudioClip Slash;
    public AudioClip PickUp;
    [Header("Background Audio Clip")]
    public AudioClip BgSoundClip;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        PlayBg();
    }
    public void PlayBg()
    {
        backgroundSound.clip = BgSoundClip;
        backgroundSound.Play();
    }

    public void PlaySfx(AudioClip audioClip)
    {
        sfxSound.clip = audioClip;
        sfxSound.Play();
    }


}
