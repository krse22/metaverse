using UnityEngine;

namespace Prototyping {
    public class PlayerCoreCamera : MonoBehaviour
    {
        public static PlayerCoreCamera Instance;

        [SerializeField] private float zPosition;
        [SerializeField] private float yPosition;
        [SerializeField] private float xRotation;

        private Transform target;

        void Awake () {
            Instance = this;
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                transform.position = new Vector3 (target.position.x, yPosition, target.position.z + zPosition);
                transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }

        public void RegisterTransformTarget(Transform transform)
        {
            target = transform;
        }

    }
}

