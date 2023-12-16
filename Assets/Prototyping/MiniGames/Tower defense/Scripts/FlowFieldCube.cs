using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Prototyping.Games
{
    [ExecuteInEditMode]
    public class FlowFieldCube : MonoBehaviour
    {

        public Vector3 Direction { get; private set; }
        public bool IsTarget { get; private set; }

        [Button("Make Target")]
        public void MakeTarget()
        {
            IsTarget = true;
            gameObject.GetComponentInParent<FlowFieldManager>().SetTarget(this);
        }

        public List<FlowFieldCube> edges = new List<FlowFieldCube>();
        public bool visited = false;
        public int value;

        private void OnDrawGizmos()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 guiPosition = new Vector2(screenPos.x, Screen.height - screenPos.y);
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.blue;
            UnityEditor.Handles.Label(transform.position, value.ToString(), style);
        }

        public void SetAdjacent(LayerMask mask) {
            edges.Clear();
            visited = false;

            float raycastDistance = transform.localScale.x;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, mask))
            {
                edges.Add(hit.transform.GetComponent<FlowFieldCube>());
            }
            if (Physics.Raycast(transform.position, -transform.forward, out hit, raycastDistance, mask))
            {
                edges.Add(hit.transform.GetComponent<FlowFieldCube>());
            }
            if (Physics.Raycast(transform.position, transform.right, out hit, raycastDistance, mask))
            {
                edges.Add(hit.transform.GetComponent<FlowFieldCube>());
            }
            if (Physics.Raycast(transform.position, -transform.right, out hit, raycastDistance, mask))
            {
                edges.Add(hit.transform.GetComponent<FlowFieldCube>());

            }
        }

    }
}

