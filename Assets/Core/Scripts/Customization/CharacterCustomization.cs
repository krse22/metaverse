using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class CustomizationMaterialInstance
{
    public string materialName;
    public int index;
    public Material material;
    public CustomizationColor customizationColor;

    public void SetColor(CustomizationColor color)
    {
        material.SetColor("_MainColor", color.color);
        customizationColor = color;
    }
}

public enum CharacterType
{
    Male = 1,
    Female = 2,
}

public class CharacterCustomization : MonoBehaviour
{

    [SerializeField] private CharacterType characterType;

    [SerializeField] private ShapeKey[] mainBodyKeys;
    [SerializeField] private CustomizationMaterialInstance[] customizationMaterials;
    [SerializeField] private SkinnedMeshRenderer mainBodyRenderer;

    [SerializeField] private Transform cameraTarget;

    private void Start()
    {
        InitializeShapeKeys();
        InitializeMaterials();
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

    void InitializeMaterials()
    {
        int index = 0;
        foreach (Material material in mainBodyRenderer.materials)
        {
            customizationMaterials[index].material = material;
            index++;
        }
    }

    public void UpdateSkinMaterial(string materialReferenceName, CustomizationColor color)
    {
        var material = customizationMaterials.FirstOrDefault((m) => m.materialName == materialReferenceName);
        material.SetColor(color);
    }

}
