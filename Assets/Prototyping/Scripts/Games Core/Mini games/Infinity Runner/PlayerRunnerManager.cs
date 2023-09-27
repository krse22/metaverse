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

        [SerializeField] private float yPosition;
        [SerializeField] private float movementSpeed;
        [SerializeField] private int totalBlockCount;
        [SerializeField] private Transform initialObject;

        private bool gameStarted = false;
        private List<InfinityRunnerBlock> currentBlocks;

        void Start()
        {
            if (prototype)
            {
                Play();
            }
        }

        void Update()
        {
            MoveBlocks();
            SpawnBlocks();
            DeleteBlocks();
        }

        void MoveBlocks()
        {
            if (gameStarted)
            {
                foreach (InfinityRunnerBlock t in currentBlocks)
                {
                    Vector3 vec = t.transform.position;
                    t.transform.position = new Vector3(vec.x, vec.y, vec.z - movementSpeed * Time.deltaTime);
                }
            }
        }

        void SpawnBlocks() { 
            if (gameStarted && currentBlocks.Count > 0)
            {
                if (currentBlocks.Count < totalBlockCount)
                {
                    InfinityRunnerBlock lastGameObject = currentBlocks.Last();
                    Vector3 lastGoEndPos = lastGameObject.End.position;
                    GameObject gameObject = Instantiate(threeLaneBaseBlocks[Random.Range(0, threeLaneBaseBlocks.Length)], null, true);
                    InfinityRunnerBlock infinityRunnerBlock = gameObject.GetComponent<InfinityRunnerBlock>();
                    float offset = infinityRunnerBlock.End.position.z - infinityRunnerBlock.transform.position.z;
                    gameObject.transform.position = new Vector3(0f, yPosition, lastGoEndPos.z + offset);

                    // infinityRunnerBlock.SetSidePosition(sideDashDistance);

                    currentBlocks.Add(infinityRunnerBlock);
                }
            }
        }

        void DeleteBlocks()
        {
            if (gameStarted)
            {
                InfinityRunnerBlock firstBlock = currentBlocks.First();
                if (firstBlock != null)
                {
                    InfinityRunnerBlock infinityRunnerBlock = firstBlock.GetComponent<InfinityRunnerBlock>();
                    if (infinityRunnerBlock.End.position.z < player.position.z)
                    {
                        currentBlocks.Remove(firstBlock);
                        Destroy(firstBlock.gameObject);
                    }
                }
            }
        }

        public void Play()
        {
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            currentBlocks = new List<InfinityRunnerBlock>();
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
            initialObject.gameObject.SetActive(true);
        }

        public void StartGame() {
            GameObject gameObject = Instantiate(threeLaneBaseBlocks[0], null, true);
            gameObject.transform.position = new Vector3(0f, yPosition, initialObject.position.z);
            currentBlocks.Add(gameObject.GetComponent<InfinityRunnerBlock>());
            endgameUI.SetActive(false);
            int[] lanes = GenerateLanes();
            controller.Play(lanes, sideDashDistance);
            initialObject.gameObject.SetActive(false);
            gameStarted = true;
        }

        public void ExitGame()
        {
            GameCore.Instance.ExitPortal();
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
        }

    }
}

