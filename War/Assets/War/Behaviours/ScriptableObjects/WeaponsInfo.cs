using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon/New Weapon")]
public class WeaponsInfo : ScriptableObject {

    public string weapon_name;
    public enum WeaponClass { SubMachineGun, Rifle, Shotgun, Sniper, Pistol};
    public WeaponClass weaponClass;
    public int maxAmmo;
    public int maxCarryAmount;
    public float fireRate;
    public enum FireType {Single, Semi, Auto };
    public FireType fireType;
}
