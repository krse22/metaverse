using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private LayerMask mask;

    public bool GetRaycastHit(float distance, out RaycastHit hit)
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            return true;
        }
        return false;
    }

}
