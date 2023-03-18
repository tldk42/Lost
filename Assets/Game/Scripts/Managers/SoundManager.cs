using System;
using System.Collections.Generic;
using Game.Scripts.Sound;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Scripts.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public int MaxMainBGMLayerCount = 8;
        public int CurrentMainBGMLayer;
        public float BGMLerpDuration = 0.5f;

        public float MasterVolumeFX = 1f;
        public float MasterVolumeBGM = 1f;

        [ReadOnly] [SerializeField] private AudioClip[] BGMClips;
        [ReadOnly] [SerializeField] private AudioClip[] AudioClips;

        [ReadOnly] private Dictionary<string, AudioClip> _AudioClipsMap = new Dictionary<string, AudioClip>();
        [ReadOnly] private Dictionary<string, AudioClip> _BGMClipMap = new Dictionary<string, AudioClip>();
        [ReadOnly] private Dictionary<string, BGMPlayer> _BGMPlayers = new Dictionary<string, BGMPlayer>();

        private Transform _BGMTransform;
        private Transform _FXTransform;

        private void Awake()
        {
            Instance = this;
            AudioClips = Resources.LoadAll<AudioClip>($"Sound/SFX");
            BGMClips = Resources.LoadAll<AudioClip>($"Sound/BGM");

            _AudioClipsMap = new Dictionary<string, AudioClip>();
            foreach (AudioClip a in AudioClips)
            {
                _AudioClipsMap.Add(a.name, a);
            }

            _BGMClipMap = new Dictionary<string, AudioClip>();
            foreach (AudioClip a in BGMClips)
            {
                _BGMClipMap.Add(a.name, a);
            }

            _BGMPlayers = new Dictionary<string, BGMPlayer>();
        }


        private void Start()
        {
            PoolManager.Instance.InitPool(Resources.Load<GameObject>("Sound/Sound Player"), 10);
            PoolManager.Instance.InitPool(Resources.Load<GameObject>("Sound/BGM Player"), 3);
        }

        public void PlayBGM(string a_name, float a_volume = 1f)
        {
            if (_BGMClipMap.ContainsKey(a_name) == false)
            {
                Debug.Log(a_name + " is not Contained audioClipsDic");
                return;
            }

            BGMPlayer bgmPlayer = PoolManager.Instance.Spawn("BGM Player").GetComponent<BGMPlayer>();
            _BGMPlayers.Add(a_name, bgmPlayer);
            bgmPlayer.Play(_BGMClipMap[a_name], a_volume);
        }

        // 한 번 재생 : 볼륨 매개변수로 지정
        public SoundPlayer PlaySound(string a_name, float a_volume = 1f)
        {
            if (_AudioClipsMap.ContainsKey(a_name) == false)
            {
                Debug.Log(a_name + " is not Contained audioClipsDic");
                return null;
            }

            var soundPlayer = PoolManager.Instance.Spawn("Sound Player").GetComponent<SoundPlayer>();
            soundPlayer.SoundPlayOneShot(_AudioClipsMap[a_name], a_volume * MasterVolumeFX);
            return soundPlayer;
        }

        // 랜덤으로 한 번 재생 : 볼륨 매개변수로 지정
        public void PlayRandomSound(string[] a_nameArray, float a_volume = 1f)
        {
            string l_playClipName;

            l_playClipName = a_nameArray[Random.Range(0, a_nameArray.Length)];

            if (_AudioClipsMap.ContainsKey(l_playClipName) == false)
            {
                Debug.Log(l_playClipName + " is not Contained audioClipsDic");
                return;
            }

            var soundPlayer = PoolManager.Instance.Spawn("SoundPlayer").GetComponent<SoundPlayer>();
            soundPlayer.SoundPlayOneShot(_AudioClipsMap[l_playClipName], a_volume * MasterVolumeFX);
        }

        // 삭제할때는 리턴값은 GameObject를 참조해서 삭제한다. 나중에 옵션에서 사운드 크기 조정하면 이건 같이 참조해서 바뀌어야함..
        public GameObject PlayLoopSound(string a_name, float a_volume = 1f)
        {
            if (_AudioClipsMap.ContainsKey(a_name) == false)
            {
                Debug.Log(a_name + " is not Contained audioClipsDic");
                return null;
            }

            var soundPlayer = PoolManager.Instance.Spawn("SoundPlayer").GetComponent<SoundPlayer>();
            soundPlayer.SoundPlayLoop(_AudioClipsMap[a_name], a_volume * MasterVolumeFX);

            return soundPlayer.gameObject;
        }

        // 주로 전투 종료시 음악을 끈다.
        public void ClearBGM()
        {
            foreach (var bgmPair in _BGMPlayers)
            {
                // PoolManager.Pools["BGMs"].Despawn(bgmPair.Value.transform);
                PoolManager.Instance.Despawn(bgmPair.Value.gameObject);
            }

            _BGMPlayers.Clear();
        }

        // public void StartMainBGM(int firstStartLayerId)
        // {
        //     ClearBGM();
        //     CurrentMainBGMLayer = firstStartLayerId;
        //     for (var i = 1; i <= MaxMainBGMLayerCount; i++)
        //     {
        //         PlayBGM($"Industrial Combat LAYER {i}");
        //     
        //         if (i == firstStartLayerId)
        //             _BGMPlayers[$"Industrial Combat LAYER {i}"].UnMute();
        //         else
        //             _BGMPlayers[$"Industrial Combat LAYER {i}"].Mute();
        //     }
        // }

        #region 옵션에서 볼륨조절

        public void SetVolumeSFX(float a_volume)
        {
            MasterVolumeFX = a_volume;
        }

        public void SetVolumeBGM(float a_volume)
        {
            MasterVolumeBGM = a_volume;
            foreach (var players in _BGMPlayers)
            {
                // if (players.Value.AudioSource.clip.name.Contains("Industrial Combat LAYER")
                //     && players.Value.AudioSource.clip.name != $"Industrial Combat LAYER {currentMainBGMLayer}")
                // {
                //     continue;
                // }
                players.Value.SetVolume();
            }
        }

        #endregion
    }

    [Serializable]
    public class SoundData
    {
        public AudioClip soundClip;
        public float soundVolume = 1f;
    }
}