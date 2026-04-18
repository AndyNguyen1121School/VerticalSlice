using Player;
using UnityEngine;

namespace Weapons
{
    public class SingleShotGun : Gun
    {
        private bool hasShot;
        public override void Attack(PlayerWeaponManager playerWeaponManager)
        {
            if (hasShot)
                return;
            
            base.Attack(playerWeaponManager);
            hasShot = true;
        }

        public override void AttackCanceled(PlayerWeaponManager playerWeaponManager)
        {
            base.AttackCanceled(playerWeaponManager);
            hasShot = false;
        }
        
    }
}