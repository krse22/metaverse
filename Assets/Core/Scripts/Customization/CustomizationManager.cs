using UnityEngine;

public class CustomizationManager : MonoBehaviour
{

    [SerializeField] private Transform cam;
    [SerializeField] private Transform currentTarget;

    [SerializeField] private CharacterCustomization maleModel;

    public void SetCameraTarget(Transform target)
    {
        currentTarget = target;
    }

    private void Update()
    {
        SetCameraPositionAndRotation();
    }

    void SetCameraPositionAndRotation()
    {
        if (currentTarget != null) 
        {
            cam.transform.position = currentTarget.position;
            cam.transform.eulerAngles = currentTarget.eulerAngles;
        }
    }

}
