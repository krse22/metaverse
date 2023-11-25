using UnityEngine;


namespace Prototyping.Games
{
    public class InfinityRunnerObject : MonoBehaviour
    {
        [SerializeField] private Transform scaler;
        public float Length { get { return scaler.localScale.z; } }

        private RunnerManagerBase manager;

        public void SetManager(RunnerManagerBase playerRunnerManager)
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
            if (transform.position.z < manager.StartPosition.position.z)
            {
                if (transform.position.z < manager.StartPosition.position.z - Length / 2f - manager.OffsetDeleteFix)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}