using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class KeypressClickEvent : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private UnityEvent onPointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }

    public UnityEvent GetEvent() { return onPointerDown; }
  
}
