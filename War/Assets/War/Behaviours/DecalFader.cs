using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalFader : MonoBehaviour {

	// Use this for initialization
	void OnEnable ()
    {
        Invoke("TurnOff", 5f);
	}

    void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
}
