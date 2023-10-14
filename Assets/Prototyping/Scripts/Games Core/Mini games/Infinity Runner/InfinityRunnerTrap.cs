using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerTrap : MonoBehaviour, IInfinityRunnerSpawnable
    {

        [SerializeField] private Transform scaler;
        [SerializeField] private float yOffset;
        public float Length { get { return scaler.localScale.z; } }
        public float YOffset { get { return yOffset; } }

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
