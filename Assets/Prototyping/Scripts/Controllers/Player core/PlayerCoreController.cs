using UnityEngine;

namespace Prototyping
{
    public class PlayerCoreController : MonoBehaviour, ICameraHolder
    {
        [Header("Camera")]
        [SerializeField] private float zPosition;
        [SerializeField] private float yPosition;
        [SerializeField] private float xRotation;

        [Header("Movement")]
        [SerializeField] private float movementSpeed;
        private float horizontal;
        private float vertical;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            PlayerCoreCamera.SetCameraOwner(this);
        }

        private void FixedUpdate()
        {
            Vector3 direction = (CoreJoystick.Horizontal * transform.right + CoreJoystick.Vertical * transform.forward).normalized;
            horizontal = direction.x * movementSpeed * Time.fixedDeltaTime;
            vertical = direction.z * movementSpeed * Time.fixedDeltaTime;
            Vector3 velocity = new Vector3(horizontal, rb.velocity.y, vertical);
            rb.velocity = velocity;
        }

        public (Vector3, Vector3) PositionAndRotation()
        {
            Vector3 camTargetPos = new Vector3(transform.position.x, transform.position.y + yPosition, transform.position.z + zPosition);
            Vector3 camTargetRot = new Vector3(xRotation, 0f, 0f);
            return (camTargetPos, camTargetRot);
        }
    }
}
