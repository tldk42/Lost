using UnityEngine;

namespace Game.Scripts.AI.Zombie
{
    public class OnHit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("HIT!!");
                // Player player = other.GetComponent<Player>();
                // player._animator.Die();
                // StartCoroutine(GameManager.Instance.GameOver());
            }
        }
    }
}