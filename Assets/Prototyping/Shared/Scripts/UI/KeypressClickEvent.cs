using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class KeypressClickEvent : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] protected UnityEvent onPointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }
  
}
