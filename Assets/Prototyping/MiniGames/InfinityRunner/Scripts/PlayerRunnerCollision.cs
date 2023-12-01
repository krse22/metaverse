using UnityEngine;

namespace Prototyping.Games
{
    public class PlayerRunnerCollision : MonoBehaviour
    {

        [SerializeField] private string obsticleTag;
        [SerializeField] private LayerMask ground;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(obsticleTag))
            {
                GetComponent<PlayerRunnerController>().ObsticleHit();
            }
            if (ground.ContainsLayer(collision.gameObject.layer))
            {
                GetComponent<PlayerRunnerController>().GroundHit();
            }
        }

    }
}