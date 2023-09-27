using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerController : MonoBehaviour, ICameraHolder
    {
        private Rigidbody rigidBody;
        private CapsuleCollider colliderReference;

        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float sideDashPower;
        private float sideDashDistance;

        private float currentX;
        private float initialX;
        private float currentY;
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

        private bool isPlaying = false;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            colliderReference = GetComponent<CapsuleCollider>();
            currentX = transform.position.x;
            initialX = transform.position.x;
            currentY = transform.position.y;
            initialColliderheight = colliderReference.height;
            initalCamLocalY = cameraTarget.localPosition.y;
        }

        public void Play(int[] lanesFromManager, float dashDistance)
        {
            isPlaying = true;
            lanes = lanesFromManager;
            sideDashDistance = dashDistance;
        }

        public void Update()
        {
            if (isPlaying)
            {
                Inputs();
                CalculateDeltaX();
                CalculateDeltaY();
                SlideCanceling();
                CameraEffects();
            }
        }

        void CameraEffects()
        {
            if (IsGrounded() && !isSliding)
            {
                camSinValue += camSinChangeForce;
                Vector3 localPos = cameraTarget.localPosition;
                cameraTarget.localPosition = new Vector3(localPos.x, initalCamLocalY + Mathf.Sin(camSinValue) * camSinYChange, localPos.z);
            }
        }

        public void ObsticleHit()
        {
            Debug.Log("Game end");
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

        void SlideLeft()
        {
            if (lanePosition > lanes[0] && !dashing)
            {
                rigidBody.AddForce(-Vector3.right * sideDashPower, ForceMode.Impulse);
                lanePosition--;
                dashing = true;
            }
        }

        void SlideRight()
        {
            if (lanePosition < lanes[lanes.Length - 1] && !dashing)
            {
                rigidBody.AddForce(Vector3.right * sideDashPower, ForceMode.Impulse);
                lanePosition++;
                dashing = true;
            }
        }

        void Jump()
        {
            if (IsGrounded())
            {
                float slideOffset = (initialColliderheight - colliderReference.height);
                rigidBody.AddForce(Vector3.up * (jumpForce + slideOffset), ForceMode.Impulse);
                slideCancel = true;
            }
        }

        void Slide()
        {
            colliderReference.height = initialColliderheight / 2f;
            if (!IsGrounded())
            {
                rigidBody.AddForce(-Vector3.up * sideDashPower, ForceMode.Impulse);
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
                float delta = Mathf.Abs(currentX - transform.position.x);
                if (delta > sideDashDistance)
                {
                    rigidBody.velocity = new Vector3(0f, rigidBody.velocity.y, rigidBody.velocity.z);

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
            if (delta > sideDashDistance)
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

