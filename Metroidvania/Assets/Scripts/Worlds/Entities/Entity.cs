using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entity class
public class Entity : MonoBehaviour
{
    // Unity Components
    public BoxCollider2D feetBox;
    public BoxCollider2D headBox;
    public BoxCollider2D bodyBox;
    public CompositeCollider2D compCol;
    protected Rigidbody2D rigid { get; private set; }

    // Entity Physics
    public float width
    {
        get
        {
            float minfx = feetBox.bounds.min.x;
            float minhx = headBox.bounds.min.x;
            float maxfx = feetBox.bounds.max.x;
            float maxhx = headBox.bounds.max.x;

            float min = minfx < minhx ? minfx : minhx;
            float max = maxfx > maxhx ? maxfx : maxhx;

            return max - min;
        }
    }

    public float height
    {
        get
        {
            float minfy = feetBox.bounds.min.y;
            float maxhy = headBox.bounds.max.y;

            return maxhy - minfy;
        }
    }

    // Change Velocity
    protected Vector2 currentVelocity => curVelocity;
    private Vector2 curVelocity;
    private Vector2 tempVelocity;

    protected void SetVelocityX(float x)
    {
        tempVelocity.Set(x, curVelocity.y);
        SetVelocityFinal();
    }

    protected void SetVelocityY(float y)
    {
        tempVelocity.Set(curVelocity.x, y);
        SetVelocityFinal();
    }

    protected void SetVelocityXY(float x, float y)
    {
        tempVelocity.Set(x, y);
        SetVelocityFinal();
    }

    protected void SetVelocityFinal()
    {
        curVelocity = tempVelocity;
        rigid.velocity = tempVelocity;
    }

    // Change Gravity
    protected void EnableGravity()
    {
        rigid.gravityScale = 1.0f;
    }

    protected void DisableGravity()
    {
        rigid.gravityScale = 0.0f;
    }

    // Terrain Checker
    protected void CheckBasicGround(out RaycastHit2D ground, out bool detected, Vector2 origin, float detectLength)
    {
        int layer = LayerInfo.groundMask;

        ground = Physics2D.Raycast(origin, Vector2.down, detectLength, layer);
        detected = ground;
    }

    protected void CheckSemiGround(out RaycastHit2D ground, out bool detected, Vector2 origin, float detectLength)
    {
        int layer = LayerInfo.semiGroundMask;

        ground = Physics2D.Raycast(origin, Vector2.down, detectLength, layer);
        detected = ground;
    }

    protected void CheckGroundAll(out RaycastHit2D ground, out bool detected, Vector2 origin, float detectLength)
    {
        int layer = LayerInfo.groundMask | LayerInfo.semiGroundMask;

        ground = Physics2D.Raycast(origin, Vector2.down, detectLength, layer);
        detected = ground;
    }

    protected void CheckCeil(out RaycastHit2D ceil, out bool detected, Vector2 origin, float detectLength)
    {
        int layer = LayerInfo.groundMask;

        ceil = Physics2D.Raycast(origin, Vector2.up, detectLength, layer);
        detected = ceil;
    }

    protected void CheckWall(out RaycastHit2D wall, out int detected, Vector2 origin, float detectLength, int lookingDirection)
    {
        int layer = LayerInfo.groundMask;

        wall = Physics2D.Raycast(origin, Vector2.right * lookingDirection, detectLength, layer);
        detected = wall ? lookingDirection : 0;
    }

    // Change Collision Ignorance
    protected void AcceptCollision(Collider2D collider)
    {
        Physics2D.IgnoreCollision(feetBox, collider, false);
        Physics2D.IgnoreCollision(headBox, collider, false);
        Physics2D.IgnoreCollision(bodyBox, collider, false);
        Physics2D.IgnoreCollision(compCol, collider, false);
    }

    protected void IgnoreCollision(Collider2D collider)
    {
        Physics2D.IgnoreCollision(feetBox, collider, true);
        Physics2D.IgnoreCollision(headBox, collider, true);
        Physics2D.IgnoreCollision(bodyBox, collider, true);
        Physics2D.IgnoreCollision(compCol, collider, true);
    }

    protected bool CanCollision(Collider2D collider)
    {
        return !Physics2D.GetIgnoreCollision(feetBox, collider);
    }

    // Unity Event Functions
    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        tempVelocity = Vector2.zero;
        curVelocity = Vector2.zero;
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        curVelocity = rigid.velocity;
    }

    // Entity Base Logics
    protected void Logic_MoveOnGround(Vector2 moveDirection, float vx, int lookingDirection)
    {
        float wx = 1.0f;
        float wy = moveDirection.y / moveDirection.x;

        SetVelocityXY(vx * wx * lookingDirection, vx * wy * lookingDirection);
    }

    protected void Logic_MoveOnAirX(float vx, int lookingDirection)
    {
        SetVelocityX(vx * lookingDirection);
    }

    protected void Logic_MoveOnAirY(float vy)
    {
        SetVelocityY(vy);
    }

    protected void Logic_MoveOnAirXY(float vx, float vy, int lookingDirection)
    {
        SetVelocityXY(vx * lookingDirection, vy);
    }
}


/*
#region Player State Constants
    private const int stIdleOnGround = 0;
    private const int stIdleLongOnGround = 1;
    private const int stSit = 2;
    private const int stHeadUp = 3;
    private const int stWalk = 4;
    private const int stRun = 5;
    private const int stFreeFall = 6;
    private const int stGliding = 7;
    private const int stIdleWall = 8;
    private const int stWallSliding = 9;
    private const int stLedgeClimb = 10;
    private const int stJumpOnGround = 11;
    private const int stJumpDown = 12;
    private const int stRoll = 13;
    private const int stJumpOnAir = 14;
    private const int stDash = 15;
    private const int stTakeDown = 16;
    private const int stJumpOnWall = 17;
    #endregion
*/