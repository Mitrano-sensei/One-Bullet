using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    /**
     * Example of a hurtful object (for demo).
     * TODO : Remove this class
     */
    [RequireComponent(typeof(Collider2D))]
    public class HurtExample : MonoBehaviour, IHurtful
    {
        public void Hurt(IDamageable damageable)
        {
            damageable.TakeDamage();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null) Hurt(damageable);
        }
    }
}
