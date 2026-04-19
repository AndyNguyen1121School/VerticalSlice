using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using UnityEngine.Pool;
using Weapons.Bullet;


namespace Player
{
    public class PlayerWeaponManager : MonoBehaviour
    {
        [SerializeField] private List<Weapon> Weapons;
        [SerializeField] private Weapon currentWeapon;
        public Transform gunTip;
        private PlayerManager playerManager;

        public ObjectPool<BulletManager> bulletPool;

        private void Awake()
        {
            if (Weapons.Count > 0)
            {
                EquipWeapon(0);
            }

            bulletPool = new ObjectPool<BulletManager>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 400, 5000);
        }

        #region ObjectPool

        private BulletManager CreatePooledItem()
        {
            BulletManager bullet = Instantiate(playerManager.BulletPrefab).GetComponent<BulletManager>();
            bullet.trail.emitting = false;
            return bullet;
        }

        private void OnTakeFromPool(BulletManager bullet)
        {
            bullet.trail.Clear();
            bullet.gameObject.SetActive(true);
            StartCoroutine(ReturnToPool(bullet, 5f));
        }
        private void OnReturnedToPool(BulletManager bullet)
        {
            bullet.gameObject.SetActive(false);
            bullet.trail.emitting = false;
            bullet.trail.Clear();
        }
        private void OnDestroyPoolObject(BulletManager bullet)
        {
            Destroy(bullet);
        }

        private IEnumerator ReturnToPool(BulletManager bullet, float duration)
        {
            yield return new WaitForSeconds(duration);
            bulletPool.Release(bullet);
        }

        #endregion
        
        private void Start()
        {
            playerManager = PlayerManager.Instance;
            playerManager.InputManager.OnAttackPerformed += AttemptToAttack;
            playerManager.InputManager.OnAttackCanceled += CancelAttack;
            playerManager.InputManager.OnSecondaryPerformed += ActivateSecondary;
            playerManager.InputManager.OnSecondaryCanceled += CancelSecondary;
        }

        private void OnDisable()
        {
            playerManager.InputManager.OnAttackPerformed -= AttemptToAttack;
            playerManager.InputManager.OnAttackCanceled -= CancelAttack;
            playerManager.InputManager.OnSecondaryPerformed -= ActivateSecondary;
            playerManager.InputManager.OnSecondaryCanceled -= CancelSecondary;
        }

        private void Update()
        {
            if (playerManager.InputManager.attackInput)
            {
                AttemptToAttack();
            }
        }

        private void EquipWeapon(int index)
        {
            if (Weapons.Count > index)
            {
                currentWeapon = Weapons[index];
            }
        }

        private void AttemptToAttack()
        {
            if (currentWeapon != null)
            {
                currentWeapon.Attack(this);
            }
        }

        private void CancelAttack()
        {
            if (currentWeapon != null)
            {
                currentWeapon.AttackCanceled(this);
            }
        }

        private void ActivateSecondary()
        {
            if (currentWeapon != null)
            {
                currentWeapon.ActivateSecondary(this);
            }
        }

        private void CancelSecondary()
        {
            if (currentWeapon != null)
            {
                currentWeapon.CancelSecondary(this);
            }
        }
    }
}