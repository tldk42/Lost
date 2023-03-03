using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "AnimalData", menuName = "Data/Animal", order = 0)]
    public class AnimalData : UnityEngine.ScriptableObject
    {
        [BoxGroup("Animal Info"), LabelWidth(100)]
        public string Name;
    }
}