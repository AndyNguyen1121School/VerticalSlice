using System;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private PlayerMovementManager movementManager;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private PlayerWeaponManager _weaponManager;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Animator _animator;

    public PlayerInputManager InputManager => inputManager;
    public PlayerMovementManager MovementManager => movementManager;
    public CharacterController CharacterController => characterController;
    public Transform CameraTarget => cameraTarget;
    public Camera Camera => _camera;
    public GameObject BulletPrefab => _bulletPrefab;
    public TrailRenderer TrailRenderer => _trailRenderer;
    public PlayerWeaponManager WeaponManager => _weaponManager;
    public Animator Animator => _animator;

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