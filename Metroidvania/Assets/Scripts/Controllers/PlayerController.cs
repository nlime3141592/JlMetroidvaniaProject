using UnityEngine;

using JlMetroidvaniaProject.Datas;
using JlMetroidvaniaProject.Utility;
using JlMetroidvaniaProject.Physics;

namespace JlMetroidvaniaProject.Controllers
{
    public class PlayerController : JlBehaviour
    {
        private Rigidbody2D m_rigidbody;

        private InputSystem m_inputSystem;
        private TerrainCollisionState m_collisionState;
        private PlayerState m_playerState;
        private VelocityState m_velocityState;

        [Header("Component State Initializer")]
        public TerrainCollisionStateInitializer terrainCollisionStateInitializer;
        public PlayerStateInitializer playerStateInitializer;
        public VelocityStateInitializer velocityStateInitializer;

        [Header("Other Attaches")]
        public GameObject playerRenderer;

        protected override void Awake()
        {
            // component allocation.
            m_rigidbody = GetComponent<Rigidbody2D>();

            // force allocation in initializer.
            velocityStateInitializer.rigidbody = m_rigidbody;

            // internal class instantiation.
            m_collisionState = new TerrainCollisionState(terrainCollisionStateInitializer);
            m_playerState = new PlayerState(playerStateInitializer);
            m_velocityState = new VelocityState(velocityStateInitializer);
        }

        protected override void Start()
        {
            m_inputSystem = GameManager.s_inputSystem;
        }

        protected override void FixedUpdate()
        {
            // 물리 요소 갱신
            m_velocityState.Update();
            m_collisionState.Update();

            // 실제 기능을 수행
            m_DoAction_Sit();
            m_DoAction_HeadUp();
            m_DoAction_Pace();
            m_DoAction_FallDown();
            m_DoAction_Gliding();
            m_DoAction_Move();
            m_DoAction_Jump();
            m_DoAction_CheckJumpCount();
            m_DoAction_WallSliding();
            m_DoAction_WallJump();
            m_DoAction_Dash();
        }

        protected override void Update()
        {
            m_playerState.Update();

            m_UpdateSit();
            m_UpdateHeadUp();
            m_UpdateGliding();
            m_UpdateMove();
            m_UpdateJump();
            m_UpdatePace();
            m_UpdateWallSticking();
            m_UpdateWallJump();
            m_UpdateDash();

            // data transfer
            StatePrinter.s_isSit = m_playerState.isSit;
            StatePrinter.s_isHeadUp = m_playerState.isHeadUp;
        }

        public static bool GetPlayerControllerByTrigger2D(Collider2D collider, out PlayerController controller)
        {
            return collider.gameObject.transform.parent.TryGetComponent<PlayerController>(out controller);
        }

        public void OnLatencyArea(float moveDirection)
        {
            m_playerState.moveDirection = moveDirection;
        }

        private void m_UpdateSit()
        {
            if(!m_collisionState.isOnGround) return;

            if(m_playerState.canSit && m_inputSystem[KeyType.Sit, PressType.Down])
            {
                m_playerState.canMove = false;
                m_playerState.canJump = false;
                m_playerState.canHeadUp = false;
                m_playerState.isSit = true;
            }
            else if(m_playerState.isSit && m_inputSystem[KeyType.Sit, PressType.Up])
            {
                m_playerState.canMove = true;
                m_playerState.canJump = true;
                m_playerState.canHeadUp = true;
                m_playerState.isSit = false;
            }
        }

        private void m_UpdateHeadUp()
        {
            if(!m_collisionState.isOnGround) return;

            if(m_playerState.canHeadUp && m_inputSystem[KeyType.HeadUp, PressType.Down])
            {
                m_playerState.canMove = false;
                m_playerState.canJump = false;
                m_playerState.canSit = false;
                m_playerState.isHeadUp = true;
            }
            else if(m_playerState.isHeadUp && m_inputSystem[KeyType.HeadUp, PressType.Up])
            {
                m_playerState.canMove = true;
                m_playerState.canJump = true;
                m_playerState.canSit = true;
                m_playerState.isHeadUp = false;
            }
        }

