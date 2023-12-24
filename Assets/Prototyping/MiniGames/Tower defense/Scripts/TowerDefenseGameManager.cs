using UnityEngine;

namespace Prototyping.Games
{
    public class TowerDefenseGameManager : MonoBehaviour
    {
        [SerializeField] private LayerMask fieldMask;
        [SerializeField] private GameObject ghostStructure;
        private GameObject currentGhost;

        private void Update()
        {
            ContinuallyPositionGhostStructure();
            if (Input.GetKeyUp(KeyCode.Mouse0) && currentGhost != null)
            {
                currentGhost = null;
                Destroy(currentGhost);
            }
        }

        public void EnterPlaceMode(GameObject button) 
        {
            GameObject go = Instantiate(ghostStructure);
            currentGhost = go;
        }

        public void ContinuallyPositionGhostStructure()
        {
            if (currentGhost == null) return;

            Debug.Log("called");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y + 10f, fieldMask))
            {
                Debug.Log(hit.transform.name);
                currentGhost.transform.position = hit.collider.gameObject.transform.position + Vector3.up * 1.5f;
            }

        }

    }
}

