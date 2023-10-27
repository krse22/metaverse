using UnityEngine;
using TMPro;

namespace Prototyping
{
    public class GamePortalSingle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        private Vector3 worldPosition;
        private bool isInit = false;
        private GamePortal portal;

        public void Initialize(GamePortal gamePortal, string gameName)
        {
            title.text = gameName;
            transform.SetParent(CoreGameUI.Instance.PortalUI.transform);
            portal = gamePortal;
            worldPosition = portal.transform.position;
            isInit = true;
        }

        public void Update()
        {
            if (isInit)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
                transform.position = screenPos;
            }
        }

        public void Play()
        {
            portal.OnPlay();
        }

    }
}

