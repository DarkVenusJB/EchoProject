using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameData.GameSOData;
using Services.InputService;
using UnityEngine;
using Zenject;

namespace GameEntities.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerMovementSOData _playerMovementConfig;
        
        private Rigidbody2D _rb;
        private bool _isGrounded;
        private bool _isWallSliding;
        private bool _isDashing;
        private bool _canDash;
        private int _jumpCount;
        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;
        private CancellationTokenSource _jumpCts;

        [Inject] private InputService _inputService;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _canDash = true;
        }

        private void Update()
        {
            CheckCollisions();
            HandleJumpBuffer();
            HandleCoyoteTime();
            HandleJumping();
            HandleWallSliding();
            HandleDashing();
        }

        private void FixedUpdate()
        {
            if (!_isDashing)
            {
                HandleMovement();
            }
        }

        private void CheckCollisions()
        {
            _isGrounded = Physics2D.OverlapCircle(transform.position, _playerMovementConfig.checkRadius, LayerMask.GetMask("Ground"));
            _isWallSliding = !_isGrounded && Physics2D.OverlapCircle(transform.position + Vector3.right * 0.5f,
                _playerMovementConfig.checkRadius, LayerMask.GetMask("Ground"));

            if (_isGrounded)
            {
                _jumpCount = _playerMovementConfig.extraJumps;
                _canDash = true;
            }
        }

        private void HandleMovement()
        {
            float moveInput = _inputService.MoveInput;
            float targetSpeed = moveInput * _playerMovementConfig.moveSpeed;
            float speedDifference = targetSpeed - _rb.velocity.x;
            float accelerationRate = Mathf.Abs(targetSpeed) > 0.1f ? _playerMovementConfig.acceleration : _playerMovementConfig.deceleration;
            float movement = speedDifference * accelerationRate * Time.fixedDeltaTime;

            _rb.velocity = new Vector2(_rb.velocity.x + movement, _rb.velocity.y);
        }

        private void HandleJumpBuffer()
        {
            if (_inputService.JumpInput)
                _jumpBufferCounter = _playerMovementConfig.jumpBufferTime;
            else
                _jumpBufferCounter -= Time.deltaTime;
        }

        private void HandleCoyoteTime()
        {
            if (_isGrounded)
                _coyoteTimeCounter = _playerMovementConfig.coyoteTime;
            else
                _coyoteTimeCounter -= Time.deltaTime;
        }

        private void HandleJumping()
        {
            if (_jumpBufferCounter > 0 && (_coyoteTimeCounter > 0 || _jumpCount > 0))
            {
                _jumpCts?.Cancel(); // Отменяем предыдущий прыжок, если он был
                _jumpCts = new CancellationTokenSource();
                JumpAnimation(_jumpCts.Token).Forget();
                _jumpBufferCounter = 0;
                _coyoteTimeCounter = 0;
                _jumpCount--;
            }
        }

        private async UniTaskVoid JumpAnimation(CancellationToken token)
        {
            float elapsedTime = 0;

            while (elapsedTime < _playerMovementConfig.jumpDuration)
            {
                if (token.IsCancellationRequested) return;

                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _playerMovementConfig.jumpDuration;
                float curveValue = _playerMovementConfig.jumpCurve.Evaluate(t);

                _rb.velocity = new Vector2(_rb.velocity.x, curveValue * _playerMovementConfig.jumpForce);
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            }
        }

        private void HandleWallSliding()
        {
            if (_isWallSliding && _rb.velocity.y < 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_playerMovementConfig.wallSlideSpeed);
            }

            if (_isWallSliding && _inputService.JumpInput)
            {
                _rb.velocity = new Vector2(-_inputService.MoveInput * _playerMovementConfig.wallJumpDirection, _playerMovementConfig.wallJumpForce);
            }
        }

        private void HandleDashing()
        {
            if (_inputService.DashInput && _canDash)
            {
                Dash().Forget();
            }
        }

        private async UniTaskVoid Dash()
        {
            _isDashing = true;
            _canDash = false;
            _rb.velocity = new Vector2(_inputService.MoveInput * _playerMovementConfig.dashSpeed, 0);
            await UniTask.Delay((int)(_playerMovementConfig.dashDuration * 1000));
            _isDashing = false;
            await UniTask.Delay((int)(_playerMovementConfig.dashCooldown * 1000));
            _canDash = true;
        }
    }
}