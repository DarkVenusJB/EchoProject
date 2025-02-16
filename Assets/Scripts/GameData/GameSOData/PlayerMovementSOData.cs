using UnityEngine;

namespace GameData.GameSOData
{
    [CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Configs/Player/Player Movement Config")]
    public class PlayerMovementSOData : ScriptableObject
    {
        [Header("Movement Settings")]
        public float moveSpeed = 7f;
        public float acceleration = 10f;
        public float deceleration = 10f;

        [Header("Jump Settings")]
        public AnimationCurve jumpCurve;
        public float jumpForce = 12f;
        public float jumpDuration = 0.5f;
        public int extraJumps = 1;
        public float coyoteTime = 0.15f;
        public float jumpBufferTime = 0.2f;

        [Header("Wall Slide Settings")]
        public float wallSlideSpeed = 2f;
        public float wallJumpForce = 10f;
        public float wallJumpDirection = 1.2f;

        [Header("Dash Settings")]
        public float dashSpeed = 20f;
        public float dashDuration = 0.2f;
        public float dashCooldown = 1f;

        [Header("Collision Settings")]
        public float checkRadius = 0.2f;
    }
}