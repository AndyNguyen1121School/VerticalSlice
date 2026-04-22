using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovementManager : MonoBehaviour
    {
        private PlayerManager _playerManager;
        
        [FormerlySerializedAs("walkSpeed")]
        [Header("Movement Attributes")] 
        [SerializeField] private float walkAcceleration = 5f;
        [SerializeField] private float deceleration = 5f;
        [SerializeField] private float airDeceleration = 0.1f;
        [SerializeField] private float minSpeed = 5f;
        [SerializeField] public float speedCap = 20f;
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float sprintSpeed = 12f;
        public float gravity = -9.81f;
        
        [SerializeField] private float _velocityY;
        [SerializeField] private Vector3 _velocityXZ;

        [Header("Ground Check")] 
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private Vector3 groundCheckOffset;
        [SerializeField] private LayerMask groundLayer;

        private float _pitch;
        private float _yaw;
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;
        private float timeSinceLastLaunch = 0;
        public float HorizontalVelocity => _velocityXZ.magnitude;
        public event Action<float> onSpeedChanged;

        [Header("Jump")]
        private bool canDoubleJump;

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
        }

        // Update is called once per frame
        private void Update()
        {
            HandleMovement();
            HandleRotation();
            timeSinceLastLaunch += Time.deltaTime;
           
        }

        private void HandleMovement()
        {
            Vector2 cachedInputDirection = _playerManager.InputManager.MovementInput;
            Camera cachedCamera = _playerManager.Camera;
            
            if (cachedInputDirection != Vector2.zero)
            {
                Vector3 cameraFlatForwardDir = cachedCamera.transform.forward;
                cameraFlatForwardDir.y = 0;
                cameraFlatForwardDir.Normalize();

                Vector3 cameraFlatRightDir = cachedCamera.transform.right;
                cameraFlatRightDir.y = 0;
                cameraFlatRightDir.Normalize();
                
                Vector3 moveDir = cameraFlatForwardDir * cachedInputDirection.y
                                  + cameraFlatRightDir * cachedInputDirection.x;
                moveDir.Normalize();

                // limit speed
                float currentSpeed;
                if (_velocityXZ.magnitude < minSpeed)
                {
                    currentSpeed = minSpeed;
                }
                else
                {
                    currentSpeed = _velocityXZ.magnitude >= speedCap ? speedCap : _velocityXZ.magnitude;
                }
                
                
                // keep momentum when switching direction
                _velocityXZ = currentSpeed * moveDir;
                _velocityXZ += moveDir * (walkAcceleration * Time.deltaTime);
            }
            else if (IsGrounded() && cachedInputDirection == Vector2.zero)
            {
                _velocityXZ = Vector3.Lerp(_velocityXZ, Vector3.zero, deceleration * Time.deltaTime);
            }
            else if (!IsGrounded() && cachedInputDirection == Vector2.zero)
            {
                _velocityXZ = Vector3.Lerp(_velocityXZ, Vector3.zero, airDeceleration * Time.deltaTime);
            }

            if (!IsGrounded())
            {
                // adjust for gravity
                _velocityY += gravity * Time.deltaTime;
            }
            else if (timeSinceLastLaunch > 0.1f)
            {
                _velocityY = 0;
                canDoubleJump = false;
            }

            if (_playerManager.InputManager.jumpInput && IsGrounded())
            {
                LaunchCharacter(Vector3.up * Mathf.Sqrt(-2 * jumpHeight * gravity));
                canDoubleJump = true;
            }
            else if (_playerManager.InputManager.jumpInput && !IsGrounded() && canDoubleJump)
            {
                LaunchCharacter(Vector3.up * Mathf.Sqrt(-2 * jumpHeight * gravity));
                canDoubleJump = false;
            }

            Vector3 moveVelocity = _velocityXZ;
            moveVelocity.y = _velocityY;
            
            _playerManager.CharacterController.Move(moveVelocity * Time.deltaTime);

            if (moveVelocity != Vector3.zero)
            {
                onSpeedChanged?.Invoke(moveVelocity.magnitude);
            }
            
            _playerManager.Animator.SetBool("IsWalking", IsGrounded() && cachedInputDirection != Vector2.zero);
            _playerManager.Animator.SetFloat("WalkingMultiplier", 1 + (_velocityXZ.magnitude * 0.01f));
        }

        private void HandleRotation()
        {
            if (_playerManager.InputManager.MouseInput == Vector2.zero)
                return;

            Vector2 input = _playerManager.InputManager.MouseInput;

            _yaw += input.x * _playerManager.InputManager.sensitivity;
            _pitch -= input.y * _playerManager.InputManager.sensitivity;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(0, _yaw, 0f);
            _playerManager.CameraTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f); 
        }

        public void LaunchCharacter(Vector3 velocity, bool overrideXZ = false, bool overrideY = false)
        {
            if (overrideXZ)
            {
                _velocityXZ = new Vector3(velocity.x, 0, velocity.z);
            }
            else
            {
                _velocityXZ += new Vector3(velocity.x, 0, velocity.z);
            }

            if (_velocityXZ.magnitude > speedCap)
            {
                _velocityXZ = _velocityXZ.normalized * speedCap;
            }

            if (overrideY)
            {
                _velocityY = velocity.y;
            }
            else
            {
                _velocityY += velocity.y;
            }

            timeSinceLastLaunch = 0;
        }

        public bool IsGrounded()
        {
            return Physics.OverlapSphere(transform.position + groundCheckOffset, groundCheckRadius, groundLayer).Length > 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
        }
    }
}