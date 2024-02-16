using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipper : MonoBehaviour
{

    [SerializeField] private Equipmentizer equip;

    public void Equippec()
    {
        equip.Equip();
    }
}
