using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        [OdinSerialize] public List<GameObject> PoolPrefabs = new List<GameObject>();

        [OdinSerialize] [DictionaryDrawerSettings(KeyLabel = "Custom Key Name", ValueLabel = "Custom Value Label")]
        public Dictionary<string, Queue<GameObject>> PoolQueues = new Dictionary<string, Queue<GameObject>>();

        private void Awake()
        {
            Instance = this;

            foreach (GameObject poolPrefab in PoolPrefabs)
            {
                InitPool(poolPrefab, 3);
            }
        }

        public void InitPool(GameObject poolPrefab, int count)
        {
            if (poolPrefab == null)
                return;

            if (!PoolQueues.ContainsKey(poolPrefab.name))
            {
                PoolPrefabs.Add(poolPrefab);
                PoolQueues.Add(poolPrefab.name, new Queue<GameObject>());
            }

            for (var i = 0; i < count; i++)
            {
                GameObject go = Instantiate(poolPrefab, transform, true);
                PoolQueues[poolPrefab.name].Enqueue(go);
                go.SetActive(false);
            }
        }

        public GameObject Spawn(string key)
        {
            if (!PoolQueues.ContainsKey(key) || PoolQueues[key].Count <= 0)
                InitPool(PoolPrefabs.Find(x => x.name == key), 3);
            GameObject go = PoolQueues[key].Dequeue();
            go.SetActive(true);
            return go;
        }

        public void Despawn(GameObject obj)
        {
            PoolQueues[obj.name.Split('(')[0]].Enqueue(obj);
            obj.SetActive(false);
        }
    }
}