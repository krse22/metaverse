using System.Collections;
using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerController : MonoBehaviour, ICameraHolder
    {
        private RunnerManagerBase manager;


        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private CapsuleCollider colliderReference;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Animation cameraAnimation;
        [SerializeField] private float sideDashPower;

        [SerializeField] private float increasedGravityForce;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float jumpForce;
        [SerializeField] private float camSinChangeForce;
        [SerializeField] private float camSinYChange;
        private float camSinValue = 0;
        private float initalCamLocalY;

        private Vector2 currentPosition;
        private bool dashing = false;
        private int lanePosition = 0;

        private float initialColliderheight;
        private bool slideCancel = false;
        private bool isSliding = false;

        private float timeToCancel = 1.2f;
        private float cancelTick = 0f;
        private enum SlideSide { Left = -1, Right = 1 };
        private SlideSide slideSide;

        void Start()
        {
            initalCamLocalY = cameraTarget.localPosition.y;
            initialColliderheight = colliderReference.height;
            PlayerCoreCamera.SetCameraOwner(this);
        }

        public void Play(RunnerManagerBase runnerManager)
        {
            transform.position = runnerManager.StartPosition.position;
            manager = runnerManager;
            currentPosition = transform.position;
            rigidBody.isKinematic = false;
            lanePosition = 0;
        }

        public void Update()
        {
            if (manager != null && manager.IsPlaying)
            {
                Inputs();
                CalculateDeltaX();
                SlideCanceling();
                CameraEffects();
                IncreaseGravity();
            }
        }

        void IncreaseGravity()
        {
            rigidBody.AddForce(-transform.up * increasedGravityForce, ForceMode.Force);
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
            dashing = false;
            colliderReference.height = initialColliderheight;
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

        public void SlideLeft()
        {
            if (lanePosition > manager.Lanes[0] && !dashing)
            {
                cameraAnimation.Play("SlideLeftCam");
                rigidBody.AddForce(transform.right * (float)SlideSide.Left * sideDashPower, ForceMode.Impulse);
                lanePosition--;
                slideSide = SlideSide.Left;
                dashing = true;
            }
        }

        public void SlideRight()
        {
            if (lanePosition < manager.Lanes[manager.Lanes.Length - 1] && !dashing)
            {
                cameraAnimation.Play("SlideRightCam");
                rigidBody.AddForce(transform.right * (float)SlideSide.Right * sideDashPower, ForceMode.Impulse);
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
            if (!dashing) return;

            float delta = Mathf.Abs(currentPosition.x - transform.position.x);
            if (delta > manager.SideDashDistance - 0.1f)
            {
                rigidBody.velocity = new Vector3(0f, rigidBody.velocity.y, rigidBody.velocity.z);
                float side = Mathf.Sign(lanePosition);
                float targetX = manager.StartPosition.position.x + (manager.SideDashDistance * Mathf.Abs(lanePosition) * side);
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
                currentPosition.x = transform.position.x;
                dashing = false;
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

