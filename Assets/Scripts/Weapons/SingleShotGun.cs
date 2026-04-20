using Player;
using UnityEngine;

namespace Weapons
{
    public class SingleShotGun : Gun
    {
        private bool hasShot;
        public override bool Attack(PlayerWeaponManager playerWeaponManager)
        {
            if (hasShot)
                return false;

            bool result = base.Attack(playerWeaponManager);

            if (result)
                hasShot = true;

            return result;
        }

        public override void AttackCanceled(PlayerWeaponManager playerWeaponManager)
        {
            base.AttackCanceled(playerWeaponManager);
            hasShot = false;
        }
        
    }
}