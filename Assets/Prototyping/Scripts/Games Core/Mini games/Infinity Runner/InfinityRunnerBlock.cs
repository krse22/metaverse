using UnityEngine;


namespace Prototyping.Games
{
    public class InfinityRunnerBlock : MonoBehaviour
    {
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private Transform scaler;
        [SerializeField] private Transform top;

        public Transform Start { get { return start; } }
        public Transform End { get { return end; } }
        public Transform Top { get { return top; } }
        public float Length { get { return scaler.localScale.z; } }
        public bool TrapsSpawned { get; set; }

    }
}

