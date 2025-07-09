using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Tambahkan ini untuk mengakses UI

public class SoundsManager : Singleton<SoundsManager>
{
    public enum Sound
    {
        BackgroundMusic,
        PlayerFootsteps,
        PlayerSwordSwing,
        PlayerSwordHit,
        PlayerDamaged,
        EnemyDamaged,
        TreasureChestOpen,
        PopUpInteract,
        Checkpoint,
        Dialogue
    }
    AudioSource myAudio;
    public SoundAudioClip[] soundAudioClipArray;
    private AudioSource source;

    [SerializeField] private Slider volumeSlider; // Tambahkan Slider

    void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    //Destroy(gameObject);
        //}
        //else
        //{
        //    instance = this;
        //    //DontDestroyOnLoad(gameObject);
        //}

        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();

        // Set nilai volume dari PlayerPrefs (jika ada)
        if (PlayerPrefs.HasKey("GameVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("GameVolume");
            SetVolume(savedVolume);
            if (volumeSlider != null)
            {
                volumeSlider.value = savedVolume; // Update UI slider
            }
        }
    }

    public void PlayClip(Sound sound)
    {
        if (myAudio == null) myAudio = GetComponent<AudioSource>();
        if (SaveManager.GetInstance() != null)
        {
            myAudio.PlayOneShot(GetAudioClip(sound), SaveManager.GetInstance().SavedVolumeValue);
        }
        else
        {
            myAudio.PlayOneShot(GetAudioClip(sound), 1);
        }
    }

    AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " was not found!");
        return null;
    }

    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }

    // Fungsi untuk mengatur volume
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Atur volume global
        PlayerPrefs.SetFloat("GameVolume", volume); // Simpan pengaturan volume
    }
}

[System.Serializable]
public class SoundAudioClip
{
    public SoundsManager.Sound sound;
    public AudioClip audioClip;
}
