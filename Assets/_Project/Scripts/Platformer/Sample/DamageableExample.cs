using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageableExample : MonoBehaviour, IDamageable
    {
        public void TakeDamage()
        {
            Debug.Log("I Took Damage!! D: " + gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
