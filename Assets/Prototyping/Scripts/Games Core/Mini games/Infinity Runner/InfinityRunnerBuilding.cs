using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerBuilding : MonoBehaviour
    {
        [SerializeField] private Transform scaler;
        [SerializeField] private Transform center;
        public float Length { get { return scaler.localScale.z; } }
        public Transform Center { get { return center; } }
    }
}