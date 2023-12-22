using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Prototyping.Games
{
    public class FlowFieldManager : MonoBehaviour
    {

        [SerializeField] private GameObject flowFieldCube;

        public FlowFieldCube target;

        private Transform topDimension;
        private Transform rightDimension;

        private bool showMesh = true;

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
            GameObject managerGo = GameObject.Find("CONSTRUCTION");
            LayerMask waveFrontLayer = managerGo.GetComponent<TowerDefenseFlowField>().WaveFrontLayer;

            if (target == null)
            {
                Debug.LogError("Flow field target not set");
                return;
            }

            foreach (Transform t in transform)
            {
                t.GetComponent<FlowFieldCube>().Initialize();
            }

            foreach (Transform t in transform)
            {
                t.GetComponent<FlowFieldCube>().SetAdjacent(waveFrontLayer);
            }

            Queue<FlowFieldMember> flowFieldMembers = new Queue<FlowFieldMember>();

            FlowFieldMember initial = target.GetComponent<FlowFieldCube>().member;

            initial.visited = true;
            initial.value = 1;
            flowFieldMembers.Enqueue(initial);

            while (flowFieldMembers.Count != 0)
            {
                FlowFieldMember v = flowFieldMembers.Dequeue();
                for (int i = 0; i < v.edges.Count; i++)
                {
                    FlowFieldMember edge = v.edges[i];
                    if (!edge.visited && !edge.emptyEdge && !edge.obsticle)
                    {
                        edge.value = v.value + 1;
                        edge.visited = true;
                        flowFieldMembers.Enqueue(edge);
                    }
                }
            }

            foreach (Transform t in transform)
            {
                t.GetComponent<FlowFieldCube>().CalculateVector();
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
        [Button("Toggle flow field values")]
        public void ToggleFlowFieldValues()
        {
            showMesh = !showMesh;
            foreach (Transform t in transform)
            {
                FlowFieldCube cube = t.GetComponent<FlowFieldCube>();
                cube.visualizeValue = !cube.visualizeValue;
            }
        }

        [BoxGroup("Visualizing Field")]
        [Button("Toggle visualize vector field")]
        public void ToggleFlowFieldVectors()
        {
            showMesh = !showMesh;
            foreach (Transform t in transform)
            {
                FlowFieldCube cube = t.GetComponent<FlowFieldCube>();
                cube.visualizeVector = !cube.visualizeVector;
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
            foreach(Transform t in transform)
            {
                FlowFieldCube flowFieldCube = t.GetComponent<FlowFieldCube>();
                if (flowFieldCube != ffc)
                {
                    flowFieldCube.RemoveIsTarget();
                }

            }
        }

    }
}