using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{

    private PlayerReferences references;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotatorSpeed;
    [SerializeField] private Transform rotator;

    private NetworkVariable<float> horizontalInput = new NetworkVariable<float>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> verticalInput = new NetworkVariable<float>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> clientYRotation = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public  bool IsMoving { get { return new Vector3(horizontalInput.Value, 0f, verticalInput.Value).magnitude > 0; } }

    public override void OnNetworkSpawn()
    {
        references = GetComponent<PlayerReferences>();
        if (!IsServer)
        {
            clientYRotation.OnValueChanged += RotationFromServer;
        }
    }

    void RotationFromServer(float prev, float current)
    {
        float lerpedAngle = Mathf.LerpAngle(rotator.eulerAngles.y, current, Time.deltaTime * rotatorSpeed);
        rotator.eulerAngles = new Vector3(0f, lerpedAngle, 0f);
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
        if (!references || !references.rigidbody) return;

        float hor = horizontalInput.Value;
        float ver = verticalInput.Value;

        Vector3 direction = (hor * transform.right + ver * transform.forward).normalized;
        hor = direction.x * movementSpeed * Time.fixedDeltaTime;
        ver = direction.z * movementSpeed * Time.fixedDeltaTime;
        Vector3 velocity = new Vector3(hor, references.rigidbody.velocity.y, ver);
        references.rigidbody.velocity = velocity;

        if (new Vector3(hor, 0f, ver).magnitude > 0f)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            rotator.transform.rotation = rotation;
            clientYRotation.Value = rotation.eulerAngles.y;
        }

    }
}
