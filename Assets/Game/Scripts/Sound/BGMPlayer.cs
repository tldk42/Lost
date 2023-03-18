using DG.Tweening;
using Game.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class BGMPlayer : MonoBehaviour
{
     public AudioSource AudioSource;
    private Transform _Transform;
    private float _OriginalVolume;
    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        _Transform = GetComponent<Transform>();
    }

    public void Play(AudioClip clip, float volume)
    {
        AudioSource.loop = true;
        AudioSource.clip = clip;
        AudioSource.volume = volume * SoundManager.Instance.MasterVolumeFX;
        _OriginalVolume = volume;
        AudioSource.Play();
    }

    public void Mute()
    {
        AudioSource.volume = 0f;
    }

    public void LerpMute()
    {
        AudioSource.DOFade(0f, SoundManager.Instance.BGMLerpDuration);
    }
    
    public void UnMute()
    {
        AudioSource.volume = _OriginalVolume * SoundManager.Instance.MasterVolumeBGM;
    }

    public void LerpUnMute()
    {
        AudioSource.DOFade(_OriginalVolume * SoundManager.Instance.MasterVolumeBGM, SoundManager.Instance.BGMLerpDuration);
    }

    public void SetVolume()
    {
        AudioSource.DOFade(_OriginalVolume * SoundManager.Instance.MasterVolumeBGM, SoundManager.Instance.BGMLerpDuration);
    }
}