using UnityEngine;

namespace Prototyping
{
    public class PlayerCoreController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        private float horizontal;
        private float vertical;

        private Rigidbody rb;

        private void Start()
        {
            PlayerCoreCamera.Instance.RegisterTransformTarget(transform);
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 direction = (CoreJoystick.Horizontal * transform.right + CoreJoystick.Vertical * transform.forward).normalized;
            horizontal = direction.x * movementSpeed * Time.fixedDeltaTime;
            vertical = direction.z * movementSpeed * Time.fixedDeltaTime;
            Vector3 velocity = new Vector3(horizontal, rb.velocity.y, vertical);
            rb.velocity = velocity;
        }

    }
}
