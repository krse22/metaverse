using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerManager : MonoBehaviour, IGamePortal
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform startPosition;

        [SerializeField] private GameObject endgameUI;

        PlayerRunnerController controller;

        public void Play()
        {
            controller = player.GetComponent<PlayerRunnerController>();
            PlayerCoreCamera.SetCameraOwner(controller);
            controller.StartController(this);
            controller.Restart();
        }

        public void OnGameEnd()
        {
            endgameUI.SetActive(true);
        }

        // Called from button
        public void RestartGame() {
            controller.Restart();
            endgameUI.SetActive(false);
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
        }

        public void ExitGame()
        {
            GameCore.Instance.ExitPortal();
            player.transform.position = new Vector3(startPosition.position.x, player.position.y, startPosition.position.z);
        }

    }
}

