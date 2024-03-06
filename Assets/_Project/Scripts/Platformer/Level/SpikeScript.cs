using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    /**
     * A spike is only an object that will hurt things that touch it.
     */
    public class SpikeScript : MonoBehaviour, IHurtful
    {
        public void Hurt(IDamageable damageable)
        {
            damageable.TakeDamage();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null) Hurt(damageable);
        }
    }
}
