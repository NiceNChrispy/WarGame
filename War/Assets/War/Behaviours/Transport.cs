using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Aeroplane;

public class Transport : Interactable
{
    public enum VehicleType {Plane, Truck, Car, tank}
    public VehicleType vehicleType;

    public Transform[] avaliableSeats;

    public int currentOccupants;

    public int VehicleHealth;
    public int VehicleHeathMax = 100;

    public bool inUse;

    public Vector3 seatOffset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CarHasSpace()
    {
        if (currentOccupants < avaliableSeats.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void VehicleInUse(bool used)
    {
        if (used)
        {
            inUse = true;
            
        }
        else
        {
            inUse = false;
        }

        switch (vehicleType)
        {
            case VehicleType.Plane:
                //GetComponent<AeroplaneController>().inUse = used;
                if (used)
                {
                    GetComponentInChildren<Player>().fpsController.ResetAndLock();
                    GetComponentInChildren<Player>().fpsController.lockCam = true;
                }
                else
                {
                    GetComponentInChildren<Player>().fpsController.ResetAndLock();
                    GetComponentInChildren<Player>().fpsController.lockCam = false;
                }                
                break;

            case VehicleType.Truck:
                //HUDManager.instance.currentHealth.text = VehicleHealth.ToString();
                break;

            case VehicleType.Car:
                //HUDManager.instance.currentHealth.text = VehicleHealth.ToString();
                break;

            case VehicleType.tank:

                break;

            default:
                break;
        }

    }

    public void Heal(int amount)
    {
        if (VehicleHealth < VehicleHeathMax)
        {
            VehicleHealth += amount;
        }

        if(inUse)
        HUDManager.instance.currentHealth.text = VehicleHealth.ToString();
    }

    public void Damage(int amount)
    {
        if (VehicleHealth <= 0)
        {
            VehicleKill();
        }

        if (inUse)
            HUDManager.instance.currentHealth.text = VehicleHealth.ToString();
    }

    public void VehicleKill()
    {

    }

}