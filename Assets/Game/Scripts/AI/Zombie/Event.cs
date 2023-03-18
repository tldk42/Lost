using System;
using Game.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Scripts.AI.Zombie
{
    public class Event : MonoBehaviour
    {

        [SerializeField]private Object[] Colliders;
        
        public void ATK_CollisionEnable()
        {
            foreach (Object o in Colliders)
            {
                SoundManager.Instance.PlaySound("Attack");
                o.GetComponent<SphereCollider>().enabled = true;
            }
        }

        public void ATK_CollisionDisable()
        {
            foreach (Object o in Colliders)
            {
                o.GetComponent<SphereCollider>().enabled = false;
            }
        }
    }
}