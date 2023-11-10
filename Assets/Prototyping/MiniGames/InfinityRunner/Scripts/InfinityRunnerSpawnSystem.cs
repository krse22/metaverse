using UnityEngine;

namespace Prototyping.Games {
    public class InfinityRunnerSpawnSystem : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnableObjects;
        [SerializeField] private int spawnCount;
        [SerializeField] private float gap;
        [SerializeField] private bool invert = false;

        [Header("Duplication")]
        [SerializeField] private bool allowDuplicates = true;
        [SerializeField] private int duplicationCount = 0;

        [SerializeField] private bool testSingleTrap;
        [SerializeField] private int singleTrapId;

        private RunnerManagerBase manager;
        private InfinityRunnerObject lastSpawned;

        private float initialZ;
        private bool zSet = false;

        private RandomManager randomManager;
        private void Update()
        {
            Spawn();
        }

        public void Initialize(RunnerManagerBase playerRunnerManager)
        {
            if (!zSet)
            {
                initialZ = transform.position.z;
                zSet = true;
            }
            manager = playerRunnerManager;
            randomManager = new RandomManager();
            randomManager.Init(duplicationCount, spawnableObjects.Length);
            transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
            InitialSpawn();
        }

        void InitialSpawn()
        {
            float spawnGap = transform.position.z;
            for (int i = 0; i < spawnCount; i++)
            {
                int picker = GetRandom(); 
                GameObject go = Instantiate(spawnableObjects[picker]);
                InfinityRunnerObject spawnable = go.GetComponent<InfinityRunnerObject>();
                spawnable.SetManager(manager);
                float x = transform.position.x;
                float currentLength = spawnable.Length / 2f;
                float lastLength = lastSpawned == null ? 0 : lastSpawned.Length / 2f;
                float offset = gap + currentLength + lastLength;
                if (i == 0)
                {
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
            if (manager != null && manager.IsPlaying)
            {
                if (transform.position.z - lastSpawned.transform.position.z > gap + lastSpawned.Length / 2f)
                {
                    int picker = GetRandom();
                    GameObject go = Instantiate(spawnableObjects[picker]);
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

        int GetRandom()
        {
            if (testSingleTrap)
            {
                return singleTrapId;
            }
            if (allowDuplicates)
            {
                return Random.Range(0, spawnableObjects.Length);
            }
            return randomManager.GetUniqueRandom();
        }

    }
}