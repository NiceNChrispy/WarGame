using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
    public class AeroplanePropellerAnimator : MonoBehaviour
    {
        [SerializeField] private Transform m_PropellorModel;                          // The model of the the aeroplane's propellor.
        [SerializeField] private float m_MaxRpm = 2000;                               // The maximum speed the propellor can turn at.

        private AeroplaneController m_Plane;      // Reference to the aeroplane controller. 
        private const float k_RpmToDps = 60f;     // For converting from revs per minute to degrees per second.


        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Plane = GetComponent<AeroplaneController>();

        }


        private void Update()
        {
            // Rotate the propellor model at a rate proportional to the throttle.
            if(m_Plane.inUse)
            m_PropellorModel.Rotate(0,0, m_MaxRpm * m_Plane.Throttle * Time.deltaTime * k_RpmToDps);

            
        }
    }
}
