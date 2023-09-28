using System.Collections;
using UnityEngine;

public class InfinityRunnerTrap : MonoBehaviour
{

    [SerializeField] private Transform scaler;
    [SerializeField] private float yOffset;
    public float Length { get { return scaler.localScale.z; } }
    public float YOffset { get { return yOffset; } }
 

}
