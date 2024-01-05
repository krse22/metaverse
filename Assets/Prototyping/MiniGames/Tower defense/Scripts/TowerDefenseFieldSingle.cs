using Sirenix.OdinInspector;
using UnityEngine;

namespace Prototyping.Games
{
    public class TowerDefenseFieldSingle : MonoBehaviour
    {

        [SerializeField] private BoxCollider boxCollider;


        [InfoBox("Can you place objects on this field box instance")]
        public bool IsOccupied;

        [InfoBox("Is a structure placed on this field")]
        public bool IsPlacedOn;

        public void SetIntoSpawnable(GameObject graphics)
        {
            graphics.transform.parent = transform;
            graphics.transform.localPosition = Vector3.zero;
            IsOccupied = true;
        }

        public void RemoveSpawnable()
        {
            var t = transform.GetChild(0);
            if (t != null)
            {
                DestroyImmediate(t.gameObject);
            }
            IsOccupied = false;
        }

    }
}

