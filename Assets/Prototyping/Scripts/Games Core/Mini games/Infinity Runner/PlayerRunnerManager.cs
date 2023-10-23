using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerManager : MonoBehaviour, IGamePortal
    {

        [SerializeField] private Transform player;
        [SerializeField] private Transform startPosition;

        [SerializeField] private GameObject endgameUI;
        [SerializeField] private int laneCount;

        PlayerRunnerController controller;

        [SerializeField] private bool prototype;

        [SerializeField] private GameObject[] threeLaneBaseBlocks;
        [SerializeField] private float sideDashDistance;
        [SerializeField] private float movementSpeed;

        private bool gameStarted = false;

        [SerializeField] private InfinityRunnerSpawnSystem[] spawnSystems;

        public float MovementSpeed { get { return movementSpeed; } }
        public bool IsPlaying { get { return gameStarted; } }
        public float PlayerZ { get { return player.position.z; } }

        [SerializeField] private Transform objectsParent;

        void Start()
        {
            if (prototype)
            {
                Play();
                spawnSystems.ToList().ForEach((s) => s.Initialize(this));
                spawnSystems.ToList().ForEach((s) => s.Restart());
            }
        }

        public void Play()
        {
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            controller = player.GetComponent<PlayerRunnerController>();
            PlayerCoreCamera.SetCameraOwner(controller);
            endgameUI.SetActive(true);
        }

        private int[] GenerateLanes()
        {
            if (laneCount % 2 == 0 || laneCount < 3)
            {
                Debug.LogError("Lane Count must be an odd number higher or equal to 3");
                return new int[0];
            }

            int middle = Mathf.CeilToInt(laneCount / 2f);
            int offset = laneCount - middle;
            int starter = 0 - offset;

            List<int> laneList = new List<int>();
            for (int i = 0; i < laneCount; i++)
            {
                laneList.Add(starter);
                starter++;
            }
            return laneList.ToArray();
        }

        public void OnGameEnd()
        {
            gameStarted = false;
            endgameUI.SetActive(true);
            controller.Stop();
        }

        public void StartGame() {
            ObjectCleanup();
            spawnSystems.ToList().ForEach((s) => s.Restart());
            endgameUI.SetActive(false);
            int[] lanes = GenerateLanes();
            controller.Play(lanes, sideDashDistance, this);
            gameStarted = true;
            controller.Restart();
        }

        public void ExitGame()
        {
            GameCore.Instance.ExitPortal();
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
        }

        public void RegisterObject(GameObject obj)
        {
            obj.transform.SetParent(objectsParent);
        }

        public void ObjectCleanup()
        {
            foreach (Transform child in objectsParent)
            {
                Destroy(child.gameObject);
            }
        }

    }
}

