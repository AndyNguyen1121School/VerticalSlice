using System;
using Player;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected float damage;
        protected PlayerManager playerManager;

        public virtual void Start()
        {
            playerManager= PlayerManager.Instance;
        }

        public abstract bool Attack(PlayerWeaponManager playerWeaponManager);
        public abstract void AttackCanceled(PlayerWeaponManager playerWeaponManager);

        public abstract void ActivateSecondary(PlayerWeaponManager playerWeaponManager);
        public abstract void CancelSecondary(PlayerWeaponManager playerWeaponManager);
    }
}