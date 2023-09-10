using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerManager : MonoBehaviour, IGamePortal
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform positions;
        
        PlayerRunnerController controller;

        public void Play()
        {
            controller = player.GetComponent<PlayerRunnerController>();
            PlayerCoreCamera.SetCameraOwner(controller);
            controller.StartController();
        }

    }
}

