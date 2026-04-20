using System;
using System.Collections;
using UnityEngine;

namespace Weapons.Trail
{
    public class TrailShrink : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trail;
        private void Start()
        {
            StartCoroutine(ShrinkTrail());
        }

        private IEnumerator ShrinkTrail()
        {
            float startingWidthMultiplier = trail.widthMultiplier;
            float timeElapsed = 0;
            float duration = trail.time;
            while (timeElapsed < duration)
            {
                trail.widthMultiplier = Mathf.Lerp(startingWidthMultiplier, 0, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            trail.widthMultiplier = 0;
        }
        
        
    }
}