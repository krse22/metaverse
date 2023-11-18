using UnityEngine;
using UnityEngine.Events;

namespace Prototyping.Games
{
    public class InfinityRunnerTutorialActivator : MonoBehaviour
    {
        [SerializeField] private UnityEvent hitEvent;

        private const string PLAYER_TAG = "Player";
        public string side;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(PLAYER_TAG))
            {
                hitEvent.Invoke();
            }
        }

    }
}