        private void m_UpdateGliding()
        {
            if(!m_playerState.isGliding && m_playerState.isWallSticking) return;
            if(!m_playerState.isGliding && m_velocityState.Y > Constant.c_INFINITE_MIN_NEGATIVE) return;

            if(m_playerState.isGliding && m_collisionState.isOnGround)
            {
                m_playerState.isGliding = false;

                m_playerState.canFalldown = true;
                m_playerState.canJump = true;
                m_playerState.canWallJump = true;
                m_playerState.canDash = true;
                return;
            }

            if(m_inputSystem[KeyType.Gliding, PressType.Pressing])
            {
                m_playerState.isGliding = true;

                m_playerState.canFalldown = false;
                m_playerState.canJump = false;
                m_playerState.canWallJump = false;
                m_playerState.canDash = false;
            }
            if(m_inputSystem[KeyType.Gliding, PressType.Up])
            {
                m_playerState.isGliding = false;

                m_playerState.canFalldown = true;
                m_playerState.canJump = true;
                m_playerState.canWallJump = true;
                m_playerState.canDash = true;
            }
        }

        private void m_UpdateMove()
        {
            if(!m_playerState.canMove)
            {
                m_playerState.moveDirection = 0.0f;
                return;
            }

            int negative = m_inputSystem[KeyType.MoveL, PressType.Pressing] ? -1 : 0;
            int positive = m_inputSystem[KeyType.MoveR, PressType.Pressing] ? 1 : 0;
            int direction = negative + positive;
            m_playerState.moveDirection = (float)direction;
        }

        private void m_UpdateJump()
        {
            if(!m_playerState.canJump) return;
            if(m_playerState.wallStickingDirection != 0) return;

            if(m_playerState.currentJumpedTime < m_playerState.MAX_JUMP_TIME)
            {
                if(m_inputSystem[KeyType.Jump, PressType.Up])
                {
                    // 점프 최고점 찍을 때 까지 스페이스바를 떼지 않음.
                    if(m_playerState.currentJumpedFrame > m_playerState.MAX_JUMP_FRAME)
                    {
                        m_playerState.currentJumpedFrame = 0;
                    }
                    // 점프 최고점 찍기 전에 스페이스바를 뗌.
                    else
                    {
                        m_playerState.currentJumpedFrame = -1;
                    }
                }
                if(m_inputSystem[KeyType.Jump, PressType.Down])
                {
                    m_playerState.currentJumpedFrame = 1;
                    m_playerState.currentJumpedTime++;
                }
            }
        }

        private void m_UpdatePace()
        {
            if(m_playerState.paceDirection == 0)
            {
                m_playerState.paceDirection = 1;
            }
            else if(m_velocityState.X < 0.0f && m_playerState.paceDirection > 0)
            {
                m_PaceLeft();
            }
            else if(m_velocityState.X > 0.0f && m_playerState.paceDirection < 0)
            {
                m_PaceRight();
            }
        }

        private void m_UpdateWallSticking()
        {
            int isLeft = m_collisionState.isLeftWallSticking ? -1 : 0;
            int isRight = m_collisionState.isRightWallSticking ? 1 : 0;
            float direction = (float)(isLeft + isRight);

            if(direction != 0)
            {
                m_playerState.wallStickingDirection = direction;

                if(
                    (direction < 0.0f && m_inputSystem[KeyType.MoveL, PressType.Pressing]) ||
                    (direction > 0.0f && m_inputSystem[KeyType.MoveR, PressType.Pressing])
                )
                {
                    m_playerState.isWallSticking = true;
                }
                else
                {
                    m_playerState.isWallSticking = false;
                }
            }
            else
            {
                m_playerState.wallStickingDirection = 0.0f;
                m_playerState.isWallSticking = false;
            }
        }

