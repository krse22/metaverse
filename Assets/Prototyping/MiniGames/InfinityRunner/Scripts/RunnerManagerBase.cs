using System.Linq;
using UnityEngine;

namespace Prototyping.Games
{
    public abstract class RunnerManagerBase : MonoBehaviour
    {
        private GameObject currentPlayer;
        [SerializeField] protected Transform startPosition;
        [SerializeField] protected InfinityRunnerSpawnSystem[] spawnSystems;

        [SerializeField] protected int laneCount;
        [SerializeField] protected float sideDashDistance;
        [SerializeField] protected float movementSpeed;
        [SerializeField] private float offsetDeleteFix = 0;

        [SerializeField] protected InfinityRunnerManagerCurrent current;

        private int[] lanes;
        protected bool isPlaying = false;
        public float MovementSpeed => movementSpeed;
        public bool IsPlaying => isPlaying;
        public Transform StartPosition => startPosition;
        public float OffsetDeleteFix => offsetDeleteFix;
        public int LaneCount => laneCount;
        public float SideDashDistance => sideDashDistance;
        public int[] Lanes => lanes;

        [SerializeField] protected Transform objectsParent;

        public abstract void OnGameEnd();
        public virtual void OnGameStart(GameObject playerPrefab)
        { 
            InitSystems();
            InitController(playerPrefab);
            isPlaying = true;
        }

        private void Start()
        {
            lanes = InfinityRunnerUtils.GenerateLanes(laneCount);
        }

        public void RegisterObject(GameObject go)
        {
            go.transform.SetParent(objectsParent);
        }

        protected void InitSystems()
        {
            spawnSystems.ToList().ForEach((s) => s.Initialize(this));
        }

        protected void InitController(GameObject playerPrefab)
        {
            GameObject go = Instantiate(playerPrefab, startPosition.position, Quaternion.identity);
            go.GetComponent<PlayerRunnerController>().Play(this);
            InfinityRunnerInputSystem.RegisterController(go.GetComponent<PlayerRunnerController>());
            currentPlayer = go;
        }

        public void ObjectCleanup()
        {
            foreach (Transform child in objectsParent)
            {
                Destroy(child.gameObject);
            }
            Destroy(currentPlayer);
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void Unpause()
        {
            isPlaying = true;
        }

    }
}