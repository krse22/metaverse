using UnityEngine;

namespace Prototyping
{
    public class CoreGameUI : MonoBehaviour
    {
        public static CoreGameUI Instance;

        [SerializeField] private GameObject protalsUI;
        public GameObject PortalUI { get {  return protalsUI; } }

        private void Awake()
        {
            Instance = this;
        }
    }
}

