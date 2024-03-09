using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer
{
    /**
     * <summary>
     * Simple enemy class.
     * The enemy can take damage and hurt other objects, but won't move at all (except from rigidbody if it has one).
     * </summary>
     */
    [RequireComponent(typeof(Collider2D))]
    public class SimpleEnemy : MonoBehaviour, IDamageable, IHurtful
    {
        [Header("References")]
        [ReadOnly][SerializeField] private string note = "References can be left to null! :)";
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider2D;

        [Header("Events")]
        [SerializeField] private UnityEvent _onDeathEvent = new();

        [Header("Misc")]
        [SerializeField] private string _deathAnimationName = "Death";
        [SerializeField] private bool _debug = true;

        protected virtual void Start()
        {
            if (_collider2D == null) _collider2D = GetComponent<Collider2D>();
            if (_animator == null) _animator = GetComponent<Animator>();
        }

        public void TakeDamage()
        {
            // Logs something for example
            if (_debug) Debug.Log("Simple enemy died :c From : " + gameObject.name);

            // Disable collider
            _collider2D.enabled = false;

            // Play death animation
            _animator.Play(_deathAnimationName);
            
            // Invoke event
            _onDeathEvent?.Invoke();

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
