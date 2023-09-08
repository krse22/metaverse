using UnityEngine;

namespace Prototyping {

    public class GameCore : MonoBehaviour
    {
        public static GameCore Instance { get; private set; }

        [Header("UI Prefabs")]
        public GameObject GamePortalSingle;

        private void Awake()
        {
            Instance = this;    
        }

    }
}