        private void m_UpdateWallJump()
        {
            if(!m_playerState.canWallJump) return;
            if(m_playerState.currentWallJumpedFrame != 0) return;

            if((m_collisionState.isLeftWallSticking || m_collisionState.isRightWallSticking) && m_velocityState.Y < Constant.c_INFINITE_MIN_POSITIVE && m_inputSystem[KeyType.Jump, PressType.Down])
            {
                m_playerState.canWallJump = false;

                m_playerState.canSit = false;
                m_playerState.canHeadUp = false;
                m_playerState.canMove = false;
                m_playerState.canJump = false;
                m_playerState.canDash = false;

                m_playerState.currentWallJumpedFrame = 1;
            }
        }

        private void m_UpdateDash()
        {
            if(!m_playerState.canDash) return;
            if(m_playerState.currentDashFrame != 0) return;
            if(m_collisionState.isOnGround) return;
            if(m_playerState.currentDashTime >= m_playerState.MAX_DASH_TIME) return;

            if(m_inputSystem[KeyType.Dash, PressType.Down])
            {
                m_playerState.canFalldown = false;
                m_playerState.canMove = false;
                m_playerState.canJump = false;
                m_playerState.canDash = false;
                m_rigidbody.gravityScale = 0.0f;
                m_playerState.currentDashTime++;

                m_CancelJump();

                m_StartDash();
            }
        }

        private void m_DoAction_Sit()
        {
            if(m_playerState.isSit)
            {
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, 0.0f, AxisType.X);
            }
        }

