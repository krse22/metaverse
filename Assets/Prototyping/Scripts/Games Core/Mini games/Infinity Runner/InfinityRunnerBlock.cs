using UnityEngine;


namespace Prototyping.Games
{
    public class InfinityRunnerBlock : MonoBehaviour, IInfinityRunnerSpawnable
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

        private PlayerRunnerManager manager;

        public void SetManager(PlayerRunnerManager playerRunnerManager) 
        {
            manager = playerRunnerManager;
        }

        void Update()
        {
            if (manager != null && manager.IsPlaying)
            {
                Vector3 vec = transform.position;
                transform.position = new Vector3(vec.x, vec.y, vec.z - manager.MovementSpeed * Time.deltaTime);
            }
        }

    }
}

