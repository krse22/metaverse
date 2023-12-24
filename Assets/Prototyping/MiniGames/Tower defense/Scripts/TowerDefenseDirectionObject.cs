using UnityEditor;
using UnityEngine;

public class TowerDefenseDirectionObject : MonoBehaviour
{

    public Vector3 directionFromRight { get => -transform.forward; }
    public Vector3 directionFromLeft { get => transform.forward; }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {

        Quaternion rotation = Quaternion.Euler(transform.eulerAngles);

        Handles.color = Color.blue;
        Vector3 forwardPosition = transform.position + transform.right * 2f;
        Handles.ArrowHandleCap(0, forwardPosition, rotation, 10f, EventType.Repaint);

        Quaternion backwardRotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180f, transform.eulerAngles.z));
        Handles.color = Color.red;
        Vector3 backwardPosition = transform.position + transform.right * -2f;
        Handles.ArrowHandleCap(0, backwardPosition, backwardRotation, 10f, EventType.Repaint);

    }
#endif


}
