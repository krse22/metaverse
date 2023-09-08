using UnityEngine;

namespace Prototyping {
    public class PlayerCoreCamera : MonoBehaviour
    {
        private static PlayerCoreCamera Instance;

        [SerializeField] private float zPosition;
        [SerializeField] private float yPosition;
        [SerializeField] private float xRotation;

        private ICameraHolder target;

        void Awake () {
            Instance = this;
        }

        public static void SetCameraOwner(ICameraHolder holderTarget) {
            Instance.target = holderTarget;
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                (Vector3 positionTarget, Vector3 rotationTarget) = target.positionAndRotation();
                transform.position = positionTarget;
                transform.rotation = Quaternion.Euler(rotationTarget);
            }
        }


    }
}

