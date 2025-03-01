using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample
{
    public class KidsControlScript : MonoBehaviour
    {
        private Animator _Animator;
        private CharacterController _Ctrl;
        private Vector3 _MoveDirection = Vector3.zero;
        private GameObject _View_Camera;

        private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
        private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
        private static readonly int JumpState = Animator.StringToHash("Base Layer.jump");
        private static readonly int DiveState = Animator.StringToHash("Base Layer.dive");
        private static readonly int LandingState = Animator.StringToHash("Base Layer.landing");

        private static readonly int SpeedParameter = Animator.StringToHash("Speed");
        private static readonly int JumpPoseParameter = Animator.StringToHash("JumpPose");

        void Start()
        {
            _Animator = GetComponent<Animator>();
            _Ctrl = GetComponent<CharacterController>();
            _View_Camera = Camera.main.gameObject;
            _Ctrl.enabled = true;
        }

        void Update()
        {
            CAMERA();
            GRAVITY();
            MOVE();
            JUMP();
            DIVE();
        }

        private void CAMERA()
        {
            _View_Camera.transform.position = transform.position + new Vector3(0, 0.5f, 2.0f);
        }

        private void GRAVITY()
        {
            if (_Ctrl.enabled)
            {
                if (CheckGrounded() && _MoveDirection.y < -0.1f)
                    _MoveDirection.y = -0.1f;
                else
                    _MoveDirection.y -= 9.81f * Time.deltaTime;

                _Ctrl.Move(_MoveDirection * Time.deltaTime);
            }
        }

        private bool CheckGrounded()
        {
            return _Ctrl.isGrounded || Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
        }

        private void MOVE()
        {
            float speed = _Animator.GetFloat(SpeedParameter);
            if (Input.GetKey(KeyCode.Z))
                speed = Mathf.Min(speed + 0.01f, 2f);
            else
                speed = Mathf.Max(speed - 0.01f, 1f);

            _Animator.SetFloat(SpeedParameter, speed);

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Vector3 velocity = transform.forward * speed * 3f * Time.deltaTime;
                _Ctrl.Move(velocity);
            }

            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.up, 2.0f);
            else if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.up, -2.0f);
        }

        private void JUMP()
        {
            if (CheckGrounded() && Input.GetKeyDown(KeyCode.S))
            {
                _Animator.CrossFade(JumpState, 0.1f, 0, 0);
                _MoveDirection.y = 5.0f;
                _Animator.SetFloat(JumpPoseParameter, _MoveDirection.y);
            }

            if (!CheckGrounded() && _Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == JumpState)
                _Animator.SetFloat(JumpPoseParameter, _MoveDirection.y);
        }

        private void DIVE()
        {
            if (!CheckGrounded() && Input.GetKeyDown(KeyCode.S) && _Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == JumpState)
            {
                _Animator.CrossFade(DiveState, 0.1f, 0, 0);
                _MoveDirection.y += 3.0f;
            }
        }
    }
}