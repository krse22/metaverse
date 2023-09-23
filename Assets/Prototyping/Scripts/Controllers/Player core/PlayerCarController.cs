using UnityEngine;
using ArcadeVP;
using UnityEngine.UIElements;

namespace Prototyping {
    public class PlayerCarController : MonoBehaviour, ICameraHolder
    {
        [SerializeField] private LayerMask carMask;
        [SerializeField] private float carSearchRadius;

        private PlayerReference reference;
        private GameObject closestCar;
        private ArcadeVehicleController currentCar;

        private void Start()
        {
            reference = GetComponent<PlayerReference>();
        }

        void Update()
        {
            SearchForCars();
            Inputs();
            Driving();
        }

        void Driving()
        {
            if (currentCar != null)
            {
                transform.position = currentCar.transform.position;
            }
        }

        void SearchForCars()
        {
            Collider car = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position, carSearchRadius, carMask);
            closestCar = null;

            float distance = -1f;
            foreach (Collider collider in colliders)
            {
                float currentDistance = Vector3.Distance(collider.gameObject.transform.position, transform.position);
                if (distance < 0)
                {
                    distance = currentDistance;
                    car = collider;
                }

                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    car = collider;
                }
            }

            if (car != null)
            {
                closestCar = car.gameObject;
            }
        }

        void Inputs()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                bool previouslyInCar = currentCar != null;
                if (closestCar != null && !previouslyInCar)
                {
                    EnterCar();
                }
                if (currentCar != null && previouslyInCar)
                {
                    ExitCar();
                }
            }
 
        }

        void EnterCar()
        {
            currentCar = closestCar.GetComponent<ArcadeVehicleController>();
            currentCar.EnterCar(this);
            reference.rb.isKinematic = true;
            PlayerCoreCamera.SetCameraOwner(this);
        }

        void ExitCar()
        {
            currentCar.LeaveCar(this);
            currentCar = null;
            reference.rb.isKinematic = false;
            PlayerCoreCamera.SetCameraOwner(reference.coreController);
        }

        public (Vector3, Vector3) PositionAndRotation()
        {
            Vector3 camTargetPos = new Vector3(currentCar.transform.position.x, currentCar.transform.position.y + 35f, currentCar.transform.position.z - 12.5f);
            Vector3 camTargetRot = new Vector3(70f, 0f, 0f);
            return (camTargetPos, camTargetRot);
        }
    }
}