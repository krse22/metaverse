using UnityEngine;


namespace Prototyping.Games
{
    public class TowerDefenseCameraController : MonoBehaviour, ICameraHolder
    {

        [SerializeField] private GameObject top;
        [SerializeField] private GameObject bottom;

        [SerializeField] private GameObject left;
        [SerializeField] private GameObject right;

        [SerializeField] private Vector3 eulerRotation;

        void Start()
        {
            PlayerCoreCamera.SetCameraOwner(this);
        }

        public (Vector3, Vector3) PositionAndRotation()
        {
            Vector3 originRight = Camera.main.WorldToScreenPoint(right.transform.position);
            Vector3 originTop = Camera.main.WorldToScreenPoint(top.transform.position);

            float y = Camera.main.transform.position.y;

            if (Screen.height < originTop.y || Screen.width < originRight.x)
            {
                y += 100f * Time.deltaTime;
            }

            return (new Vector3(0f, y, 0f), eulerRotation);
        }
    }
}

