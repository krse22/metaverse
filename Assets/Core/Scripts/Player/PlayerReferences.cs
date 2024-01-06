using Unity.Netcode;
using UnityEngine;

public class PlayerReferences : NetworkBehaviour
{

    public Rigidbody rigidbody;
    public CapsuleCollider capsuleCollider;
    public PlayerController controller;
    public PlayerMovement movement;

    public override void OnNetworkSpawn()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        controller = GetComponent<PlayerController>();
        movement = GetComponent<PlayerMovement>();
    }

}
