using System;
using System.Collections;
using Cinemachine;
using Interface;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour, IDamageable
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
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    public PlayerInputManager InputManager => inputManager;
    public PlayerMovementManager MovementManager => movementManager;
    public CharacterController CharacterController => characterController;
    public Transform CameraTarget => cameraTarget;
    public Camera Camera => _camera;
    public GameObject BulletPrefab => _bulletPrefab;
    public TrailRenderer TrailRenderer => _trailRenderer;
    public PlayerWeaponManager WeaponManager => _weaponManager;
    public Animator Animator => _animator;
    public CinemachineImpulseSource ImpulseSource => _impulseSource;
    public CinemachineVirtualCamera CameraObject;

    private bool hasDied;
    
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

    private void Die()
    {
        CameraObject.Follow = null;
        Rigidbody rb = CameraObject.gameObject.AddComponent<Rigidbody>();
        CameraObject.gameObject.AddComponent<SphereCollider>();
        Vector3 impulseDirection = -transform.forward;
        impulseDirection += -transform.right;
        rb.AddForce(impulseDirection * 2, ForceMode.Force);
    }

    public void Damage(float damage)
    {
        if (!hasDied)
        {
            Die();
            hasDied = true;
            StartCoroutine(ShowDeathScreen());
        }
    }

    private IEnumerator ShowDeathScreen()
    {
        yield return new WaitForSeconds(3f);
        UIManager.instance.ActivateDeathScreen();
    }
}