        private void m_DoAction_HeadUp()
        {
            if(m_playerState.isHeadUp)
            {
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, 0.0f, AxisType.X);
            }
        }

        private void m_DoAction_FallDown()
        {
            if(!m_playerState.canFalldown) return;
            if(m_playerState.isGliding) return;
            if(m_playerState.isWallSticking) return;
            if(m_collisionState.isLeftWallSticking || m_collisionState.isRightWallSticking) return;

            if(!m_collisionState.isOnGround && m_velocityState.Y < -m_playerState.MAX_FALLING_SPEED)
            {
                float finalVelocity = -m_playerState.MAX_FALLING_SPEED;
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, finalVelocity, AxisType.Y);
            }
        }

        private void m_DoAction_Gliding()
        {
            if(!m_playerState.isGliding) return;
            if(m_playerState.canFalldown) return;

            float finalVelocity = m_playerState.glidingSpeed * -1;
            MetroidPhysics.DoUniformMotion2D(m_rigidbody, finalVelocity, AxisType.Y);
        }

        private void m_DoAction_Move()
        {
            if(!m_playerState.canMove) return;

            float finalVelocity = m_playerState.moveSpeed * m_playerState.moveDirection;
            MetroidPhysics.DoUniformMotion2D(m_rigidbody, finalVelocity, AxisType.X);
        }

        private void m_DoAction_Jump()
        {
            // NOTE: Do jump.
            if(m_playerState.currentJumpedFrame > 0 && m_playerState.currentJumpedFrame <= m_playerState.MAX_JUMP_FRAME) // 0.26 seconds
            {
                float finalVelocity = m_playerState.jumpSpeed;
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, finalVelocity, AxisType.Y);
                m_playerState.currentJumpedFrame++;
            }

            // NOTE: Cancel jump.
            else if(m_playerState.currentJumpedFrame == -1)
            {
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, 0.0f, AxisType.Y);
                m_playerState.currentJumpedFrame = 0;
            }
        }

        private void m_DoAction_CheckJumpCount()
        {
            if(m_collisionState.isOnGround && m_velocityState.Y < Constant.c_INFINITE_MIN_POSITIVE && m_playerState.currentJumpedTime > 0)
            {
                m_playerState.currentJumpedTime = 0;
            }
        }

        private void m_DoAction_Pace()
        {
            int angle = 90 * (1 - m_playerState.paceDirection);

            playerRenderer.transform.eulerAngles = Vector3.up * (float)angle;
        }

        private void m_DoAction_WallSliding()
        {
            if(m_playerState.isGliding) return;
            if(m_playerState.isWallSticking) return;
            if(m_velocityState.Y > Constant.c_INFINITE_MIN_NEGATIVE) return;

            if(m_collisionState.isLeftWallSticking || m_collisionState.isRightWallSticking)
            {
                float finalVelocity = m_playerState.wallSlidingSpeed;
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, -finalVelocity, AxisType.Y);
            }
        }

        private void m_DoAction_WallJump()
        {
            if(m_playerState.currentWallJumpedFrame == 1)
            {
                float finalVelocityX = m_playerState.wallJumpSpeedX * (m_playerState.wallStickingDirection) * -1;
                float finalVelocityY = m_playerState.wallJumpSpeedY;

                MetroidPhysics.DoUniformMotion2D(m_rigidbody, finalVelocityX, AxisType.X);
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, finalVelocityY, AxisType.Y);

                m_playerState.currentWallJumpedFrame++;
            }
            else if(m_playerState.currentWallJumpedFrame > 1 && m_playerState.currentWallJumpedFrame <= m_playerState.MAX_WALL_JUMP_FRAME)
            {
                m_playerState.currentWallJumpedFrame++;
            }
            else if(m_playerState.currentWallJumpedFrame > m_playerState.MAX_WALL_JUMP_FRAME)
            {
                m_playerState.canSit = true;
                m_playerState.canHeadUp = true;
                m_playerState.canMove = true;
                m_playerState.canJump = true;
                m_playerState.canDash = true;

                m_playerState.currentWallJumpedFrame = 0;
                m_playerState.canWallJump = true;
            }
            else if(m_playerState.currentWallJumpedFrame == -1)
            {
                m_playerState.canSit = true;
                m_playerState.canHeadUp = true;
                m_playerState.canMove = true;
                m_playerState.canJump = true;
                m_playerState.canDash = true;

                m_playerState.currentWallJumpedFrame = 0;
                m_playerState.canWallJump = true;
            }
        }

        private void m_DoAction_Dash()
        {
            if(m_playerState.currentDashFrame == 1)
            {
                float finalVelocity = m_playerState.dashSpeed * (float)m_playerState.paceDirection;
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, finalVelocity, AxisType.X);
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, 0.0f, AxisType.Y);
                m_AddDashFrame();
            }
            else if(m_playerState.currentDashFrame > 1 && m_playerState.currentDashFrame <= m_playerState.MAX_DASH_FRAME)
            {
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, 0.0f, AxisType.Y);
                m_AddDashFrame();
            }
            else if(m_playerState.currentDashFrame > m_playerState.MAX_DASH_FRAME)
            {
                m_playerState.canFalldown = true;
                m_playerState.canMove = true;
                m_playerState.canJump = true;
                m_playerState.canDash = true;
                m_rigidbody.gravityScale = 5.0f;
                m_EndDash();
            }
            else if(m_playerState.currentDashFrame == -1)
            {
                m_playerState.canFalldown = true;
                m_playerState.canMove = true;
                m_playerState.canJump = true;
                m_playerState.canDash = true;
                m_ClearDashFrame();
            }
            else if(m_playerState.currentDashTime > 0 && m_collisionState.isOnGround)
            {
                m_playerState.currentDashTime = 0;
            }
        }

        private void m_StartJump() => m_playerState.currentJumpedFrame = 1;
        private void m_EndJump() => m_playerState.currentJumpedFrame = 0;
        private void m_CancelJump() => m_playerState.currentJumpedFrame = -1;
        private void m_AddJumpFrame() => m_playerState.currentJumpedFrame++;
        private void m_ClearJumpFrame() => m_playerState.currentJumpedFrame = 0;

        private void m_AddJumpTime() => m_playerState.currentJumpedTime++;
        private void m_ClearJumpTime() => m_playerState.currentJumpedTime = 0;

        private void m_PaceLeft() => m_playerState.paceDirection = -1;
        private void m_PaceRight() => m_playerState.paceDirection = 1;

        private void m_StartDash() => m_playerState.currentDashFrame = 1;
        private void m_EndDash() => m_playerState.currentDashFrame = 0;
        private void m_CancelDash() => m_playerState.currentDashFrame = -1;
        private void m_AddDashFrame() => m_playerState.currentDashFrame++;
        private void m_ClearDashFrame() => m_playerState.currentDashFrame = 0;
    }
}