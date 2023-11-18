using System.Collections;
using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerController : MonoBehaviour, ICameraHolder
    {
        private Rigidbody rigidBody;
        private CapsuleCollider colliderReference;

        private RunnerManagerBase manager;

        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float sideDashPower;
        private float sideDashDistance;

        private float currentX;
        private float currentY;

        private float initialX;
        private bool initialSet = false;

        private bool dashing = false;

        [SerializeField] private float jumpForce;
        [SerializeField] private Transform cameraTarget;
        private float camSinValue = 0;
        private float initalCamLocalY;

        [SerializeField] private float camSinChangeForce;
        [SerializeField] private float camSinYChange;

        private int[] lanes;
        private int lanePosition = 0;

        private float initialColliderheight;
        private bool slideCancel = false;
        private bool isSliding = false;

        private float timeToCancel = 1.2f;
        private float cancelTick = 0f;

        private bool canCheckForTraps = false;

        public void Play(int[] lanesFromManager, float dashDistance, RunnerManagerBase runnerManager)
        {
            if (!initialSet)
            {
                rigidBody = GetComponent<Rigidbody>();
                colliderReference = GetComponent<CapsuleCollider>();
                initialX = transform.position.x;
                initalCamLocalY = cameraTarget.localPosition.y;
                initialColliderheight = colliderReference.height;
                initialSet = true;
            }
            transform.position = new Vector3(initialX, 1f, 0f);
            currentX = transform.position.x;
            currentY = transform.position.y;
            colliderReference.height = initialColliderheight;
            rigidBody.isKinematic = false;
            lanePosition = 0;
            dashing = false;
            lanes = lanesFromManager;
            sideDashDistance = dashDistance;
            manager = runnerManager;
            StartCoroutine(CanCheckForTrapsInit());
        }

        IEnumerator CanCheckForTrapsInit()
        {
            yield return new WaitForSeconds(0.2f);
            canCheckForTraps = true;
        }

        public void Update()
        {
            if (manager != null && manager.IsPlaying)
            {
                Inputs();
                CalculateDeltaX();
                CalculateDeltaY();
                SlideCanceling();
                CameraEffects();
                CheckForTrapsFix();
            }
        }

        void CheckForTrapsFix()
        {
            // For some reason, SOMETIMES when sliding you just pass through a trap, can't find a fix so this is a fix for now
            // Yea also sometimes it calls this when you start the game so il do an easy fix of waiting for 0.2 secs before this can be played
            if (canCheckForTraps)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
                foreach (Collider col in colliders)
                {
                    if (col.transform.CompareTag("InfinityRunnerObsticle"))
                    {
                        ObsticleHit();
                    }
                }
            }
        }

        void CameraEffects()
        {
            if (IsGrounded() && !isSliding)
            {
                camSinValue += manager.MovementSpeed / 200f;
                Vector3 localPos = cameraTarget.localPosition;
                cameraTarget.localPosition = new Vector3(localPos.x, initalCamLocalY + Mathf.Sin(camSinValue) * camSinYChange, localPos.z);
            }
        }

        public void ObsticleHit()
        {
            manager.OnGameEnd();
            rigidBody.isKinematic = true;
            canCheckForTraps = false;
        }

        void Inputs()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SlideLeft();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SlideRight();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Slide();
            }
        }

        private enum SlideSide { Left = -1, Right = 1 };
        private SlideSide slideSide;

        public void SlideLeft()
        {
            if (lanePosition > lanes[0] && !dashing)
            {
                // rigidBody.AddForce(-Vector3.right * sideDashPower, ForceMode.Impulse);
                lanePosition--;
                slideSide = SlideSide.Left;
                dashing = true;
            }
        }

        public void SlideRight()
        {
            if (lanePosition < lanes[lanes.Length - 1] && !dashing)
            {
                // rigidBody.AddForce(Vector3.right * sideDashPower, ForceMode.Impulse);
                lanePosition++;
                slideSide = SlideSide.Right;
                dashing = true;
            }
        }

        public void Jump()
        {
            if (IsGrounded())
            {
                float slideOffset = (initialColliderheight - colliderReference.height);
                rigidBody.AddForce(Vector3.up * (jumpForce + slideOffset), ForceMode.Impulse);
                slideCancel = true;
            }
        }

        public void Slide()
        {
            slideCancel = false;
            colliderReference.height = initialColliderheight / 2f;
            if (!IsGrounded())
            {
                float multipliedVelocity = 0;
                if (rigidBody.velocity.y > 0)
                {
                    multipliedVelocity = rigidBody.velocity.y;
                }
                rigidBody.AddForce(-Vector3.up * (multipliedVelocity + (sideDashPower / 2f)) , ForceMode.Impulse);
            }
            isSliding = true;
            cancelTick = Time.time;
        }

        void SlideCanceling()
        {
            if (Time.time > cancelTick + timeToCancel && !slideCancel)
            {
                slideCancel = true;
            }

            if (slideCancel)
            {
                colliderReference.height = Mathf.Lerp(colliderReference.height, initialColliderheight + 0.1f, Time.deltaTime * 8);
                if (colliderReference.height > initialColliderheight)
                {
                    slideCancel = false;
                    isSliding = false;
                    colliderReference.height = initialColliderheight;
                }
            }
        }

        void CalculateDeltaX()
        {
            if (dashing)
            {
                float slidingSide = (float)slideSide;
                transform.position = new Vector3(transform.position.x + slidingSide * sideDashPower * Time.deltaTime, transform.position.y, transform.position.z);

                float delta = Mathf.Abs(currentX - transform.position.x);
                if (delta > sideDashDistance)
                {
                    // rigidBody.velocity = new Vector3(0f, rigidBody.velocity.y, rigidBody.velocity.z);

                    float side = Mathf.Sign(lanePosition);
                    float targetX = initialX + (sideDashDistance * Mathf.Abs(lanePosition) * side);
                    transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
                    currentX = transform.position.x;

                    dashing = false;
                }
            }
        }

        void CalculateDeltaY()
        {
            float delta = Mathf.Abs(currentY - transform.position.y);
            if (delta > sideDashDistance - 0.15f)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, -1.2f, rigidBody.velocity.z);
            }
        }

        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -transform.up, colliderReference.height / 2f + 0.1f, groundMask);
        }

        public (Vector3, Vector3) PositionAndRotation()
        {
            return (cameraTarget.position, cameraTarget.eulerAngles);
        }
    }
}

