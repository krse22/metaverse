using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{

    private NetworkVariable<bool> IsMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private PlayerReferences playerReferences;
    
    [SerializeField] private GameObject malePrefab;
    private Animator animator;

    [SerializeField] private Transform rotator;

    public override void OnNetworkSpawn()
    {
        GameObject go = Instantiate(malePrefab);
        animator = go.GetComponent<Animator>();

        playerReferences = GetComponent<PlayerReferences>();
        go.transform.SetParent(rotator, false);
        go.transform.localPosition = -Vector3.up;
       
    }

    private void Update()
    {
        if (animator && playerReferences)
        {
            if (IsServer) 
            { 
                IsMoving.Value = playerReferences.movement.IsMoving;
            }
            if (IsClient)
            {
                animator.SetBool("IsMoving", IsMoving.Value);
            }
        }
    }

}
