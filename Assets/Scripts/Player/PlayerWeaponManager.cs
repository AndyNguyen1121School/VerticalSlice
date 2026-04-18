using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerWeaponManager : MonoBehaviour
    {
        [SerializeField] private List<Weapon> Weapons;
        [SerializeField] private Weapon currentWeapon;
        public Transform gunTip;

        private void Awake()
        {
            if (Weapons.Count > 0)
            {
                EquipWeapon(0);
            }
        }

        private void EquipWeapon(int index)
        {
            if (Weapons.Count > index)
            {
                currentWeapon = Weapons[index];
            }
        }
        
        
    }
}