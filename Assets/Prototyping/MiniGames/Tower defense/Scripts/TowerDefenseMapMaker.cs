using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


namespace Prototyping.Games {

    [ExecuteInEditMode]
    public class TowerDefenseMapMaker : MonoBehaviour
    {
        [SerializeField] private GameObject mapEnvironmentCube;

        [SerializeField] private Transform rightDimension;
        [SerializeField] private Transform topDimension;

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Event e = Event.current;
            //Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(e.mousePosition.x, Screen.height - e.mousePosition.y - 36, 0)); //Upside-down and offset a little because of menus
            //RaycastHit hit;

            //  Ray ray = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(e.mousePosition);
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            // Debug.Log("Mouse pos: " + e.mousePosition + " world point: " + mousePosition);
            // Debug.Log(mousePosition);
            // Ray ray = new Ray(mousePosition, SceneView.currentDrawingSceneView.camera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.gameObject.name);
            }

            // Ensure continuous Update calls.
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }

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