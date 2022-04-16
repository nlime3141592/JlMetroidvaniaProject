using System;

using UnityEngine;

namespace JlMetroidvaniaProject.Datas
{
    public class PlayerState : StateBase
    {
        // sit
        public bool canSit;
        public bool isSit;

        // head up
        public bool canHeadUp;
        public bool isHeadUp;

        // fall down
        public float MIN_FALLING_SPEED => m_initializer.MIN_FALLING_SPEED;
        public float MAX_FALLING_SPEED => m_initializer.MAX_FALLING_SPEED;
        public bool canFalldown;

        // move
        public float RUN_SPEED_COEFFICIENT => m_initializer.RUN_SPEED_COEFFICIENT;
        public bool canMove;
        public float moveSpeed;
        public float moveDirection;

        // pace
        public int paceDirection;

        // jump
        public int MAX_JUMP_FRAME => m_initializer.MAX_JUMP_FRAME;
        public int MAX_JUMP_TIME => m_initializer.MAX_JUMP_TIME;
        public bool canJump;
        public float jumpSpeed;
        public int currentJumpedFrame;
        public int currentJumpedTime;

        // wall sticking
        public bool isWallSticking;
        public float wallStickingDirection;

        // wall sliding
        public float wallSlidingSpeed;

        // wall jump
        public int MAX_WALL_JUMP_FRAME => m_initializer.MAX_WALL_JUMP_FRAME;
        public bool canWallJump;
        public float wallJumpSpeedX;
        public float wallJumpSpeedY;
        public int currentWallJumpedFrame;

        // dash
        public int MAX_DASH_FRAME => m_initializer.MAX_DASH_FRAME;
        public int MAX_DASH_TIME => m_initializer.MAX_DASH_TIME;
        public bool canDash;
        public float dashSpeed;
        public int currentDashFrame;
        public int currentDashTime;

        // gliding
        public bool isGliding;
        public float glidingSpeed;

        //////////////////////////////////////// OPTION
        private PlayerStateInitializer m_initializer;

        public PlayerState(PlayerStateInitializer initializer)
        {
            m_initializer = initializer;

            canSit = true;
            canHeadUp = true;
            canFalldown = true;
            canMove = true;
            canJump = true;
            canWallJump = true;
            isWallSticking = false;
            canDash = true;

            Update();
        }

        public override void Update()
        {
            // fall down

            // move
            moveSpeed = m_initializer.moveSpeed;

            // jump
            jumpSpeed = m_initializer.jumpSpeed;

            // wall sliding
            wallSlidingSpeed = m_initializer.wallSlidingSpeed;

            // wall jump
            wallJumpSpeedX = m_initializer.wallJumpSpeedX;
            wallJumpSpeedY = m_initializer.wallJumpSpeedY;

            // dash
            dashSpeed = m_initializer.dashSpeed;

            // gliding
            glidingSpeed = m_initializer.glidingSpeed;
        }
    }
}