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

        [SerializeField] private InfinityRunnerSpawnSystem[] spawnSystems;

        [SerializeField] private bool prototype;
        [SerializeField] private float sideDashDistance;
        [SerializeField] private float movementSpeed;

        private bool gameStarted = false;
        private PlayerRunnerController controller;

        public float MovementSpeed { get { return movementSpeed; } }
        public bool IsPlaying { get { return gameStarted; } }
        public float PlayerZ { get { return player.position.z; } }

        [SerializeField] private Transform objectsParent;

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
        }

        // Called directly from Buttons in UI
        public void Play() {
            ObjectCleanup();
            InitSystems();
            InitController();

            endgameUI.SetActive(false);
            gameStarted = true;
            controller.Restart();
        }

        void InitController()
        {
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            controller = player.GetComponent<PlayerRunnerController>();
            PlayerCoreCamera.SetCameraOwner(controller);
            int[] lanes = GenerateLanes();
            controller.Play(lanes, sideDashDistance, this);
        }

        void InitSystems()
        {
            spawnSystems.ToList().ForEach((s) => s.Initialize(this));
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

        // Called on Play
        // If this is a 5 lane manager, 3 lane manager UI button will call this
        // if this is a 3 lane manager, 5 lane manager UI button will call this
        public void ObjectCleanup()
        {
            foreach (Transform child in objectsParent)
            {
                Destroy(child.gameObject);
            }
        }

    }
}

