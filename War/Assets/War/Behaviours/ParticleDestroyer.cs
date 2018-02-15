using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour {

    ParticleSystem particles;

    public void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (!particles.IsAlive())
        {
            this.gameObject.SetActive(false);
        }
    }
}
