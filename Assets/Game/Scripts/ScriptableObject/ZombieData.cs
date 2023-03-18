using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
namespace Game.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "Zombie", menuName = "Data/Zombie", order = 0)][Serializable]
    public class ZombieData : UnityEngine.ScriptableObject
    {
        [FoldoutGroup("속성"), Range(0, 100)] public float Hp;
        [FoldoutGroup("속성"), Range(0, 5)] public float Speed;
        [FoldoutGroup("속성"), Range(3, 10)] public float FOVRange = 6f;
        [FoldoutGroup("속성"), Range(10, 120)] public float FOV = 60;
        [FoldoutGroup("속성"), Range(0, 10)] public float AttackRange = .5f;
    }
}