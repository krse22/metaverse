using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerCollision : MonoBehaviour
    {

        [SerializeField] private string obsticleTag;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(obsticleTag))
            {
                GetComponent<PlayerRunnerController>().ObsticleHit();
            }
        }

    }
}