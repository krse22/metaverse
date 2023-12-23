using UnityEngine;

public class TowerDefenseFieldSingle : MonoBehaviour
{

    [SerializeField] private BoxCollider boxCollider;

    public bool IsOccupied { get; private set; }

    public void SetIntoSpawnable(GameObject graphics)
    {
        graphics.transform.parent = transform;
        graphics.transform.localPosition = Vector3.zero;
        IsOccupied = true;
    }

    public void RemoveSpawnable()
    {
        var t = transform.GetChild(0);
        if (t != null)
        {
            DestroyImmediate(t.gameObject);
        }
        IsOccupied = false;
    }

}
