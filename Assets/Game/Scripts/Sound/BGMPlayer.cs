using DG.Tweening;
using Game.Scripts.Managers;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource _audioSource;
    private Transform _transform;
    private float originalVolume;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _transform = GetComponent<Transform>();
    }

    public void Play(AudioClip clip, float volume)
    {
        _audioSource.loop = true;
        _audioSource.clip = clip;
        _audioSource.volume = volume * SoundManager.Instance.MasterVolumeFX;
        originalVolume = volume;
        _audioSource.Play();
    }

    public void Mute()
    {
        _audioSource.volume = 0f;
    }

    public void LerpMute()
    {
        _audioSource.DOFade(0f, SoundManager.Instance.BGMLerpDuration);
    }
    
    public void UnMute()
    {
        _audioSource.volume = originalVolume * SoundManager.Instance.MasterVolumeBGM;
    }

    public void LerpUnMute()
    {
        _audioSource.DOFade(originalVolume * SoundManager.Instance.MasterVolumeBGM, SoundManager.Instance.BGMLerpDuration);
    }

    public void SetVolume()
    {
        _audioSource.DOFade(originalVolume * SoundManager.Instance.MasterVolumeBGM, SoundManager.Instance.BGMLerpDuration);
    }
}