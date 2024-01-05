using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Prototyping.Games
{
    public class FlowFieldMember
    {
        public int value = 0;
        public bool visited = false;
        public bool emptyEdge = false;
        public bool obsticle = false;
        public List<FlowFieldMember> edges;

        public FlowFieldMember(bool isObsticle, bool isEdgeEmpty = false) { 
            value = 0;
            visited = false;
            obsticle = isObsticle;
            emptyEdge = isEdgeEmpty;
            edges = new List<FlowFieldMember>();
        }
    }

    [ExecuteInEditMode]
    public class FlowFieldCube : MonoBehaviour
    {

        public Vector3 Direction { get; private set; }
        public bool IsTarget { get; private set; }
        public bool isObsticle;

        public Vector3 directionVector;

        [Button("Make Target")]
        public void MakeTarget()
        {
            IsTarget = true;
            gameObject.GetComponentInParent<FlowFieldManager>().SetTarget(this);
        }

        [Button("Log Directions")]
        public void LogDirections()
        {
            int forward = member.edges[0].value;
            int backwards = member.edges[1].value;
            int left = member.edges[2].value;
            int right = member.edges[3].value;

            float x = left - right;
            float z = backwards - forward;


            Debug.Log("up: " + forward + " down: " + backwards);
            Debug.Log("left: " + left + " right: " + right);
            Debug.Log("X: " + x + " Z:" + z);
            Debug.Log(new Vector3(x, 0f, z).normalized);
        }

        public void RemoveIsTarget()
        {
            IsTarget = false;
        }


        public bool visualizeValue = false;
        public bool visualizeVector = false;

        public FlowFieldMember member;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (IsTarget)
            {
                GUIStyle targetStyle = new GUIStyle();
                targetStyle.normal.textColor = Color.red;
                Handles.Label(transform.position + Vector3.forward * 0.5f, "TARGET", targetStyle);
            }

            if (member == null) return;

            if (visualizeValue)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.blue;
                Handles.Label(transform.position, member.value.ToString(), style);
            }

            if (directionVector != Vector3.zero && visualizeVector)
            {
                Gizmos.color = Color.red;
                float yPosition = transform.position.y + transform.localScale.y / 2f;
                Vector3 startPosition = new Vector3(transform.position.x, yPosition, transform.position.z);
                Vector3 direction = new Vector3(directionVector.x, 0, directionVector.z);
                Gizmos.DrawSphere(startPosition, 0.05f);
                Gizmos.DrawRay(startPosition, direction / 2f);
            }
        }
#endif
        public void Initialize()
        {
            member = new FlowFieldMember(isObsticle);
        }

        public void SetAdjacent(LayerMask mask) {
            RaycastFindAdject(transform.forward, mask);
            RaycastFindAdject(-transform.forward, mask);
            RaycastFindAdject(-transform.right, mask);
            RaycastFindAdject(transform.right, mask);
        }

        public void RaycastFindAdject( Vector3 direction, LayerMask mask)
        {
            RaycastHit hit;
            float raycastDistance = transform.localScale.x;
            if (Physics.Raycast(transform.position, direction, out hit, raycastDistance, mask))
            {
                member.edges.Add(hit.transform.GetComponent<FlowFieldCube>().member);
            }
            else
            {
                member.edges.Add(new FlowFieldMember(true, true));
            }
        }

        public void CalculateVector()
        {
            int forward = member.edges[0].value;
            int backwards = member.edges[1].value;
            int left = member.edges[2].value;
            int right = member.edges[3].value;

            float x = left - right;
            float z = backwards - forward;

            if (backwards == 0)
            {
                z = forward - backwards;
            }
            if (right == 0)
            {
                x = right - left;
            }
            if (forward == 0)
            {
                z = forward - backwards;
            }
            if (left == 0)
            {
                x = right - left;
            }

            directionVector = new Vector3(x, 0f, z).normalized;
        
        }
    }
}

