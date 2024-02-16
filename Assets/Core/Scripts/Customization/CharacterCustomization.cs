using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{

    [SerializeField] private ShapeKey[] mainBodyKeys;
    [SerializeField] private SkinnedMeshRenderer mainBodyRenderer;

    [SerializeField] private Transform cameraTarget;

    private void Start()
    {
        InitializeShapeKeys();
    }

    void InitializeShapeKeys()
    {
        foreach (ShapeKey key in mainBodyKeys)
        {
            int positive = mainBodyRenderer.sharedMesh.GetBlendShapeIndex(key.ShapeKeyName + "+");
            key.shapeIndexPositive = positive;

            if (key.HasNegative)
            {
                int negative = mainBodyRenderer.sharedMesh.GetBlendShapeIndex(key.ShapeKeyName + "-");
                key.shapeIndexNegative = negative;
            }
        }
    }

}
