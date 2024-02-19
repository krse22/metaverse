using System;
using UnityEngine;

[Serializable]
public class CustomizationColor
{
    public Color color;
    public int networkId;
}


[Serializable]
public class CustomizationColorParent
{
    public string customizationName;
    public CustomizationColor[] colors;
}

[Serializable]
public class CustomizationTexture
{
    public Texture mainColor;
    public int id;
}

[CreateAssetMenu(fileName = "CustomizationScriptableObject", menuName = "ScriptableObjects/CustomizationScriptableObject", order = 1)]
public class CustomizationScriptableObject : ScriptableObject
{
    [Header("Body Colors Parent")]
    public CustomizationColorParent[] colorGroups;

    [Header("Body Colors")]
    public CustomizationColor[] lipColor;
    public CustomizationColor[] eyebrowColor;
    public CustomizationColor[] eyelashColor;
    public CustomizationColor[] skinColor;
    public CustomizationColor[] nailsColor;
    public CustomizationColor[] eyesColor;

    [Header("Base Textures")]
    public CustomizationTexture[] baseAtlas;


}
