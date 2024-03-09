using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{

    public class PatrolState : EnemyState
    {
        private float _patrolSpeed = 5f;
        private List<Transform> _patrolWaypoint = new();

        private int _currentWaypointIndex = 0;

        public PatrolState(SimpleEnemy enemy, Transform player, List<Transform> patrolWaypoint, float patrolSpeed) : base(enemy, player)
        {
            _patrolWaypoint = patrolWaypoint;
            _patrolSpeed = patrolSpeed;
        }

        public override void Update()
        {
            MoveToWaypoint();
        }

        private void MoveToWaypoint()
        {
            if (_patrolWaypoint.Count == 0) return;

            var target = _patrolWaypoint[_currentWaypointIndex].position;
            var direction = (target - _enemy.transform.position).normalized;

            _enemy.transform.position += direction * _patrolSpeed * Time.deltaTime;

            if (Vector2.Distance(_enemy.transform.position, target) < 0.1f)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _patrolWaypoint.Count;
            }

            HandleFlipSprite();
        }

        private void HandleFlipSprite()
        {
            if (_enemy.transform.position.x < _patrolWaypoint[_currentWaypointIndex].position.x) FlipSprite();
            else FlipSprite(false);
        }
    }
}
