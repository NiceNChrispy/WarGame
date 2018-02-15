using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour {

    public WeaponsInfo weaponInfo;
    public Transform barrelEnd;
    public Transform[] handPositions;
    public int currentAmmo;
    

    public AudioSource source;
    public AudioClip[] clips;

    ObjectPooler pooler;

    private void Start()
    {
        pooler = ObjectPooler.instance;
        currentAmmo = weaponInfo.maxAmmo;
    }

    public void FireWeapon()
    {
        //GunSounds();
        FireBulletObj();
        PlayEffects();
    }

    public bool CheckEmpty()
    {
        if (currentAmmo > 0)
        {
            return true;
        }
        return false;
    }

    public bool CanReload()
    {
        if (currentAmmo < weaponInfo.maxAmmo)
        {
            return true;
        }
        return false;
    }

    public void GunSounds()
    {
        source.PlayOneShot(clips[0]);
    }

    public void ReloadWeapon()
    {
        currentAmmo = weaponInfo.maxAmmo;
        HUDManager.instance.currentAmmo.text = currentAmmo.ToString();
        HUDManager.instance.holdingAmmo.text = weaponInfo.maxCarryAmount.ToString();
    }

    public void FireBulletObj()
    {
        Quaternion quart = Quaternion.Euler(new Vector3(barrelEnd.transform.forward.x, barrelEnd.transform.forward.y, barrelEnd.transform.forward.z));
        GameObject obj = pooler.SpawnFromPool("BulletObjects", barrelEnd.position, quart);
        RaycastHit bulletRay;
        if (Physics.Raycast(barrelEnd.position, transform.forward, out bulletRay, 300f))
        {
            //Debug.Log(bulletRay.transform.name);
            Quaternion normalRot = Quaternion.Euler(bulletRay.normal);
            pooler.SpawnFromPool("BulletHoles", bulletRay.point, normalRot);
        }
        currentAmmo--;
        HUDManager.instance.currentAmmo.text = currentAmmo.ToString();
    }

    public void PlayEffects()
    {
        Quaternion quart = Quaternion.Euler(new Vector3(barrelEnd.transform.forward.x, barrelEnd.transform.forward.y, barrelEnd.transform.forward.z));
        GameObject obj = pooler.SpawnFromPool("MuzzleFlash", barrelEnd.position, quart);
    }
}
