using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Prototyping.Games
{
    public class FlowFieldManager : MonoBehaviour
    {

        [SerializeField] private GameObject flowFieldCube;
        [SerializeField] private LayerMask waveFrontLayer;

        public FlowFieldCube target;

        private Transform topDimension;
        private Transform rightDimension;

        private bool showMesh = true;
        private bool visualizeTarget = true;

        [HorizontalGroup("Offset")]
        [SerializeField] private float leftOffset;


        [HorizontalGroup("Offset")]
        [SerializeField] private float topOffset;

        [InfoBox("Flow field dimensions are based on Top and Right game objects ( check CONSTURCTION game object in the scene ), you can increase their size and click bake again", InfoMessageType.Info)]

        [Button("Make flowfield")]
        [BoxGroup("Baking flow field")]
        public void MakeFlowfield()
        {
            ClearFlowField();
            transform.position = Vector3.zero;

            GameObject managerGo = GameObject.Find("CONSTRUCTION");
            if (!Validate(managerGo))
            {
                return;
            }

            TowerDefenseFlowField manager = managerGo.GetComponent<TowerDefenseFlowField>();
            (topDimension, rightDimension) = manager.GetDimensions;


            float startX = rightDimension.position.x * -1f;
            float startZ = topDimension.position.z * -1f;

            float endX = rightDimension.position.x;
            float endZ = topDimension.position.z;

            GameObject go = Instantiate(flowFieldCube);
            float flowCubeDimension = go.transform.localScale.x;

            for (float i = startZ + topOffset; i < endZ; i += flowCubeDimension)
            {
                for (float j = startX + leftOffset; j < endX; j += flowCubeDimension)
                {
                    GameObject gameObject = Instantiate(flowFieldCube);
                    gameObject.transform.position = new Vector3(j, 0f, i);
                    gameObject.transform.SetParent(transform);
                }
            }

            showMesh = true;

            DestroyImmediate(go);
        }

        [Button("Bake flowfield")]
        [BoxGroup("Baking flow field")]
        public void BakeFlowField()
        {
            if (target == null)
            {
                Debug.LogError("Flow field target not set");
                return;
            }

            foreach (Transform t in transform)
            {
                t.GetComponent<FlowFieldCube>().SetAdjacent(waveFrontLayer);
            }

            Queue<FlowFieldCube> flowFieldCubes = new Queue<FlowFieldCube>();

            FlowFieldCube initial = target.GetComponent<FlowFieldCube>();
            initial.visited = true;
            initial.value = 0;
            flowFieldCubes.Enqueue(initial);
            int value = 0;

            while (flowFieldCubes.Count != 0)
            {
                FlowFieldCube v = flowFieldCubes.Dequeue();

                for (int i = 0; i < v.edges.Count; i++)
                {
                    FlowFieldCube edge = v.edges[i];
                    if (!edge.visited)
                    {
                        edge.value = v.value + 1;
                        edge.visited = true;
                        flowFieldCubes.Enqueue(edge);
                    }
                }
            }

        }

        [BoxGroup("Visualizing Field")]
        [Button("Toggle mesh renderer")]
        public void ToggleMeshRenderer()
        {
            showMesh = !showMesh;
            foreach (Transform t in transform)
            {
                t.gameObject.GetComponent<MeshRenderer>().enabled = showMesh;
            }
        }

        [BoxGroup("Visualizing Field")]
        [Button("Visualize target renderer")]
        public void VisualizeTarget()
        {
            if (target == null) return;

            visualizeTarget = !visualizeTarget;
            if (visualizeTarget)
            {
                target.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else
            {
                target.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        bool Validate(GameObject go)
        {
            if (go == null)
            {
                Debug.LogError("Manager with a name CONSTRUCTION and script TowerDefenseFlowField is not found in the scene");
                return false;
            }


            if (go.GetComponent<TowerDefenseFlowField>() == null)
            {
                Debug.LogError("TowerDefenseFlowField script not found on CONSTRUCTION game object");
                return false;
            }
            return true;
        }

        void ClearFlowField()
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

        public void SetTarget(FlowFieldCube ffc)
        {
            target = ffc;
        }

    }
}