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
        private PlayerRunnerManager playerRunnerManager;

        private int[] positions = { -1, 0, 1 };
        private int currentPosition = 0;

        private float currentX = 0f;
        private float deltaX = 0f;

        private bool sliding = false;
        private Coroutine slideCoroutine;

        private bool runGame = true;

        [Header("Delta")]
        [SerializeField] private float deltaRange;

        // During development
        private float initialZ;

        void Start() { 
            rb = GetComponent<Rigidbody>(); 
            capsuleCollider = GetComponent<CapsuleCollider>();  
        }

        public void StartController(PlayerRunnerManager manager)
        {
            currentX = transform.position.x;
            initialZ = transform.position.z;
            playerRunnerManager = manager;
        }

        void Update()
        {
            if (!runGame)
            {
                Inputs();
                CalculateDeltaX();
                Positions();
                RemoveSlide();
            } else
            {
                rb.velocity = Vector3.zero;
            }
        }

        public void ObsticleHit()
        {
            runGame = true;
            playerRunnerManager.OnGameEnd();
        }

        public void Restart()
        {
            runGame = false;
        }

        void FixedUpdate()
        {
            BaseMovement();
        }

        void Inputs()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                SlideLeft();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SlideRight();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Slide();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
            }
        }

        void Jump()
        {
            if (IsGrounded())
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                StopSlideCoroutine();
            }
        }

        void SlideLeft()
        {
            if (currentPosition > positions[0])
            {
                rb.AddForce(-transform.right * sideForce, ForceMode.Impulse);
                currentPosition--;
            }
        }

        void SlideRight()
        {
            if (currentPosition < positions[positions.Length - 1])
            {
                rb.AddForce(transform.right * sideForce, ForceMode.Impulse);
                currentPosition++;
            }
        }

        void Slide()
        {
            if (!sliding)
            {
                capsuleCollider.height = 1f;
                sliding = true;
                slideCoroutine = StartCoroutine(ResetSlide());
                if (!IsGrounded())
                {
                    rb.AddForce(-transform.up * jumpForce, ForceMode.Impulse);
                }
            } else
            {
                if (slideCoroutine != null)
                {
                    StopCoroutine(slideCoroutine);
                    slideCoroutine = StartCoroutine(ResetSlide());
                }
            }
        }

        void StopSlideCoroutine()
        {
            if (slideCoroutine != null)
            {
                StopCoroutine(slideCoroutine);
                sliding = false;
            }
        }

        void BaseMovement()
        {
            if (!runGame)
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
            if (!sliding)
            {
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, 2f, Time.deltaTime * 15f);
            }
        }

        bool IsGrounded ()
        {
            return Physics.Raycast(transform.position, -transform.up, capsuleCollider.height / 2f + 0.0001f, groundMask);
        }

        IEnumerator ResetSlide()
        {
            yield return new WaitForSeconds(1.2f);
            sliding = false;
        }

        public (Vector3, Vector3) positionAndRotation()
        {
            Vector3 pos = cameraPosition.position;
            Vector3 rot = cameraPosition.rotation.eulerAngles;
            return (pos, rot);
        }

    }
}

