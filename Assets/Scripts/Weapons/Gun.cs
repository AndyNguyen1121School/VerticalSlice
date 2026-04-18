using System;
using ScriptableObjects;
using UnityEngine;
using Weapons.Bullet;
using Random = UnityEngine.Random;


namespace Weapons
{
    public class Gun : Weapon
    {
        [SerializeField] private GunData gunData;
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

        public override void Start()
        {
            base.Start();
            playerManager.InputManager.OnAttackPerformed += Attack;
            playerManager.InputManager.OnAttackCanceled += AttackCanceled;
        }

        public override void Attack()
        {
            for (int i = 0; i < gunData.bulletCount; ++i)
            {
                Vector3 randomSpread = GetSpreadDirection();
                Vector3 endPos = playerManager.WeaponManager.gunTip.position + randomSpread * 100f;
                Quaternion spawnDirection = Quaternion.LookRotation((endPos - playerManager.WeaponManager.gunTip.position).normalized);
                BulletManager bullet = Instantiate(playerManager.BulletPrefab, playerManager.WeaponManager.gunTip.position, spawnDirection).GetComponent<BulletManager>();
                bullet.InitializeBulletAttributes(gunData.bulletData);
            }
            
            Vector3 finalPushVelocity = -PlayerManager.Instance.Camera.transform.forward * gunData.pushForce;
            finalPushVelocity.y *= gunData.verticalMultiplier;
            PlayerManager.Instance.MovementManager.LaunchCharacter(finalPushVelocity, false, false);
        }

        public void AttackCanceled()
        {
            
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
    }
}