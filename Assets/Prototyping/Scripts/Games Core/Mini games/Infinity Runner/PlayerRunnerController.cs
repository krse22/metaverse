using System.Collections;
using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerController : MonoBehaviour, ICameraHolder
    {

        [Header("Main")]
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float sideForce;
        [SerializeField] private LayerMask groundMask;

        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;
        private bool startController = false;

        private int[] positions = { -1, 0, 1 };
        private int currentPosition = 0;

        private float currentX = 0f;
        private float deltaX = 0f;

        private bool hasJumped = false;

        [Header("Delta")]
        [SerializeField] private float deltaRange;

        // During development
        private float initialZ;

        void Start() { 
            rb = GetComponent<Rigidbody>(); 
            capsuleCollider = GetComponent<CapsuleCollider>();  
        }

        public void StartController()
        {
            startController = true;
            currentX = transform.position.x;
            initialZ = transform.position.z;
        }

        void Update()
        {
            Inputs();
            CalculateDeltaX();
            Positions();
            RemoveSlide();
        }

        void FixedUpdate()
        {
            BaseMovement();
        }

        void Inputs()
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !hasJumped)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                hasJumped = true;
                StartCoroutine(ResetJump());
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (currentPosition > positions[0])
                {
                    rb.AddForce(-transform.right * sideForce, ForceMode.Impulse);
                    currentPosition--;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
           
                if (currentPosition < positions[positions.Length - 1])
                {
                    rb.AddForce(transform.right * sideForce, ForceMode.Impulse);
                    currentPosition++;
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                capsuleCollider.height = 0.25f;
            }
        }

        void BaseMovement()
        {
            if (startController)
            {
                Vector3 velocityVector = new Vector3(rb.velocity.x, rb.velocity.y, movementSpeed);
                rb.velocity = velocityVector;
            }
        }
        
        void CalculateDeltaX()
        {
            deltaX = Mathf.Abs(currentX - transform.position.x);
        }

        void Positions()
        {
            if (deltaX >= deltaRange)
            {
                float setX = Mathf.Round(transform.position.x);
                transform.position = new Vector3(setX, transform.position.y, transform.position.z);
                currentX = transform.position.x;
                rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
            }
        }

        void RemoveSlide()
        {

        }

        bool IsGrounded ()
        {
            return Physics.Raycast(transform.position, -transform.up, capsuleCollider.height + 0.0001f, groundMask);
        }

        IEnumerator ResetJump()
        {
            yield return new WaitForSeconds(0.5f);
            hasJumped = false;
        }

        public (Vector3, Vector3) positionAndRotation()
        {
            Vector3 pos = cameraPosition.position;
            Vector3 rot = cameraPosition.rotation.eulerAngles;
            return (pos, rot);
        }

    }
}

