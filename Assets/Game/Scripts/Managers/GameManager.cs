using System;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        // Singleton 인스턴스   
        public static GameManager Instance;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SoundManager.Instance.PlayBGM("artic");
        }
    }
}
