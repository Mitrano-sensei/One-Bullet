using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Collider2D))]
    public class SimpleEnemy : MonoBehaviour, IDamageable, IHurtful
    {
        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider2D;

        [Header("Misc")]
        [SerializeField] private string _deathAnimationName = "Death";

        void Start()
        {
            Debug.Log("Start");

            if (_collider2D == null) _collider2D = GetComponent<Collider2D>();
            if (_animator == null) _animator = GetComponent<Animator>();

            if (_collider2D == null) Debug.LogError("Je ne comprend pas");
        }

        public void TakeDamage()
        {
            Debug.Log("Simple enemy died :c From : " + gameObject.name);

            // Disable collider
            _collider2D.enabled = false;

            // Play death animation
            _animator.Play(_deathAnimationName);

            // Disable game object after animation ends
            StartCoroutine(DisableGameObjectAfterAnimation());
        }

        private IEnumerator DisableGameObjectAfterAnimation()
        {
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            gameObject.SetActive(false);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null) Hurt(damageable);
        }

        public void Hurt(IDamageable damageable)
        {
            damageable.TakeDamage();
        }
    }
}
