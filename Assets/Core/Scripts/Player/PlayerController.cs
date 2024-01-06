using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour, ICameraHolder
{

    [Header("Camera")]
    [SerializeField] private float zPosition;
    [SerializeField] private float yPosition;
    [SerializeField] private float xRotation;

    public override void OnNetworkSpawn()
    {
        if (!IsServer && IsOwner)
        {
            CameraController.SetCameraOwner(this);
        }
    }

    public (Vector3, Vector3) PositionAndRotation()
    {
        Vector3 camTargetPos = new Vector3(transform.position.x, transform.position.y + yPosition, transform.position.z + zPosition);
        Vector3 camTargetRot = new Vector3(xRotation, 0f, 0f);
        return (camTargetPos, camTargetRot);
    }

}
