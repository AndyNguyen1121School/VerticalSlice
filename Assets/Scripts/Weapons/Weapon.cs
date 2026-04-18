using System;
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

        public abstract void Attack();
    }
}