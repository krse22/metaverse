using System.Collections.Generic;
using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerManager : MonoBehaviour, IGamePortal
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform startPosition;

        [SerializeField] private GameObject endgameUI;
        [SerializeField] private int laneCount;

        [SerializeField] private bool isPrototype;

        PlayerRunnerController controller;


        void Start()
        {
            if (isPrototype)
            {
                RestartGame();
            }
        }

        public void Play()
        {

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
            endgameUI.SetActive(true);
        }

        public void RestartGame() {
            endgameUI.SetActive(false);
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
            controller = player.GetComponent<PlayerRunnerController>();
            int[] lanes = GenerateLanes();
            controller.Play(lanes);
            PlayerCoreCamera.SetCameraOwner(controller);
        }

        public void ExitGame()
        {
            GameCore.Instance.ExitPortal();
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
        }

    }
}

