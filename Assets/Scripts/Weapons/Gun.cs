using System;
using System.Collections.Generic;
using Interface;
using Player;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Weapons.Bullet;
using Random = UnityEngine.Random;


namespace Weapons
{
    public class Gun : Weapon
    {
        [SerializeField] protected GunData gunData;
        [SerializeField] protected bool _canPush = false;
        [SerializeField] private Animator gunAnimator;
        private float timeSinceLastShot;
        public Transform gunTip;
        private AnimationClip[] animationClips;
        public string shootAnimationName;
        

        private void OnEnable()
        {
            timeSinceLastShot = 1 / gunData.fireRate;
        }

        private void Awake()
        {
            if (gunData != null)
            {
                damage = gunData.bulletData.damage;
            }
            else
            {
                Debug.LogError("No gun data assigned.");
            }

            animationClips = gunAnimator.runtimeAnimatorController.animationClips;
        }

        private void Update()
        {
            timeSinceLastShot += Time.deltaTime;
        }

        public override bool Attack(PlayerWeaponManager playerWeaponManager)
        {
            if (timeSinceLastShot <  1 / gunData.fireRate)
                return false;

            timeSinceLastShot = 0;

            if (gunData.fireMode == FireMode.Projectile)
            {
                SpawnPhysicalBullets();
            }
            else
            {
                HandleHitscan();
            }
            
            HandleLaunching();
            ActivateCameraShake();  
            PlayShootAnimation();
            

            return true;
        }

        public override void AttackCanceled(PlayerWeaponManager playerWeaponManager)
        {
            
        }

        public override void ActivateSecondary(PlayerWeaponManager playerWeaponManager)
        {
            _canPush = true;
        }

        public override void CancelSecondary(PlayerWeaponManager playerWeaponManager)
        {
            _canPush = false;
        }

        public virtual void HandleHitscan()
        {
           
        }

        private void SpawnPhysicalBullets()
        {
            for (int i = 0; i < gunData.bulletCount; ++i)
            {
                Vector3 randomSpread = GetSpreadDirection();
                Vector3 endPos = playerManager.WeaponManager.gunTip.position + randomSpread * 100f;
                Quaternion spawnDirection = Quaternion.LookRotation((endPos - playerManager.WeaponManager.gunTip.position).normalized);
                BulletManager bullet = playerManager.WeaponManager.bulletPool.Get();
                bullet.transform.rotation = spawnDirection;
                bullet.gameObject.transform.position = playerManager.WeaponManager.gunTip.position;
                bullet.DisableTrailRenderer();
                bullet.InitializeBulletAttributes(gunData.bulletData);
                bullet.EnableTrailRenderer();
            }
        }

        private void HandleLaunching()
        {
            if (!_canPush)
                return;
        
            Vector3 finalPushVelocity = -PlayerManager.Instance.Camera.transform.forward;
            finalPushVelocity.y = 0;
            finalPushVelocity.Normalize();
            finalPushVelocity *= gunData.pushForce;
            
            if (gunData.impulseY)
            {
                finalPushVelocity.y = Mathf.Sqrt(-2 * gunData.verticalForce * playerManager.MovementManager.gravity);
            }
            else
            {
                finalPushVelocity.y += gunData.verticalForce;
            }

            PlayerManager.Instance.MovementManager.LaunchCharacter(finalPushVelocity, false, false);
            
        }

        protected Vector3 GetSpreadDirection()
        {
            Vector3 direction = playerManager.Camera.transform.forward;

            if (gunData.spreadVariance != Vector3.zero)
            {
                direction += new Vector3(
                    Random.Range(-gunData.spreadVariance.x, gunData.spreadVariance.x),
                    Random.Range(-gunData.spreadVariance.y, gunData.spreadVariance.y),
                    Random.Range(-gunData.spreadVariance.z, gunData.spreadVariance.z)
                );

                direction.Normalize();
            }

            return direction;
        }
        private void ActivateCameraShake()
        {
            playerManager.ImpulseSource.GenerateImpulseWithForce(gunData.screenShakeForce);
        }

        private float GetAnimationClipLength(string name)
        {
            foreach (var clip in animationClips)
            {
                if (clip.name == name)
                {
                    return clip.length;
                }
            }
            Debug.LogError("Animation clip " + name + " does not exist");
            return -1;
        }

        private void PlayShootAnimation()
        {
            float cachedClipLength = GetAnimationClipLength(shootAnimationName);
            if (cachedClipLength < 0)
                return;

            gunAnimator.speed = cachedClipLength / (1  / gunData.fireRate);
            gunAnimator.Play("Shoot", 0, 0);
        }
        
        private void OnDisable()
        {
            _canPush = false;
        }
    }
}