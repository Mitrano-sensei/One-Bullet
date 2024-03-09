using FiniteStateMachine;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FlyingEnemy : SimpleEnemy
    {
        [Header("Flying Enemy informations")]

        [SerializeField] private float _patrolSpeed = 5f;
        [SerializeField] private float _chaseSpeed = 5f;
        [SerializeField] private float _chaseDistance = 5f;
        [SerializeField] private List<Transform> _patrolWaypoint = new();

        private StateMachine _stateMachine = new();

        public void Awake()
        {
            // Find player with tag "Player"
            var player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null) Debug.LogError("No player found, please give the player tag to player object");

            var patrolState = new PatrolState(this, player, _patrolWaypoint, _patrolSpeed);
            var chaseState = new ChaseState(this, player, .2f, _chaseSpeed, _chaseDistance);

            At(chaseState, patrolState, new FuncPredicate(() => !chaseState.IsChasing));
            At(patrolState, chaseState, new FuncPredicate(() => patrolState.DistanceToPlayer < _chaseDistance));

            _stateMachine.SetState(patrolState);

            var rb = GetComponent<Rigidbody2D>();
            if (!rb.isKinematic) Debug.LogError("Rb2D set to dynamic, please set it to kinmatic on " + gameObject.name);
            rb.useFullKinematicContacts = true;

        }

        protected override void Start()
        {
            base.Start();
        }

        protected void Update()
        {
            _stateMachine.Update();
        }

        protected void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        /**
         * Creates a transition from one state to another, with a specified condition
         */
        void At(IState from, IState to, IPredicate condition)
        {
            _stateMachine.AddTransition(from, to, condition);
        }

        /**
         * Creates a transition from any state to another, with a specified condition
         */
        void AtAny(IState to, IPredicate condition)
        {
            _stateMachine.AddAnyTransition(to, condition);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }

    }
}
