using System;
using UnityEngine;

[Serializable]
public class ShapeKey
{
    private static int maxValue = 100;

    private int value;
    
    [SerializeField] private bool hasNegative;
    [SerializeField] private string shapeKeyName;

    public int shapeIndexPositive { get; set; }
    public int shapeIndexNegative { get; set; }

    public string ShapeKeyName { get { return shapeKeyName; } }
    public bool HasNegative { get {  return hasNegative; } }
    public int Value { get { return value; } }

    public void SetValue(int value)
    {
        this.value = value;
    }

    public int GetIndex()
    {
        if (this.value >= 0)
        {
            return shapeIndexPositive;
        }
        return shapeIndexNegative;
    }

}
