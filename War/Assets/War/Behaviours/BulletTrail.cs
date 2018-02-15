using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour {

    public int speed;

    private void OnEnable()
    {
        
    }

    void FixedUpdate ()
    {
        transform.Translate(transform.forward * speed, Space.Self);
	}

    private void OnDisable()
    {
        transform.position = Vector3.zero;
    }
}
