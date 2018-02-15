using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {


    public static HUDManager instance;


    public Text currentAmmo;
    public Text holdingAmmo;
    public Text gunName;
    public Text currentHealth;


    private void Awake()
    {
        instance = this;
    }
}
