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

    bool masterMute;
    bool bgmMute;
    bool effectMute;
    bool voiceMute;

    float currentMasterSound;
    float currentBGMSound;
    float currentEffectSound;
    float currentVoiceSound;

    Slider masterSlider;
    Slider bgmSlider;
    Slider effectSlider;
    Slider voiceSlider;
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
    }


    protected override void _Init()
    {
        audioMixer = Resources.Load<AudioMixer>("AudioMixer/FORAudio");
        LoadAllSource("BGM", "Effect", "Voice");
        audios = GameObject.Find("Main Camera").GetComponents<AudioSource>();

        audios[(int)Enum_SoundType.BGM].loop = true;
        audios[(int)Enum_SoundType.EffectBGM].loop = true;
    }

    public void VolumeSetting()
    {
        SetMasterVolume(masterSlider.value);
        SetMasterVolume(bgmSlider.value);
        SetMasterVolume(effectSlider.value);
        SetMasterVolume(voiceSlider.value);
    }


    private void LoadAllSource(string _bgm, string _effect, string _voice)
    {
        AudioClip[] bgmSource;
        AudioClip[] effectSource;
        AudioClip[] voiceSource;

        bgmSource = Resources.LoadAll<AudioClip>(_bgm);
        effectSource = Resources.LoadAll<AudioClip>(_effect);
        voiceSource = Resources.LoadAll<AudioClip>(_voice);
 
        LoadSource(bgmSource, bgm);
        LoadSource(effectSource, effect);
        LoadSource(voiceSource, voice);

    }

    private void LoadSource(AudioClip[] source,Dictionary<string, AudioClip> sourceClips)
    {
        for (int i = 0; i < source.Length; i++)
        {
            string name = source[i].name;
            AudioClip sourceClip = source[i];

            sourceClips.Add(name, sourceClip);
        }
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
    public void SliderSetting(Slider _masterSlider, Slider _bgmSlider, Slider _effectSlider, Slider _voiceSlider)
    {
        masterSlider = _masterSlider;
        bgmSlider = _bgmSlider;
        effectSlider = _effectSlider;
        voiceSlider = _voiceSlider;

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        effectSlider.onValueChanged.AddListener(SetEffectVolume);
        voiceSlider.onValueChanged.AddListener(SetVoiceVolume);

        VolumeSetting();
    }

    public void SetMute(string soundSource, bool value)
    {
        switch (soundSource)
        {
            case "Master":
                MasterMuteSound();
                break;
            case "BGM":
                BGMMuteSound();
                break;
            case "Effect":
                EffectMuteSound();
                break;
            case "Voice":
                VoiceMuteSound();
                break;
        }
    }


    public void MasterMuteSound()
    {
        if (masterMute)
        {         
            masterMute = false;
            SetMasterVolume(currentMasterSound);
        }
        else
        {
            float previousMasterSound = currentMasterSound;
            SetMasterVolume(0.001f);
            masterMute = true;
            currentMasterSound = previousMasterSound;
        }
    }
    public void BGMMuteSound()
    {
        if (bgmMute)
        {
            bgmMute = false;
            SetBGMVolume(currentBGMSound);
        }
        else
        {
            float previousBGMSound = currentBGMSound;
            SetBGMVolume(0.001f);
            bgmMute = true;
            currentBGMSound = previousBGMSound;
        }
    }
    public void EffectMuteSound()
    {     
        if (effectMute)
        {
            effectMute = false;
            SetEffectVolume(currentEffectSound);
        }
        else
        {
            float previousEffectSound = currentEffectSound;
            SetEffectVolume(0.001f);
            effectMute = true;
            currentEffectSound = previousEffectSound;
        }
    }
    public void VoiceMuteSound()
    {
       
        if (voiceMute)
        {
            voiceMute = false;
            SetVoiceVolume(currentVoiceSound);
        }
        else
        {
            float previousVoiceSound = currentVoiceSound;
            SetVoiceVolume(0.001f);
            voiceMute = true;
            currentVoiceSound = previousVoiceSound;
        }
    }
    public void SetMasterVolume(float volume)
    {
        currentMasterSound = volume;
        
        if (!masterMute)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
    }

    public void SetBGMVolume(float volume)
    {
        currentBGMSound = volume;
        
        if (!bgmMute)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        }
    }
    public void SetEffectVolume(float volume)
    {
        currentEffectSound = volume;
       
        if (!effectMute)
        {
            audioMixer.SetFloat("Effect", Mathf.Log10(volume) * 20);
        }
    }
    public void SetVoiceVolume(float volume)
    {
        currentVoiceSound = volume;
       
        if (!voiceMute)
        {
            audioMixer.SetFloat("Voice", Mathf.Log10(volume) * 20);
        }
    }
    public void SetVolume(string sound, float volume)
    {
        audioMixer.SetFloat(sound, Mathf.Log10(volume) * 20);
    }
}
