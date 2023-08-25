using UnityEngine;

namespace Prototyping
{
    public class PlayerReference : MonoBehaviour
    {
        public PlayerCoreController coreController;
        public PlayerCarController carController;
        public CapsuleCollider capsuleCollider;

        private void Awake()
        {
            coreController = GetComponent<PlayerCoreController>();
            carController = GetComponent<PlayerCarController>();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

    }

}
