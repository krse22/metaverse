using System;
using UnityEngine;

[Serializable]
public class ShapeKey
{
    private static float maxValue = 100;

    private float value;
    
    [SerializeField] private bool hasNegative;
    [SerializeField] private string shapeKeyName;

    public int shapeIndexPositive { get; set; }
    public int shapeIndexNegative { get; set; }

    public string ShapeKeyName { get { return shapeKeyName; } }
    public bool HasNegative { get {  return hasNegative; } }

}
