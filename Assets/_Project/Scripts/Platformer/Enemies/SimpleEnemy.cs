using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Collider2D))]
    public class SimpleEnemy : MonoBehaviour, IDamageable
    {
        public void TakeDamage()
        {
            Debug.Log("Simple enemy died :c From : " + gameObject.name);
            gameObject.SetActive(false);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage();
        }
    }
}
