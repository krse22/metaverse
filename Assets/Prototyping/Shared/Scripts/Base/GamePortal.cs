using UnityEngine;

namespace Prototyping
{
    public interface IGamePortal
    {
        void Play();
    }

    public class GamePortal : MonoBehaviour
    {
        [SerializeField] private GameObject game;
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
            game.GetComponent<IGamePortal>().Play();
        }

    }
}

