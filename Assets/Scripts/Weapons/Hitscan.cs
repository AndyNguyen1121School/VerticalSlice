using Interface;
using UnityEngine;

namespace Weapons
{
    public class Hitscan : Gun
    {
        [Header("Hitscan")] 
        [SerializeField] protected LayerMask whatIsDamageable;
        [SerializeField] private ParticleSystem bulletParticle;
        public override void HandleHitscan()
        {
            base.HandleHitscan();
            
            RaycastHit hit;
            Vector3 startPos = PlayerManager.Instance.WeaponManager.gunTip.position;
            Vector3 endPos;
            Vector3 dir = GetSpreadDirection();


            if (Physics.Raycast(PlayerManager.Instance.Camera.transform.position, PlayerManager.Instance.Camera.transform.forward, out hit, 100f,
                    whatIsDamageable))
            {
                IDamageable damageScript;
                if (hit.collider.TryGetComponent<IDamageable>(out damageScript))
                {
                    damageScript.Damage(gunData.bulletData.damage);
                }
                ActivateParticle(hit.point);
                endPos = hit.point;
            }
            else
            {
                endPos = startPos + dir * 100;
            }
            
            TrailRenderer trailRenderer = Instantiate(PlayerManager.Instance.TrailRenderer, startPos, Quaternion.identity);
            if (trailRenderer != null)
            {
                trailRenderer.colorGradient = gunData.bulletData.trailColor;
                trailRenderer.AddPosition(startPos);
                trailRenderer.transform.position = endPos;
                Destroy(trailRenderer.gameObject, trailRenderer.time);
            }
        }

        private void ActivateParticle(Vector3 particlePosition)
        {
            ParticleSystem particle = Instantiate(bulletParticle, particlePosition, Quaternion.identity);
            ParticleSystem.MainModule main = particle.main;
            main.startColor = gunData.bulletData.trailColor;
        }
    }
}