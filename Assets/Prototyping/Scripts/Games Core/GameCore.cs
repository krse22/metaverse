using Prototyping.Games;
using UnityEngine;

namespace Prototyping {

    public class GameCore : MonoBehaviour
    {
        public static GameCore Instance { get; private set; }

        [Header("UI Prefabs")]
        public GameObject GamePortalSingle;

        [SerializeField] private PlayerCoreController playerCoreController;

        private void Awake()
        {
            Instance = this;    
        }

        public void ExitPortal()
        {
            PlayerCoreCamera.SetCameraOwner(playerCoreController);
        }

    }
}

