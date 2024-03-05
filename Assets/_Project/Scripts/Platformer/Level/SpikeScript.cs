using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
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
