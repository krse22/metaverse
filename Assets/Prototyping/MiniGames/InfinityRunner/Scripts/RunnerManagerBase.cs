using System.Linq;
using UnityEngine;

namespace Prototyping.Games
{
    public abstract class RunnerManagerBase : MonoBehaviour
    {
        [SerializeField] protected Transform player;
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
        public virtual void OnGameStart()
        {
            ObjectCleanup();
            InitSystems();
            InitController();
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

        protected void InitController()
        {
            player.GetComponent<PlayerRunnerController>().Play(this);
        }

        public void ObjectCleanup()
        {
            foreach (Transform child in objectsParent)
            {
                Destroy(child.gameObject);
            }
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