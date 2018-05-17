using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New RangedWeapon", menuName = "Item/WeaponItem/Ranged")]
public class Ranged : WeaponItem {

    [Header("Ranged")]
    public float range;
    public int ammo;
}
