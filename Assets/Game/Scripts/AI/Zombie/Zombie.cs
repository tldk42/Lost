using System;
using Game.Scripts.ScriptableObject;
using UnityEngine;
namespace Game.Scripts.AI.Zombie
{
    public class Zombie : MonoBehaviour
    {
        public ZombieData Data;

        private void Awake()
        {
            Data = Resources.Load<ZombieData>("Zombie");
        }

    }
}