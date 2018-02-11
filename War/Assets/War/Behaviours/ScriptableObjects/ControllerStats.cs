using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
    [CreateAssetMenu(menuName = "Controller/Stats")]
    public class ControllerStats : ScriptableObject
    {
        public float walkSpeed = 4; //How fast you move normally
        public float runSpeed = 6; //How fast you move when sprinting
        public float crouchSpeed = 2; //How fast you move when crouching
        public float aimSpeed = 2; //How fast you move whilst ADS
        public float rotateSpeed = 8;
    }
}