using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerController : MonoBehaviour, ICameraHolder
    {
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private float movementSpeed;

        private Rigidbody rb;       

        void Start() { 
            rb = GetComponent<Rigidbody>(); 
        }

        void Update()
        {
           Vector3 velocityVector = new Vector3(rb.velocity.x + transform.forward.x, rb.velocity.y, rb.velocity.z + transform.forward.z);
           rb.velocity = velocityVector * Time.deltaTime * movementSpeed;
        }
        
        public (Vector3, Vector3) positionAndRotation()
        {
            Vector3 pos = cameraPosition.position;
            Vector3 rot = cameraPosition.rotation.eulerAngles;
            return (pos, rot);
        }
    }
}

