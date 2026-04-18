using System;
using Player;
using ScriptableObjects;
using UnityEngine;
using Weapons.Bullet;
using Random = UnityEngine.Random;


namespace Weapons
{
    public class Gun : Weapon
    {
        [SerializeField] protected GunData gunData;
        [SerializeField] protected bool _canPush = false;
        private float timeSinceLastShot;

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
        }

        private void Update()
        {
            timeSinceLastShot += Time.deltaTime;
        }

        public override void Attack(PlayerWeaponManager playerWeaponManager)
        {
            if (timeSinceLastShot <  1 / gunData.fireRate)
                return;

            timeSinceLastShot = 0;
            
            for (int i = 0; i < gunData.bulletCount; ++i)
            {
                Vector3 randomSpread = GetSpreadDirection();
                Vector3 endPos = playerManager.WeaponManager.gunTip.position + randomSpread * 100f;
                Quaternion spawnDirection = Quaternion.LookRotation((endPos - playerManager.WeaponManager.gunTip.position).normalized);
                BulletManager bullet = Instantiate(playerManager.BulletPrefab, playerManager.WeaponManager.gunTip.position, spawnDirection).GetComponent<BulletManager>();
                bullet.InitializeBulletAttributes(gunData.bulletData);
            }

            if (_canPush)
            {
                Vector3 finalPushVelocity = -PlayerManager.Instance.Camera.transform.forward * gunData.pushForce;
                
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

        private Vector3 GetSpreadDirection()
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

        private void OnDisable()
        {
            _canPush = false;
        }
    }
}