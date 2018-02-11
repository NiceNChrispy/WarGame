using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace War
{
    public class InputHandler : MonoBehaviour
    {
        float horizontal;
        float vertical;

        bool aimInput;
        bool sprintInput;
        bool shootInput;
        bool crouchInput;
        bool reloadInput;
        bool switchInput;
        bool pivotInput;

        bool isInit;

        float delta;

        public StatesManager statesMan;

        public CameraHandler camHandler;

        void Start()
        {
            
            InitInGame();
        }

        public void InitInGame()
        {
            statesMan.Init();
            camHandler.Init(this);
            isInit = true;
        }

        private void FixedUpdate()
        {

            if(!isInit)
            {
                return;
            }

            delta = Time.fixedDeltaTime;
            GetInput_FixedUpdate();
            InGame_UpdateStates_FixedUpdate();
            statesMan.FixedTick(delta);
            camHandler.FixedTick(delta);
        }

        void GetInput_FixedUpdate()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        void InGame_UpdateStates_FixedUpdate()
        {
            statesMan.inp.horizontal = horizontal;
            statesMan.inp.vertical = vertical;

            statesMan.inp.moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            Vector3 moveDir = camHandler.mTransform.forward * vertical;
            moveDir += camHandler.mTransform.right * horizontal;
            moveDir.Normalize();
            statesMan.inp.moveDirection = moveDir;
        }

        private void Update()
        {
            if (!isInit)
            {
                return;
            }
            GetInput_Update();
            InGame_UpdateStates_Update();
            delta = Time.deltaTime;
            statesMan.Tick(delta);
        }

        void GetInput_Update()
        {
            aimInput = Input.GetMouseButton(1);
        }

        void InGame_UpdateStates_Update()
        {
            statesMan.states.isAiming = aimInput;
        }

    }

    public enum GamePhase {inGame, inMenu }
}
