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
        private PlayerManager playerManager;

        private void Awake()
        {
            if (Weapons.Count > 0)
            {
                EquipWeapon(0);
            }
        }

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