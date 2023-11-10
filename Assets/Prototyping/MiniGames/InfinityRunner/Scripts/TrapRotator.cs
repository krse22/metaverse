using UnityEngine;

namespace Prototyping.Games
{
    public class TrapRotator : MonoBehaviour
    {

        [SerializeField] private float rotateSpeed;

        void Update()
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotateSpeed * Time.deltaTime);
        }
    }
}