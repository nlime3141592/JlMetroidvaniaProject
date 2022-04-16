using System;

namespace JlMetroidvaniaProject.Datas
{
    [Serializable]
    public class PlayerStateInitializer : IStateInitializer
    {
        // fall down
        public float MIN_FALLING_SPEED;
        public float MAX_FALLING_SPEED;

        // move
        public float RUN_SPEED_COEFFICIENT;
        public float moveSpeed;

        // pace

        // jump
        public int MAX_JUMP_FRAME;
        public int MAX_JUMP_TIME;
        public float jumpSpeed;

        // wall holding

        // wall sliding
        public float wallSlidingSpeed;

        // wall jump
        public int MAX_WALL_JUMP_FRAME;
        public float wallJumpSpeedX;
        public float wallJumpSpeedY;

        // dash
        public int MAX_DASH_FRAME;
        public int MAX_DASH_TIME;
        public float dashSpeed;

        // gliding
        public float glidingSpeed;
    }
}