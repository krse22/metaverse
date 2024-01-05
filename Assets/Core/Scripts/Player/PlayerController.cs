using Prototyping;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform rotator;

    private NetworkVariable<float> horizontalInput = new NetworkVariable<float>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> verticalInput = new NetworkVariable<float>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> clientYRotation = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody>();
        if (!IsServer)
        {
            clientYRotation.OnValueChanged += RotationFromServer;
        }
    }

    void RotationFromServer(float prev, float current)
    {
        rotator.eulerAngles = new Vector3(0f, current, 0f);
    }

    private void Update()
    {
        if (!IsServer && IsOwner)
        {
            SampleInputs();
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            UpdatePositionAndRotation();
        }
    }

    void SampleInputs()
    {
        horizontalInput.Value = PlayerJoystickInput.Horizontal;
        verticalInput.Value = PlayerJoystickInput.Vertical;
    }

    void UpdatePositionAndRotation()
    {
        if (!rb) return;

        float hor = horizontalInput.Value;
        float ver = verticalInput.Value;
        if (new Vector3(hor, 0, ver).magnitude > 0)
        {
            Vector3 direction = (hor * transform.right + ver * transform.forward).normalized;
            hor = direction.x * movementSpeed * Time.fixedDeltaTime;
            ver = direction.z * movementSpeed * Time.fixedDeltaTime;
            Vector3 velocity = new Vector3(hor, rb.velocity.y, ver);
            rb.velocity = velocity;

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            rotator.transform.rotation = rotation;
            clientYRotation.Value = rotation.eulerAngles.y;
        } else
        {
            rb.velocity = Vector3.zero;
        }


    }

}
