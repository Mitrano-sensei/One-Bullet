using DG.Tweening;
using FiniteStateMachine;
using UnityEngine;

namespace Platformer
{
    public class EnemyState : BaseState
    {
        public static int _globalId = 0;

        protected SimpleEnemy _enemy;
        protected Transform _player;
        public float DistanceToPlayer => Vector2.Distance(_enemy.transform.position, _player.position);

        public EnemyState(SimpleEnemy enemy, Transform player)
        {
            _enemy = enemy;
            _player = player;

            Name = _enemy.gameObject.name + _globalId++;
        }

        protected void FlipSprite(bool faceRight = true)
        {
            //if (faceRight && _enemy.transform.localScale.x < 0 || !faceRight && _enemy.transform.localScale.x > 0)
            //    _enemy.transform.localScale = new Vector3(-_enemy.transform.localScale.x, _enemy.transform.localScale.y, _enemy.transform.localScale.z);

            // DOTween the scale so it flips smoothly
            var newScale = new Vector3(faceRight ? 1 : -1, _enemy.transform.localScale.y, _enemy.transform.localScale.z);
            _enemy.transform.DOScale(newScale, .2f);

        }

    }
}
