using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerManager : MonoBehaviour, IGamePortal
    {
        [SerializeField] private Transform player;

        public void Play()
        {
            PlayerCoreCamera.SetCameraOwner(player.GetComponent<PlayerRunnerController>());
        }
    }
}

