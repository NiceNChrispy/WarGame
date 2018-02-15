using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : MonoBehaviour {

    public float throwPower;
    public Transform throwLocation;
    public GameObject throwModel;
    //This will choose which grenade to throw, for now it is just US
    public void Throw()
    {
        GameObject grenade = Instantiate(throwModel, throwLocation.transform.position, Quaternion.identity);
        grenade.GetComponent<Grenades>().Throw(transform.forward * throwPower);
    }
}
