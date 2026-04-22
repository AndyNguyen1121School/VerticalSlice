using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class FractureEnemy : MonoBehaviour
    {
       [SerializeField] private List<Rigidbody> bodyParts;
       [SerializeField] private float force;
       [SerializeField] private float radius;
       [SerializeField] private float upwardsForce;
       [SerializeField] private float despawnDelay = 5f;
       [SerializeField] private float despawnDuration = 5f;

       private void Start()
       {
           foreach (Rigidbody rb in bodyParts)
           {
               rb.AddExplosionForce(force, transform.position, radius, upwardsForce);
           }

           StartCoroutine(DespawnGameObject());
       }

       private IEnumerator DespawnGameObject()
       {
           yield return new WaitForSeconds(despawnDelay);

           float elapsedTime = 0;
           while (elapsedTime < despawnDuration)
           {
               elapsedTime += Time.deltaTime;
               Vector3 currentShrinkScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime / despawnDuration);
               
               foreach (Rigidbody rb in bodyParts)
               {
                   rb.isKinematic = true;
                   rb.detectCollisions = false;
                   rb.transform.localScale = currentShrinkScale;
               }
               yield return null;
           }
           
           Destroy(gameObject);
       }
    }
}