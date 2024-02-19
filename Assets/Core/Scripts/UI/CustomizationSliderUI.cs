using UnityEngine;
using UnityEngine.UI;

public class CustomizationSliderUI : MonoBehaviour
{

    [SerializeField] private CustomizationManager customizationManager;
    [SerializeField] private Slider slider;

    public void OnChange()
    {
        customizationManager.UpdateSlider((int)slider.value);
    }
    
}
