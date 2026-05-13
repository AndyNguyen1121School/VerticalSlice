using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        [Header("Assign in Inspector")] [SerializeField]
        public PlayerInput PlayerInput;

        [Header("Input Info")]
        public float sensitivity;
        [SerializeField] private Vector2 movementInput;
        [SerializeField] private Vector2 mouseInput;
        public Vector2 MovementInput => movementInput;
        public Vector2 MouseInput => mouseInput;
        public bool jumpInput;
        public bool attackInput;
        public bool secondaryInput;

        [HideInInspector] public event Action OnAttackPerformed;
        [HideInInspector] public event Action OnAttackCanceled;
        [HideInInspector] public event Action OnSecondaryPerformed;
        [HideInInspector] public event Action OnSecondaryCanceled;
        [HideInInspector] public event Action<int> OnWeaponSwitching;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (!PlayerPrefs.HasKey("Sensitivity"))
            {
                PlayerPrefs.SetFloat("Sensitivity", 0.12f);
                PlayerPrefs.Save();
            }
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }

        private void Update()
        {
            jumpInput = PlayerInput.actions["Jump"].WasPressedThisFrame();
        }

        private void OnEnable()
        {
            PlayerInput.actions["Move"].performed += OnMove;
            PlayerInput.actions["Move"].canceled += OnMoveCanceled;
            PlayerInput.actions["Look"].performed += OnLook;
            
            PlayerInput.actions["Attack"].performed += HandleAttackPerformed;
            PlayerInput.actions["Attack"].canceled += HandleAttackCanceled;
            PlayerInput.actions["Secondary"].started += HandleSecondaryStarted;
            PlayerInput.actions["Secondary"].canceled += HandleSecondaryCanceled;
            PlayerInput.actions["Weapon1"].started += Weapon1Handler;
            PlayerInput.actions["Weapon2"].started += Weapon2Handler;
            
        }

        private void OnDisable()
        {
            PlayerInput.actions["Move"].performed -= OnMove;
            PlayerInput.actions["Move"].canceled -= OnMoveCanceled;
            PlayerInput.actions["Look"].performed -= OnLook;
            
            PlayerInput.actions["Attack"].performed -= HandleAttackPerformed;
            PlayerInput.actions["Attack"].canceled -= HandleAttackCanceled;
            PlayerInput.actions["Secondary"].started -= HandleSecondaryStarted;
            PlayerInput.actions["Secondary"].canceled -= HandleSecondaryCanceled;
            PlayerInput.actions["Weapon1"].started -= Weapon1Handler;
            PlayerInput.actions["Weapon2"].started -= Weapon2Handler;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            movementInput = ctx.ReadValue<Vector2>();
        }
        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            movementInput = Vector2.zero;;
        }
        private void OnLook(InputAction.CallbackContext ctx)
        {
            mouseInput = ctx.ReadValue<Vector2>() * sensitivity;
        }
        
        private void HandleAttackPerformed(InputAction.CallbackContext ctx)
        {
            attackInput = true;
            OnAttackPerformed?.Invoke();
        }

        private void HandleAttackCanceled(InputAction.CallbackContext ctx)
        {
            attackInput = false;
            OnAttackCanceled?.Invoke();
        }

        private void HandleSecondaryStarted(InputAction.CallbackContext ctx)
        {
            secondaryInput = true;
            OnSecondaryPerformed?.Invoke();
        }

        private void HandleSecondaryCanceled(InputAction.CallbackContext ctx)
        {
            secondaryInput = false;
            OnSecondaryCanceled?.Invoke();
        }

        private void Weapon1Handler(InputAction.CallbackContext ctx)
        {
            OnWeaponSwitching?.Invoke(0);
        }
        
        private void Weapon2Handler(InputAction.CallbackContext ctx)
        {
            OnWeaponSwitching?.Invoke(1);
        }

        public void ResetMouseInput()
        {
            mouseInput = Vector2.zero;
        }
    }
}