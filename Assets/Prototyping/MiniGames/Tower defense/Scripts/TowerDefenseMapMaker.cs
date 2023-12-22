using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


namespace Prototyping.Games {

    public class TowerDefenseMapMaker : MonoBehaviour
    {
        [SerializeField] private GameObject mapEnvironmentCube;
        [SerializeField] private GameObject positionCube;

        [SerializeField] private Transform rightDimension;
        [SerializeField] private Transform topDimension;

        [SerializeField] private bool deletionMode;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
    
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.transform.CompareTag("TowerDefenseField"))
                {
                    if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                    {
                        Debug.Log("Instantiate");
                    }
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.localScale);
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

        [Button("Make field")]
        [BoxGroup("MakingField")]
        public void MakeField()
        {
            ClearField();

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
                    gameObject.transform.SetParent(transform);
                }
            }

            DestroyImmediate(go);
        }

        [Button("Remove field")]
        [BoxGroup("MakingField")]
        public void ClearField()
        {
            var tempArray = new GameObject[transform.childCount];

            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = transform.GetChild(i).gameObject;
            }

            foreach (var child in tempArray)
            {
                DestroyImmediate(child);
            }
        }

    }
}