using UnityEngine;
using Sirenix.OdinInspector;

namespace Prototyping.Games {
    public class TowerDefenseFlowField : MonoBehaviour
    {
        [SerializeField] private GameObject flowFieldManager;

        [BoxGroup("Field size")]
        [SerializeField] private GameObject top;

        [BoxGroup("Field size")]
        [SerializeField] private GameObject right;

        [SerializeField] private LayerMask waveFrontLayer;

        [InfoBox("NAME of this game object must be CONSTRUCTION and must be unique in a level, Only one with this name so editor can reference it", InfoMessageType.Warning)]

        public (Transform, Transform) GetDimensions { get { return (top.transform, right.transform); } }

        public LayerMask WaveFrontLayer => waveFrontLayer;

        [Button("Create new flow field", ButtonSizes.Large)]
        public void CreateField()
        {
            GameObject go = Instantiate(flowFieldManager);
            go.transform.position = Vector3.zero;
        }

    }
}

