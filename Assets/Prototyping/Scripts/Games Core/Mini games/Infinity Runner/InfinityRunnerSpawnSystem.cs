using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prototyping.Games {
    public class InfinityRunnerSpawnSystem : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnableObjects;
        [SerializeField] private int spawnCount;
        [SerializeField] private float gap;

        private PlayerRunnerManager manager;
        private IInfinityRunnerSpawnable lastSpawned;
        private Transform lastSpawnedTransform;

        private bool runSystem;

        private void Update()
        {
            Spawn();
        }

        public void Initialize(PlayerRunnerManager playerRunnerManager)
        {
            manager = playerRunnerManager;
            InitialSpawn();
        }

        void InitialSpawn()
        {
            float spawnGap = transform.position.z;
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject gameObject = Instantiate(spawnableObjects[Random.Range(0, spawnableObjects.Length)]);
                IInfinityRunnerSpawnable spawnable = gameObject.GetComponent<IInfinityRunnerSpawnable>();
                spawnable.SetManager(manager);
                float x = transform.position.x;
       
                float currentLength = spawnable.Length / 2f;
                float lastLength = lastSpawned == null ? 0 : lastSpawned.Length / 2f;
                float offset = gap + currentLength + lastLength;
                if (i == 0) {
                    offset = 0;
                }
                spawnGap += offset;
                gameObject.transform.position = new Vector3(x, transform.position.y, spawnGap);
                lastSpawned = spawnable;
                lastSpawnedTransform = gameObject.transform;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, lastSpawnedTransform.position.z + lastSpawned.Length / 2f);
        }

        void Spawn()
        {
            if (manager.IsPlaying)
            {
                if (transform.position.z - lastSpawnedTransform.position.z > gap + lastSpawned.Length / 2f)
                {
                    GameObject gameObject = Instantiate(spawnableObjects[Random.Range(0, spawnableObjects.Length)]);
                    IInfinityRunnerSpawnable spawnable = gameObject.GetComponent<IInfinityRunnerSpawnable>();
                    spawnable.SetManager(manager);
                    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + spawnable.Length / 2f);
                    lastSpawned = spawnable;
                    lastSpawnedTransform = gameObject.transform;
                }
            }
        }

    }
}