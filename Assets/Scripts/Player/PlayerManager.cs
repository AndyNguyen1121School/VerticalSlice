using System;
using Player;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private PlayerMovementManager movementManager;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Camera _camera;

    public PlayerInputManager InputManager => inputManager;
    public PlayerMovementManager MovementManager => movementManager;
    public CharacterController CharacterController => characterController;
    public Transform CameraTarget => cameraTarget;
    public Camera Camera => _camera;

    public static PlayerManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        { 
            Debug.LogError("More than one playerManager in the scene. Destroyed.");
            Destroy(transform.parent.gameObject);
        }
    }
}