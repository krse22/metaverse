using UnityEngine;

public class CustomizationManager : MonoBehaviour
{

    [SerializeField] private Transform cam;
    [SerializeField] private Transform currentTarget;

    [SerializeField] private CharacterCustomization maleModel;
    [SerializeField] private CharacterCustomization femaleModel;

    [SerializeField] private CustomizationScriptableObject bodyColors;

    private CharacterCustomization currentModel;

    private void Start()
    {
        SetMale();
    }

    public void SetCameraTarget(Transform target)
    {
        currentTarget = target;
        currentModel = maleModel;
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

    public void SetMale()
    {
        currentModel = maleModel;
    }

    public void SetFemale()
    {
        currentModel = femaleModel;
    }

    public void SetColor(string bodyGroup, CustomizationColor colorToSet)
    {
        currentModel.UpdateSkinMaterial(bodyGroup, colorToSet);
    }


}
