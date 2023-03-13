using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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

        [ReadOnly] private AudioClip[] _BGMClips;
        [ReadOnly] private AudioClip[] _AudioClips;

        [ReadOnly] private Dictionary<string, AudioClip> _AudioClipsMap = new Dictionary<string, AudioClip>();
        [ReadOnly] private Dictionary<string, AudioClip> _BGMClipMap = new Dictionary<string, AudioClip>();
        [ReadOnly] private Dictionary<string, BGMPlayer> _BGMPlayers = new Dictionary<string, BGMPlayer>();

        private Transform _BGMTransform;
        private Transform _FXTransform;

        private void Awake()
        {

            Instance = this;
            _AudioClips = Resources.LoadAll<AudioClip>("");
            _BGMClips = Resources.LoadAll<AudioClip>("");

        }
    }
}