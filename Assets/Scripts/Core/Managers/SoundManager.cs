using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : SubClass<GameManager>
{ 
    public enum Enum_SoundSlider
    {
        Master,
        BGM,
        Effect,
        Voice,
    }
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
        SetVolume("Master", masterMute, masterSlider.value);
        SetVolume("BGM", bgmMute,bgmSlider.value);
        SetVolume("Effect",effectMute,effectSlider.value);
        SetVolume("Voice",voiceMute,voiceSlider.value);
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

        masterSlider.onValueChanged.AddListener((value) => SetVolume("Master", masterMute, value));
        bgmSlider.onValueChanged.AddListener((value) => SetVolume("BGM",  bgmMute, value));
        effectSlider.onValueChanged.AddListener((value) => SetVolume("Effect",effectMute, value));
        voiceSlider.onValueChanged.AddListener((value) => SetVolume("Voice",voiceMute, value));

        VolumeSetting();
    }

    public void MuteOnOff(Enum_SoundSlider soundSlider, bool muteCheck)
    {
        switch (soundSlider)
        {
            case Enum_SoundSlider.Master:
                MasterMuteSound(muteCheck);
                break;
            case Enum_SoundSlider.BGM:
                BGMMuteSound(muteCheck);
                break;
            case Enum_SoundSlider.Effect:
                EffectMuteSound(muteCheck);
                break;
            case Enum_SoundSlider.Voice:
                VoiceMuteSound(muteCheck);
                break;
        }
    }


    public void MasterMuteSound(bool toggleEvent)
    {
        masterMute = toggleEvent;
        
        if (masterMute)
        {
            float previousMasterSound = currentMasterSound;
            SetVolume("Master", false, 0.001f);
            currentMasterSound = previousMasterSound;
        }
        else
        {
            SetVolume("Master", masterMute, currentMasterSound);
        }
    }
    public void BGMMuteSound(bool toggleEvent)
    {
        bgmMute = toggleEvent;

        if (bgmMute)
        {
            float previousBGMSound = currentBGMSound;
            SetVolume("BGM", false, 0.001f);  
            currentBGMSound = previousBGMSound;
        }
        else
        {
            SetVolume("BGM", bgmMute, currentBGMSound);
        }
    }
    public void EffectMuteSound(bool toggleEvent)
    {
        effectMute = toggleEvent;

        if (effectMute)
        {
            float previousEffectSound = currentEffectSound;
            SetVolume("Effect", false, 0.001f);
            currentEffectSound = previousEffectSound;
         
        }
        else
        {
            SetVolume("Effect", effectMute, currentEffectSound);
        }
    }
    public void VoiceMuteSound(bool toggleEvent)
    {
        voiceMute = toggleEvent;

        if (voiceMute)
        {
            float previousVoiceSound = currentVoiceSound;
            SetVolume("Voice", false, 0.001f);
            currentVoiceSound = previousVoiceSound;
        }
        else
        {
            SetVolume("Voice",voiceMute,currentVoiceSound);
        }
    }
   /* public void SetMasterVolume(float volume)
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
    }*/
    public void SetVolume(string soundName, bool mute, float volume)
    {
        switch (soundName)
        {
            case "Master":
                currentMasterSound = volume;
                break;
            case "BGM":               
                currentBGMSound = volume;
                break;
            case "Effect":
                currentEffectSound = volume;
                break;
            case "Voice":
                currentVoiceSound = volume;
                break;
                
        }   
        if (!mute)
        {
            audioMixer.SetFloat(soundName, Mathf.Log10(volume) * 20);
        }
    }


    /// <summary>
    /// 변경 사항 있을 경우 로컬 저장 + 서버 저장 요청
    /// </summary>
  /*  void SaveVolOptions()
    {
        if (GameManager.Data.volOptions.MasterVol - volSliders[0].value > 0.01f ||
            GameManager.Data.volOptions.BgmVol - volSliders[1].value > 0.01f ||
            GameManager.Data.volOptions.EffectVol - volSliders[2].value > 0.01f || // 값의 차이가 1% 이상 나는 부분이 있다면,
            GameManager.Data.volOptions.VoiceVol - volSliders[3].value > 0.01f)
        {
            GameManager.Data.volOptions.MasterVol = volSliders[0].value;
            GameManager.Data.volOptions.BgmVol = volSliders[1].value;
            GameManager.Data.volOptions.EffectVol = volSliders[2].value;
            GameManager.Data.volOptions.VoiceVol = volSliders[3].value;
            // 저장할 볼륨 서버로 전송
            C_SAVE_VOL_OPTIONS save_vol_options = new C_SAVE_VOL_OPTIONS();
            save_vol_options.VolOptions = GameManager.Data.volOptions;
            GameManager.Network.Send(PacketHandler.Instance.SerializePacket(save_vol_options));
            // TODO : AddPacketPair
        }
    }*/


}
