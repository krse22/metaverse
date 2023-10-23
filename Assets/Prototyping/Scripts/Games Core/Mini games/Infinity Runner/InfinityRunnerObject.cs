using UnityEngine;


namespace Prototyping.Games
{
    public class InfinityRunnerObject : MonoBehaviour
    {
        [SerializeField] private Transform scaler;
        public float Length { get { return scaler.localScale.z; } }

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
            if (transform.position.z < manager.PlayerZ)
            {
                if (transform.position.z < manager.PlayerZ - Length / 2f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}