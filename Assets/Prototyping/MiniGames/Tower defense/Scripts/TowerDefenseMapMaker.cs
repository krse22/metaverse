using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Prototyping.Games {

    public class TowerDefenseMapMaker : MonoBehaviour
    {
        [SerializeField] private GameObject mapEnvironmentCube;
        [SerializeField] private GameObject graphicsCube;

        [SerializeField] private Transform rightDimension;
        [SerializeField] private Transform topDimension;

        [SerializeField] private Transform fieldsParent;

        [SerializeField] private bool deletionMode;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            bool isMouseEvent = Event.current.type == EventType.MouseUp && Event.current.button == 0;

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.transform.CompareTag("TowerDefenseField"))
                {
                    TowerDefenseFieldSingle fieldSingle = hit.collider.gameObject.GetComponent<TowerDefenseFieldSingle>();
                    if (isMouseEvent && !fieldSingle.IsOccupied && !deletionMode)
                    {
                        GameObject graphics = Instantiate(graphicsCube);
                        fieldSingle.SetIntoSpawnable(graphics);
                    }

                    if (isMouseEvent && fieldSingle.IsOccupied && deletionMode)
                    {
                        fieldSingle.RemoveSpawnable();
                    }

                    if (!fieldSingle.IsOccupied && !deletionMode)
                    {
                        DrawGizmosCube(hit, Color.green, 0f);
                    }
    
                    if (fieldSingle.IsOccupied && deletionMode)
                    {
                        DrawGizmosCube(hit, Color.red, 0.1f);
                    }
                }
            }

            // Ensure continuous Update calls.
            if (!Application.isPlaying)
            {
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }

        }


#endif

        public void DrawGizmosCube(RaycastHit hit, Color color, float yOffset)
        {
            Vector3 hitObjectPosition = hit.collider.gameObject.transform.position;
            Vector3 hitPosition = new Vector3(hitObjectPosition.x, hitObjectPosition.y + yOffset, hitObjectPosition.z);
            Gizmos.color = color;
            Gizmos.DrawCube(hitPosition, hit.collider.gameObject.transform.localScale);
        }


        [Button("Make field")]
        [BoxGroup("Making Field")]
        public void MakeField()
        {
            RemoveField();

            float startX = rightDimension.position.x * -1f;
            float startZ = topDimension.position.z * -1f;

            float endX = rightDimension.position.x;
            float endZ = topDimension.position.z;

            GameObject go = Instantiate(mapEnvironmentCube);
            float flowCubeDimension = go.transform.localScale.x;
            float topOffset = flowCubeDimension / 2f;
            float leftOffset = flowCubeDimension / 2f;

            for (float i = startZ + topOffset; i < endZ; i += flowCubeDimension)
            {
                for (float j = startX + leftOffset; j < endX; j += flowCubeDimension)
                {
                    GameObject gameObject = Instantiate(mapEnvironmentCube);
                    gameObject.transform.position = new Vector3(j, 0f, i);
                    gameObject.transform.SetParent(fieldsParent);
                }
            }

            DestroyImmediate(go);
        }

        [Button("Remove field")]
        [BoxGroup("Making Field")]
        public void RemoveField()
        {
            var tempArray = new GameObject[fieldsParent.childCount];

            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = fieldsParent.GetChild(i).gameObject;
            }

            foreach (var child in tempArray)
            {
                DestroyImmediate(child);
            }
        }

        [Button("Clear field")]
        [BoxGroup("Making Field")]
        public void ClearField()
        {
            foreach(Transform child in fieldsParent)
            {
                TowerDefenseFieldSingle fieldSingle = child.gameObject.GetComponent<TowerDefenseFieldSingle>();
                if (fieldSingle.IsOccupied)
                {
                    fieldSingle.RemoveSpawnable();
                }
            }
        }

        [Button("Save Field")]
        [BoxGroup("Making Field")]
        public void SaveField()
        {
            SaveTowerDefenseMapEditor.OpenWindow(this);
        }


    }
}