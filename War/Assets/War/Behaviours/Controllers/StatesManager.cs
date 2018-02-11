using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
    public class StatesManager : MonoBehaviour
    {
        public ControllerStats stats;

        public ControllerStates states;
        public InputVariables inp;


        [System.Serializable]
        public class InputVariables
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
            public Vector3 aimPosition;
            public Vector3 rotateDirection;
        }


        [System.Serializable]
        public class ControllerStates
        {
            public bool onGround;
            public bool isAiming;
            public bool isCrouching;
            public bool isRunning;
            public bool isInteracting;
        }

        #region References

        public Animator anim;
        public GameObject activeModel;

        [HideInInspector]
        public Rigidbody rigid;
        //[HideInInspector]
        public Collider controllerCollider;

        //List of all ragdoll colliders in the character
        List<Collider> ragdollColliders = new List<Collider>();

        //List of all ragdoll rigibodies in the character
        List<Rigidbody> ragdollRigids = new List<Rigidbody>();

        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public LayerMask ignoreForGround;

        [HideInInspector]
        public Transform mTransform;
        public CharacterState charStates;

        public float delta;

        #endregion

        #region Init

        public void Init()
        {
            SetupAnimator();
            mTransform = this.transform;

            //Sets up the rigidbody how we need it in script rather than having to worry about sorting it out in inspector
            rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            controllerCollider.GetComponent<Collider>();

            SetupRagdoll();

            ignoreLayers = ~(1 << 9);
            ignoreForGround = ~(1 << 9 | 1 << 10);
        }

        /// <summary>
        /// Makes sure the anim has an animator and if not will find and set it to anim
        /// </summary>
        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

            if (anim == null)
            {
                anim = activeModel.GetComponent<Animator>();
            }

            anim.applyRootMotion = false;
        }


        /// <summary>
        /// Sets up the characters ragdolls ready to be used
        /// </summary>
        void SetupRagdoll()
        {
            Rigidbody[] rigids = activeModel.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody r in rigids)
            {
                if (r == rigid)
                {
                    continue;
                }
                Collider c = r.gameObject.GetComponent<Collider>();
                c.isTrigger = true;
                ragdollRigids.Add(r);
                ragdollColliders.Add(c);
                r.isKinematic = true;
                r.gameObject.layer = 10;
            }
        }

        #endregion

        #region FixedUpdate

        public void FixedTick(float d)
        {
            delta = d;
            switch (charStates)
            {
                case CharacterState.normal:
                    states.onGround = OnGround();
                    if (states.isAiming)
                    {
                        MovementAiming();
                    }
                    else
                    {
                        //on ground but not aiming
                        MovementNormal();
                    }
                    RotationNormal();

                    break;
                case CharacterState.inAir:
                    rigid.drag = 0;
                    states.onGround = OnGround();
                    break;
                case CharacterState.cover:
                    break;
                case CharacterState.vaulting:
                    break;
                default:
                    break;
            }
        }

        public void MovementNormal()
        {
            if (inp.moveAmount > 0.05f)
            {
                rigid.drag = 0;
            }
            else
            {
                rigid.drag = 4; //Stops you from sliding away when you let go of the buttons
            }

            float speed = stats.walkSpeed;

            if (states.isRunning)
                speed = stats.runSpeed;

            if (states.isCrouching)
                speed = stats.crouchSpeed;

            Vector3 dir = Vector3.zero;
            dir = mTransform.forward * (speed * inp.moveAmount);
            rigid.velocity = dir;
        }

        void RotationNormal()
        {
            if (!states.isAiming)
                inp.rotateDirection = inp.moveDirection;

            Vector3 targetDir = inp.rotateDirection;
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = mTransform.forward;
            }

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(mTransform.rotation, lookDir, stats.rotateSpeed * delta);
            mTransform.rotation = targetRot;
        }

#endregion

        #region Update

        public void Tick(float d)
        {
            delta = d;
            switch (charStates)
            {
                case CharacterState.normal:
                    states.onGround = OnGround();
                    HandleAnimationsAll();
                    break;
                case CharacterState.inAir:
                    states.onGround = OnGround();
                    break;
                case CharacterState.cover:
                    break;
                case CharacterState.vaulting:
                    break;
                default:
                    break;
            }
        }

        void HandleAnimationsAll()
        {
            anim.SetBool(StaticStrings.sprint, states.isRunning);
            anim.SetBool(StaticStrings.aiming, states.isAiming);
            anim.SetBool(StaticStrings.crouch, states.isCrouching);

            if (states.isAiming)
                HandleAnimationsAiming();
            else
                HandleAnimationsNormal();
        }

        void HandleAnimationsNormal()
        {

            if (inp.moveAmount > 0.05f)
            {
                rigid.drag = 0;
            }
            else
            {
                rigid.drag = 4; //Stops you from sliding away when you let go of the buttons
            }

            float anim_v = inp.moveAmount;
            anim.SetFloat(StaticStrings.vertical, anim_v, 0.15f, delta);
        }

        void HandleAnimationsAiming()
        {
            float v = inp.vertical;
            float h = inp.horizontal;
            anim.SetFloat(StaticStrings.horizontal, h, 0.2f, delta);
            anim.SetFloat(StaticStrings.vertical, v, 0.2f, delta);
        }

        public void MovementAiming()
        {
            float speed = stats.aimSpeed;
            Vector3 dir = inp.moveDirection * speed;
            rigid.velocity = dir;
        }

        #endregion 

        /// <summary>
        /// A grounded check
        /// </summary>
        /// <returns></returns>
        bool OnGround()
        {
            Vector3 origin = mTransform.position;
            origin.y += 0.6f;
            Vector3 dir = -Vector3.up;
            float dist = 0.7f;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, dist, ignoreForGround))
            {
                Vector3 tp = hit.point;
                mTransform.position = tp;
                return true;
            }
            return false;
        }
    }

    public enum CharacterState {normal, inAir, cover, vaulting}
}
