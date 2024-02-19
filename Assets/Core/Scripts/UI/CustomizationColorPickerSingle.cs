using UnityEngine;
using UnityEngine.UI;

public class CustomizationColorPickerSingle : MonoBehaviour
{
    public Image colorImage;
    private CustomizationColor customizationColor;
    private CustomizationColorPickerUI colorPickerUI;

    public void SetCustomizationColor(CustomizationColor colorToSet, CustomizationColorPickerUI pickerUI)
    {
        customizationColor = colorToSet;
        colorPickerUI = pickerUI;
        colorImage.color = colorToSet.color;
    }

    public void SetColor()
    {
        colorPickerUI.SetColor(customizationColor);
    }

}
