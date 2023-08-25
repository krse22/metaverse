using UnityEngine;
using ArcadeVP;

namespace Prototyping {
    public class PlayerCarController : MonoBehaviour
    {
        [SerializeField] private LayerMask carMask;
        [SerializeField] private float carSearchRadius;

        private GameObject closestCar;
        private ArcadeVehicleController currentCar;

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
        }

        void ExitCar()
        {
            currentCar.LeaveCar(this);
            currentCar = null;
        }
       
    }
}