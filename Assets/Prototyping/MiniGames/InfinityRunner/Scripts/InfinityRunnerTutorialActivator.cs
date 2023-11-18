using UnityEngine;

namespace Prototyping.Games
{
    public class InfinityRunnerTutorialActivator : MonoBehaviour
    {

        [SerializeField] private InfinityRunnerTutorial manager;

        private const string PLAYER_TAG = "Player";
        public string side;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(PLAYER_TAG))
            {
                manager.OnTutorialHit(side);
            }
        }

    }
}