using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SpikeScript : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();

            damageable?.TakeDamage();
        }
    }
}
