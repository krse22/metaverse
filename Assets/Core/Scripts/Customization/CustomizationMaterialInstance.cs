using System;
using UnityEngine;

public enum CharacterType
{
    Male = 1,
    Female = 2,
}

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

