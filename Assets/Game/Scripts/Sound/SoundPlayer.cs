using System.Collections;
using Game.Scripts.Managers;
using UnityEngine;

namespace Game.Scripts.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        public AudioSource AudioSource;
        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        public void SoundPlayOneShot(AudioClip clip, float volume)
        {
            StartCoroutine(DespawnSound(clip.length));
            AudioSource.mute = false;
            AudioSource.PlayOneShot(clip, volume);
        }
    
        public void SoundPlayLoop(AudioClip clip, float volume)
        {
            AudioSource.clip = clip;
            AudioSource.volume = volume;
            AudioSource.loop = true;
            AudioSource.Play();
        }

        IEnumerator DespawnSound(float time)
        {
            yield return new WaitForSeconds(time);
            AudioSource.mute = true;
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}