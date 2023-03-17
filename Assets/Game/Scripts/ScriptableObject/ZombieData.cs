using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
namespace Game.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "Zombie", menuName = "Data/Zombie", order = 0)][Serializable]
    public class ZombieData : UnityEngine.ScriptableObject
    {
        [BoxGroup("Attribute")] public float Hp;
        [BoxGroup("Attribute")] public float Speed;
    }
}