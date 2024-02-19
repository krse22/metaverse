using System.Linq;
using UnityEngine;

public class CustomizationColorPickerUI : MonoBehaviour
{

    [SerializeField] private CustomizationManager manager;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private CustomizationScriptableObject valuesObject;

    private string currentBodyPart;

    public void Initialize(string bodyPart)
    {
        DeleteAllChildren(contentParent);
        currentBodyPart = bodyPart;
        var colors = valuesObject.colorGroups.FirstOrDefault((bp) => bp.customizationName == currentBodyPart).colors;
        foreach(var color in colors) {
            GameObject go = Instantiate(prefab, contentParent);
            go.GetComponent<CustomizationColorPickerSingle>().SetCustomizationColor(color, this);
        }
    }

    public void SetColor(CustomizationColor col)
    {
        manager.SetColor(currentBodyPart, col);
    }

    void DeleteAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            Destroy(child.gameObject);
        }
    }

}
