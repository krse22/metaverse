using UnityEngine;

namespace Prototyping
{
    public class GamePortal : MonoBehaviour
    {
        [SerializeField] private string gameName;
        private GamePortalSingle portalSingle;

        void Start()
        {
            GameObject portalUI = Instantiate(GameCore.Instance.GamePortalSingle);
            portalSingle = portalUI.GetComponent<GamePortalSingle>();
            portalSingle.Initialize(this, gameName);
        }

        public void OnPlay()
        {
            Debug.Log("Play " + gameName);
        }

    }
}

