using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour {

    public enum GrenadeType {Frag, Molotov }
    public GrenadeType grenadeType;

    public int FuseTime;

    public Rigidbody rb;

    public float explosionForce;
    public float explosionRadius;

    public Collider[] colliders;


    private void OnEnable()
    {
        Invoke("Explode", FuseTime);
    }

    public void Throw(Vector3 Force)
    {
        rb.AddForce(Force);
    }

    public void FixedUpdate()
    {
        Vector3 explosionPos = transform.position;
        colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
    }

    private void Explode()
    {
        foreach (Collider hit in colliders)
        {
            Rigidbody rigids = hit.GetComponent<Rigidbody>();

            if (rigids != null)
                rigids.AddExplosionForce(explosionForce, transform.position, explosionRadius, 10f);
        }
        ObjectPooler.instance.SpawnFromPool("LargeExplosion", transform.position, Quaternion.identity);
        transform.position = Vector3.zero;
        Destroy(this.gameObject);

    }
}
