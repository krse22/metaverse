using UnityEngine;

namespace Prototyping.Games {
    public class InfinityRunnerSpawnSystem : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnableObjects;
        [SerializeField] private int spawnCount;
        [SerializeField] private float gap;
        [SerializeField] private bool invert = false;

        private PlayerRunnerManager manager;
        private InfinityRunnerObject lastSpawned;

        private float initialZ;

        private void Update()
        {
            Spawn();
        }

        public void Initialize(PlayerRunnerManager playerRunnerManager)
        {
            initialZ = transform.position.z;
            manager = playerRunnerManager;
        }

        public void Restart()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
            InitialSpawn();
        }

        void InitialSpawn()
        {
            float spawnGap = transform.position.z;
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject go = Instantiate(spawnableObjects[Random.Range(0, spawnableObjects.Length)]);
                InfinityRunnerObject spawnable = go.GetComponent<InfinityRunnerObject>();
                spawnable.SetManager(manager);
                float x = transform.position.x;
                float currentLength = spawnable.Length / 2f;
                float lastLength = lastSpawned == null ? 0 : lastSpawned.Length / 2f;
                float offset = gap + currentLength + lastLength;
                if (i == 0) {
                    offset = 0;
                }
                spawnGap += offset;
                go.transform.position = new Vector3(x, transform.position.y, spawnGap);
                lastSpawned = spawnable;
                InvertSpawned(go);
                manager.RegisterObject(go);
            } 
            transform.position = new Vector3(transform.position.x, transform.position.y, spawnGap);
        }

        void Spawn()
        {
            if (manager.IsPlaying)
            {
                if (transform.position.z - lastSpawned.transform.position.z > gap + lastSpawned.Length / 2f)
                {
                    GameObject go = Instantiate(spawnableObjects[Random.Range(0, spawnableObjects.Length)]);
                    InfinityRunnerObject spawnable = go.GetComponent<InfinityRunnerObject>();
                    spawnable.SetManager(manager);
                    go.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + spawnable.Length / 2f);
                    lastSpawned = spawnable;
                    InvertSpawned(go);
                    manager.RegisterObject(go);
                }
            }
        }

        void InvertSpawned(GameObject go)
        {
            if (invert)
            {
                go.transform.localScale = new Vector3(-1f, go.transform.localScale.y, go.transform.localScale.z);
            }
        }

    }
}