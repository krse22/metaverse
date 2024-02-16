using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassEquip : MonoBehaviour
{

    [SerializeField] private Equipper[] equippers;

    [Button("Equip")]
    public void MassEquippec()
    {
        equippers.ForEach((e) => e.Equippec());

    }

}
