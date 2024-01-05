using UnityEngine;

namespace Prototyping.Games
{
    public class TowerDefenseGameManager : MonoBehaviour
    {
        [SerializeField] private LayerMask fieldMask;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask fieldOnlyMask;
        [SerializeField] private LayerMask structureMask;

        [SerializeField] private GameObject ghostStructure;
        [SerializeField] private GameObject testStructure;

        private GameObject currentGhost;
        private GameObject toPlaceOn = null;

        private void Update()
        {
            ContinuallyPositionGhostStructure();
            if (Input.GetKeyUp(KeyCode.Mouse0) && currentGhost != null)
            {
                if (toPlaceOn != null)
                {
                    GameObject go = Instantiate(testStructure);
                    toPlaceOn.GetComponent<TowerDefenseFieldSingle>().IsPlacedOn = true;
                    go.transform.position = toPlaceOn.transform.position + new Vector3(0f, toPlaceOn.transform.localScale.y / 2f + go.transform.localScale.y / 2f, 0f);
                }
                Destroy(currentGhost);
                currentGhost = null;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && currentGhost == null)
            {
                DetectIfStructureHit();
            }
        }

        void DetectIfStructureHit()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y + 10f, structureMask))
            {
                Debug.Log("Structure hit");
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

            toPlaceOn = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y + 10f, fieldMask))
            {
                bool canPlace = CanPlace(hit);
                SetPlaceColor(canPlace);
                currentGhost.transform.position = RaycastedPosition(hit);

                if (canPlace)
                {
                    toPlaceOn = hit.collider.gameObject;
                }
            }
        }

        bool CanPlace(RaycastHit hit)
        {
            int layer = hit.collider.gameObject.layer;
            if (groundMask.ContainsLayer(layer)) return false;

            if (fieldOnlyMask.ContainsLayer(layer))
            {
                var fieldSingle = hit.collider.gameObject.GetComponent<TowerDefenseFieldSingle>();
                return fieldSingle != null && !fieldSingle.IsPlacedOn && fieldSingle.IsOccupied;
            }

            return true;
        }

        Vector3 RaycastedPosition(RaycastHit hit)
        {
            int layer = hit.collider.gameObject.layer;
            if (groundMask.ContainsLayer(layer))
            {
                return hit.point + Vector3.up * 1.5f;
            }

            return hit.collider.gameObject.transform.position + Vector3.up * 1.5f;
        }

        void SetPlaceColor(bool canPlace)
        {
            if (!canPlace)
            {
                Color red = new Color(Color.red.r, Color.red.g, Color.red.b, 0.4f);
                currentGhost.gameObject.GetComponent<MeshRenderer>().material.color = red;
            } else
            {
                Color white = new Color(Color.white.r, Color.white.g, Color.white.b, 0.4f);
                currentGhost.gameObject.GetComponent<MeshRenderer>().material.color = white;
            }
        }

    }
}

