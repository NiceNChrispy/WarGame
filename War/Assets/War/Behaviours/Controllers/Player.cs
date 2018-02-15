using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour, Damageable {

    public GameObject lookingAt;
    public FirstPersonController fpsController;
    public CharacterController characterCont;
    public Transport currentVehicle;

    private int maxHealth = 100;
    public int playerHealth;

    public GameObject gunObject;

    public enum ControlType {Vehicle, OnFoot }
    public ControlType controlType;

    public Weapons activeWeapon;
    public Weapons weaponSlotOne;
    public Weapons weaponSlotTwo;
    public Throwables throwableSlot;

    // Use this for initialization
    void Start ()
    {
        activeWeapon = weaponSlotOne;
        weaponSlotTwo.gameObject.SetActive(false);
        HUDManager.instance.gunName.text = activeWeapon.weaponInfo.weapon_name;
        HUDManager.instance.holdingAmmo.text = activeWeapon.weaponInfo.maxCarryAmount.ToString();
        HUDManager.instance.currentHealth.text = playerHealth.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (controlType == ControlType.OnFoot)
        {
            GroundControls();
        }

            AllControls();
    }

	void FixedUpdate ()
    {
        RaycastHit lookAtHit;
        if (Physics.Raycast(transform.position, transform.forward, out lookAtHit, 3f))
        {
            if (lookAtHit.transform.gameObject.GetComponentInParent<Interactable>())
            {
                lookingAt = lookAtHit.transform.gameObject;
            }
            else
            {
                lookingAt = null;
            }
        }
        else
        {
            lookingAt = null;
        }

    }

    void Interact()
    {
        if (!fpsController.inVehicle)
        {
            if (lookingAt != null)
            {
                if (lookingAt.gameObject.GetComponent<Transport>())
                {
                    GetInVehicle(lookingAt.gameObject.GetComponent<Transport>());
                }
            }
        }
        else
        {
            GetOutOfVehicle();
        }
    }

    void ActivateObject()
    {

    }

    void AllControls()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void GroundControls()
    {

        if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(weaponSlotOne);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(weaponSlotTwo);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            throwableSlot.Throw();
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadWeapon();
        }
    }

    public void SwitchWeapon(Weapons newEquipped)
    {
        activeWeapon.gameObject.SetActive(false);
        activeWeapon = newEquipped;
        activeWeapon.gameObject.SetActive(true);
        HUDManager.instance.gunName.text = activeWeapon.weaponInfo.weapon_name;
        HUDManager.instance.holdingAmmo.text = activeWeapon.weaponInfo.maxCarryAmount.ToString();
        HUDManager.instance.currentAmmo.text = activeWeapon.currentAmmo.ToString();
    }

    public void FireWeapon()
    {
        if (activeWeapon.CheckEmpty())
        {
            activeWeapon.FireWeapon();
        }
    }

    public void ReloadWeapon()
    {
        if (activeWeapon.CanReload())
        {
            activeWeapon.ReloadWeapon();
        }
        else
        {
            return;
        }
    }

    public void Heal(int amount)
    {
        if (controlType == ControlType.OnFoot)
        {
            if (playerHealth < maxHealth)
            {
                playerHealth += amount;
            }
            HUDManager.instance.currentHealth.text = playerHealth.ToString();
        }
    }

    public void Damage(int amount)
    {
        if (controlType == ControlType.OnFoot)
        {
            if (playerHealth <= 0)
            {
                KillPlayer();
            }
            HUDManager.instance.currentHealth.text = playerHealth.ToString();
        }
    }

    public void KillPlayer()
    {

    }

    void GetInVehicle(Transport vehicle)
    {
        //int seatNum;
        gunObject.SetActive(false);
        currentVehicle = vehicle;
        characterCont.enabled = false;
        transform.position = currentVehicle.avaliableSeats[0].transform.position - currentVehicle.seatOffset;
        fpsController.inVehicle = true;
        transform.parent = currentVehicle.avaliableSeats[0].transform;
        currentVehicle.VehicleInUse(true);
        controlType = ControlType.Vehicle;
        //if (vehicle.CarHasSpace())
        //{
        //}
    }

    void GetOutOfVehicle()
    {
        gunObject.SetActive(true);
        fpsController.inVehicle = false;
        transform.position = transform.position += new Vector3(-2, 0, 0);
        characterCont.enabled = true;
        currentVehicle.VehicleInUse(false);
        transform.parent = null;
        currentVehicle = null;
        controlType = ControlType.OnFoot;
        //HUDManager.instance.currentHealth.text = playerHealth.ToString();
    }
}
