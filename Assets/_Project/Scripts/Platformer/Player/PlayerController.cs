using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Reference")]
        [SerializeField] private InputReader _playerInput;

        [Header("Move info")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        private Rigidbody2D rb;
        private float _hInput;
        private float _vInput;
        private bool _readyToLand;
        public float accelerationTime = 0.1f; // Time taken to reach max speed
        public float decelerationTime = 0.2f; // Time taken to stop from max speed
        private Animator _animator;
        private float currentVelocityX = 0f;
        private float targetVelocityX = 0f;
        [Header("Move info")]
        public float jumpStartTime = 2f;
        private float jumpTime;
        private bool isJumping = false;
        
        [Header("Collision info")]

        //ground and wall detection stuffs
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private float _groundCheckDistance;
        private bool _isGrounded;
        private bool _canDoubleJump = true;

        private bool _facingRight = true;
        private int _facingDirection = 1;

        public float _wallCheckDistance;
        private bool _isWallDetected;


        // Bufferjump and cayotejump
        [Header("Buffer and Cayote info")]

        [SerializeField] private float _bufferJumpTime;
        private float _bufferJumpCounter;
        [SerializeField] private float _cayoteJumpTime;
        private float _cayoteJumpCounter;
        private bool _canHaveCayoteJump;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _playerInput.Move += direction =>
            {
                _hInput = direction.x;
                _vInput = direction.y;
            };

            _playerInput.Jump += () =>
            {
                JumpButton();
            };
        }

        // Update is called once per frame
        void Update()
        {
            FlipController();
            CollisionCheck();
            AnimController();
            _bufferJumpCounter -= Time.deltaTime;
            _cayoteJumpCounter -= Time.deltaTime;

            if (_isGrounded)
            {

                if (_bufferJumpCounter > 0)
                {
                    _bufferJumpCounter = -1;
                    Jump();
                }
                _canHaveCayoteJump = true;

                if (_readyToLand)
                {
                    _readyToLand = false;
                }
            }
            else
            {
                if (!_readyToLand)
                {
                    _readyToLand = true;
                }

                if (_canHaveCayoteJump)
                {
                    _canHaveCayoteJump = false;
                    _cayoteJumpCounter = _cayoteJumpTime;
                }
            }

            Move();
        }

        /*
         * Move Function
         */
        public void Move()
        {
            targetVelocityX = _hInput * _moveSpeed;
            currentVelocityX = Mathf.Lerp(currentVelocityX, targetVelocityX,
                                Time.deltaTime * (_hInput == 0 ? 1 / decelerationTime : 1 / accelerationTime));
            rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);
        }

        /*
         * Jump function
         */
        public void Jump()
        {
            isJumping = true;
            jumpTime = jumpStartTime;
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && _isGrounded)  
            {
                isJumping = true;
                jumpTime = jumpStartTime;
                rb.velocity = Vector2.up * _jumpForce;
            }
            if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isJumping)
            { 
                if(jumpTime > 0)
                {
                    rb.velocity = Vector2.up * _jumpForce;
                    jumpTime -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
                
            }
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                isJumping = false;
            }
             
        }

        /*
         *  Flip Player function 
         */
        private void Flip()
        {
            _facingDirection = _facingDirection * -1;
            _facingRight = !_facingRight;
            transform.Rotate(0, 180, 0);
        }

        private void FlipController()
        {
            if (_facingRight && ((int)rb.velocity.x) < 0)
            {
                Flip();
            }
            else if (!_facingRight && ((int)rb.velocity.x) > 0)
            {
                Flip();
            }
        }

        /*
         * Jump button function
         */
        public void JumpButton()
        {

            if (!_isGrounded)
            {
                _bufferJumpCounter = _bufferJumpTime;
            }

            else if (_isGrounded || _cayoteJumpCounter > 0)
            {
                Jump();
            }

        }

        /*
         * Collision check function
         *
         */
        private void CollisionCheck()
        {
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _whatIsGround);

            //TODO
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - _groundCheckDistance));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (_wallCheckDistance * _facingDirection), transform.position.y));

        }

        public void TakeDamage()
        {
            // TODO : Call the animation, shake screen etc...
            gameObject.SetActive(false);
        }

        /*
         * Function for Animation controller
         *
         */
        public void AnimController()
        {
            _animator.SetBool("isMoving", (int)rb.velocity.x != 0);
            _animator.SetBool("isGrounded", _isGrounded);
        }
    }

}
