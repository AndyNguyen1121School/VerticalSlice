using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderComponent : MonoBehaviour
{
    public event Action<Collider> OnCollisionStay;
    private void OnTriggerStay(Collider other)
    {
        OnCollisionStay?.Invoke(other);
    }
}
