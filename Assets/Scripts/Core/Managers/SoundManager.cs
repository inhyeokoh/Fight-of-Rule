using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : SubClass<GameManager>
{ 
    public enum Enum_SoundType
    {
        BGM,
        Effect,
        EffectBGM,
        Voice,
    }

    Slider masterSlider;
    AudioMixer audioMixer;
    AudioSource[] audios;
    Dictionary<string, AudioClip> bgm = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> effect = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> effectBGM = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> voice = new Dictionary<string, AudioClip>();

    protected override void _Clear()
    {
    }

    protected override void _Excute()
    {
        //masterSlider.onValueChanged.AddListener()

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

  
    protected override void _Init()
    {
        audioMixer = Resources.Load<AudioMixer>("AudioMixer/FORAudio");
        masterSlider = _board.slider;

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        AudioClip[] bgmSource;
        bgmSource = Resources.LoadAll<AudioClip>("BGM");

        for (int i = 0; i < bgmSource.Length; i++)
        {
            string name = bgmSource[i].name;
            AudioClip source = bgmSource[i];

            bgm.Add(name, source);
        }   
        audios = GameObject.Find("Main Camera").GetComponents<AudioSource>();
      
        audios[(int)Enum_SoundType.BGM].loop = true;
        audios[(int)Enum_SoundType.EffectBGM].loop = true;

        SoundChanage("추억의 길");
         
    }

    public void SoundAllClear()
    {
        for (int i = 0; i < audios.Length; i++)
        {
            audios[i].Stop();
            
            if (audios[i].clip != null)
            {
                audios[i].clip = null;
            }
        }
    }

    public void SoundPlayOnShot(string soundClip, Enum_SoundType soundType = Enum_SoundType.BGM)
    {
        switch (soundType)
        {             
            case Enum_SoundType.Effect:
                audios[(int)soundType].PlayOneShot(effect[soundClip]);
                break;
            case Enum_SoundType.Voice:
                audios[(int)soundType].PlayOneShot(voice[soundClip]);
                break;
            default:
                return;
        }
    }


    public void SoundChanage(string soundClip, Enum_SoundType soundType = Enum_SoundType.BGM)
    {
        if (audios[(int)soundType].clip != null)
        {
            audios[(int)soundType].clip = null;
            audios[(int)soundType].Stop();
        }

        switch (soundType)
        {
            case Enum_SoundType.BGM:             
                audios[(int)soundType].clip = bgm[soundClip];
                audios[(int)soundType].Play();
                break;
            case Enum_SoundType.Effect:
                audios[(int)soundType].clip = effect[soundClip];
                audios[(int)soundType].Play();
                break;
            case Enum_SoundType.EffectBGM:
                audios[(int)soundType].clip = effectBGM[soundClip];
                audios[(int)soundType].Play();
                break;
            case Enum_SoundType.Voice:
                audios[(int)soundType].clip = voice[soundClip];
                audios[(int)soundType].Play();
                break;
        }

    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    public void SetEffectVolume(float volume)
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(volume) * 20);
    }
    public void SetEffectBGMVolume(float volume)
    {
        audioMixer.SetFloat("EffectBGM", Mathf.Log10(volume) * 20);
    }

    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("Voice", Mathf.Log10(volume) * 20);
    }
    public void SetVolume(string sound, float volume)
    {
        audioMixer.SetFloat(sound, Mathf.Log10(volume) * 20);
    }
}
