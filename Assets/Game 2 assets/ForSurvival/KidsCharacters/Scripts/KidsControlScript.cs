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

        private static readonly int SpeedParameter = Animator.StringToHash("Speed");
        private static readonly int JumpPoseParameter = Animator.StringToHash("JumpPose");

        public float walkSpeed = 3.0f;
        public float runSpeed = 6.0f;
        public float jumpPower = 7.0f;
        private float gravity = 9.81f;
        private bool isGrounded;
        private float turnSmoothVelocity;
        public float turnSmoothTime = 0.1f;

        void Start()
        {
            _Animator = GetComponent<Animator>();
            _Ctrl = GetComponent<CharacterController>();
            _View_Camera = Camera.main.gameObject;

            Debug.Log("KidsControlScript Initialized");
        }

        void Update()
        {
            CAMERA_FOLLOW();
            MOVE();
            APPLY_GRAVITY();
        }

        private void CAMERA_FOLLOW()
        {
            Vector3 offset = new Vector3(0, 2.5f, -5f);
            _View_Camera.transform.position = transform.position + transform.rotation * offset;
            _View_Camera.transform.LookAt(transform.position + Vector3.up * 1.5f);
        }

        private void MOVE()
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
            float vertical = Input.GetAxisRaw("Vertical"); // W/S or Up/Down Arrow

            Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

            if (moveDir.magnitude >= 0.1f)
            {
                Debug.Log("Movement Key Pressed: " + moveDir); // Debug movement detection

                // Convert movement direction to world space
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + _View_Camera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);

                Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                _Ctrl.Move(move * speed * Time.deltaTime);

                _Animator.SetFloat(SpeedParameter, speed / runSpeed);
                Debug.Log("Animation Speed Set: " + (speed / runSpeed)); // Debug animation
            }
            else
            {
                _Animator.SetFloat(SpeedParameter, 0);
                Debug.Log("No Movement Detected"); // Debug if no movement
            }

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump Key Pressed"); // Debug jump detection
                _MoveDirection.y = jumpPower;
                _Animator.CrossFade("Base Layer.jump", 0.1f, 0, 0);
            }
        }

        private void APPLY_GRAVITY()
        {
            isGrounded = _Ctrl.isGrounded;
            if (isGrounded && _MoveDirection.y < 0)
                _MoveDirection.y = -2f;

            _MoveDirection.y -= gravity * Time.deltaTime;
            _Ctrl.Move(_MoveDirection * Time.deltaTime);
        }
    }
}
