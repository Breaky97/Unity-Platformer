using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] music;
    public AudioSource[] sfx;

    public int levelMusicToPlay;

    private int currentTrack;

    public AudioMixerGroup musicMixer, sfxMixer;

    

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(levelMusicToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    currentTrack++;
        //    PlayMusic(currentTrack);
        //}
        
    }

    public void PlayMusic(int musicToPlay)
    {
        for(int i = 0; i < music.Length; i++)
        {
            music[i].Stop();
        }

        music[musicToPlay].Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Play();
    }

    public void SetMusicLevel()
    {
        musicMixer.audioMixer.SetFloat("musicVol", UIManager.instance.musicVolSlider.value);
    }

    public void SetSFXLevel()
    {
        musicMixer.audioMixer.SetFloat("sfxVol", UIManager.instance.sfxVolSlider.value);
    }
}
