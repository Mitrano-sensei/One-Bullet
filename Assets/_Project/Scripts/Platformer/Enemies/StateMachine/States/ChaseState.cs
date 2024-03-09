using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class ChaseState : EnemyState
    {
        private float _chaseSpeed;
        private float _chaseDistance;

        private CountdownTimer _chaseTimer;
        public bool IsChasing => !_chaseTimer.IsFinished;
        
        private Rigidbody2D _rb;

        /**
         * 
         * @param chaseTime     the time after which the enemy will stop chasing the player when outside of the chase distance
         */
        public ChaseState(SimpleEnemy enemy, Transform player, float chaseTime, float chaseSpeed, float chaseDistance) : base(enemy, player)
        {
            _chaseTimer = new CountdownTimer(chaseTime);
            _chaseSpeed = chaseSpeed;
            _chaseDistance = chaseDistance;
            _chaseTimer.Start();
        }

        public override void Update()
        {
            base.Update();

            // Manage Timer
            _chaseTimer.Tick(Time.deltaTime);

            if (DistanceToPlayer < _chaseDistance) _chaseTimer.Start();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsChasing) MoveToPlayer();
        }

        public override void OnExit()
        {
            base.OnExit();
            _chaseTimer.Reset();
            FlipSprite();
        }

        private void MoveToPlayer()
        {
            if (_rb == null) _rb = _enemy.GetComponent<Rigidbody2D>();
            _rb.MovePosition(Vector2.MoveTowards(_enemy.transform.position, _player.position, _chaseSpeed * Time.fixedDeltaTime));

            if (_enemy.transform.position.x < _player.position.x) FlipSprite();
            else FlipSprite(false);
        }
    }
}
