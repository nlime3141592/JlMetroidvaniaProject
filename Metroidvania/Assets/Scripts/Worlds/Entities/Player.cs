using System;
using System.Threading;
using UnityEngine;

// 최적화 코드
public class Player : Entity
{
    #region Components
    public SpriteRenderer spRenderer;
    public Animator animator;
    public ElongatedHexagonCollider2D hexaCol;
    private Vector2 hexaColOffset;
    #endregion

    #region Player State Constants
    private const int stIdleGround = 0;
    private const int stIdleGroundLong = 1;
    private const int stSit = 2;
    private const int stHeadUp = 3;
    private const int stWalk = 4;
    private const int stRun = 5;
    private const int stFreeFall = 6;
    private const int stGliding = 7;
    private const int stIdleWall = 8;
    private const int stWallSliding = 9;
    private const int stLedgeClimbHead = 10;
    private const int stLedgeClimbBody = 11;
    private const int stJumpGround = 12;
    private const int stJumpDown = 13;
    private const int stRoll = 14;
    private const int stJumpAir = 15;
    private const int stDash = 16;
    private const int stTakeDown = 17;
    private const int stJumpWall = 18;
    #endregion

    // NOTE: 플레이어 초기 위치, 테스트를 위한 벡터 변수, 릴리즈 시 제거해야 함.
    private Vector2 initialPosition;

    // 충돌 판정의 특이점들 (singular points)
    protected Vector2 dir_hPos;
    protected Vector2 dir_hlPos;
    protected Vector2 dir_hrPos;
    protected Vector2 dir_fPos;
    protected Vector2 dir_flPos;
    protected Vector2 dir_frPos;
    protected Vector2 dir_cPos;
    protected Vector2 dir_clPos;
    protected Vector2 dir_crPos;
    protected Vector2 dir_ltPos;
    protected Vector2 dir_lbPos;
    protected Vector2 dir_rtPos;
    protected Vector2 dir_rbPos;

    protected Vector2 hPos; // Head Position
    protected Vector2 hlPos; // Head Left Position
    protected Vector2 hrPos; // Head Right Position
    protected Vector2 fPos; // Feet Position
    protected Vector2 flPos; // Feet Left Position
    protected Vector2 frPos; // Feet Right Position
    protected Vector2 cPos; // Center Position (Body Position)
    protected Vector2 clPos;
    protected Vector2 crPos;
    protected Vector2 ltPos; // Left Top Position
    protected Vector2 lbPos; // Left Bottom Position
    protected Vector2 rtPos; // Right Top Position
    protected Vector2 rbPos; // Right Bottom Position

    // Check Ground
    protected bool canCheckBasicGround = true;
    protected bool canCheckSemiGround = true;
    protected RaycastHit2D detectedGround;
    public bool isDetectedGround;
    public bool isHitGround;

    // Check Ceil
    protected bool canCheckCeil = true;
    protected RaycastHit2D detectedCeil;
    public bool isDetectedCeil;
    public bool isHitCeil;

    // Check Wall
    protected bool canCheckWallT = true;
    protected bool canCheckWallB = true;
    protected RaycastHit2D detectedWallT;
    protected RaycastHit2D detectedWallB;
    public int isHitWallT;
    public int isHitWallB;

    // Check Head-Over Semi Ground
    protected bool canCheckHeadOverSemiGround = true;
    protected RaycastHit2D headOverSemiGroundBefore;
    protected RaycastHit2D headOverSemiGroundCurrent;
    private RaycastHit2D[] headOverSemiGroundBefores;
    private RaycastHit2D[] headOverSemiGroundCurrents;

    // Check Ledge
    protected bool canCheckLedge = true;
    protected RaycastHit2D detectedLedge;
    public float ledgeCheckingWidth = 0.04f;
    public float ledgeCheckingHeight = 0.2f;
    protected Vector2 ledgeCornerTopPos;
    protected Vector2 ledgeCornerSidePos;
    public bool isHitLedgeHead;
    public bool isHitLedgeBody;

    // Check Ledge Hanging
    public bool canCheckHangingBasic = true;
    public bool canCheckHangingSemi = true;
    protected RaycastHit2D leftHangingGround;
    protected RaycastHit2D rightHangingGround;
    public bool isHangingGround;

    // Input Handling
    private InputData inputData;
    private InputData preInputData;
    private uint preInputPressing = 0;
    private uint preInputDown = 0;

    // 플레이어 이동 관련 속성
    private bool canUpdateLookDir = true;
    private int lookDir;
    public bool isRun;
    protected float vx; // NOTE: 값 임시 저장을 위한 변수
    protected float vy; // NOTE: 값 임시 저장을 위한 변수
    protected Vector2 moveDir;

    #region State Constants and Variables
    // common state options
    public int currentState => machine.state;
    private StateMachine machine;

    // stIdleGround options
    private int proceedIdleGroundFrame;
    public int preInputFrameIdleGround;

    // stIdleGroundLong options
    // TODO: 외부 클래스에서 접근해야 할 필요가 있음.
    public int idleLongTransitionFrame = 900;

    // stSit options
    // TODO: 외부 클래스에서 접근해야 할 필요가 있음.
    public int proceedSitFrame;
    private RaycastHit2D currentSitGround;

    // stHeadUp options
    // TODO: 외부 클래스에서 접근해야 할 필요가 있음.
    public int proceedHeadUpFrame;

    // stWalk options
    public float walkSpeed = 3.5f;

    // stRun options
    public float runSpeed = 7.0f;

    // stFreeFall options
    public float maxFreeFallSpeed = 12.0f;
    public int freeFallFrame = 39;
    private DiscreteGraph freeFallGraph;
    private int proceedFreeFallFrame;
    public int preInputFrameFreeFall;

    // stGliding options
    public float glidingSpeed = 0.05f;
    public int glidingAccelFrameX = 39;
    public int glidingDeaccelFrameX = 26;
    private DiscreteGraph glidingAccelGraphX;
    private DiscreteGraph glidingDeaccelGraphX;
    private int proceedGlidingAccelFrameX;
    private int leftGlidingDeaccelFrameX;

    // stIdleOnWall options
    public int preInputFrameIdleWall;

    // stWallSliding options
    public float maxWallSlidingSpeed = 1.5f;
    public int wallSlidingFrame = 26;
    private DiscreteGraph wallSlidingGraph;
    private int proceedWallSlidingFrame;

    // stLedgeClimb options
    public Vector2 ledgeHoldPos;
    public Vector2 ledgeEndPos;
    public bool isLedgeAnimationEnded;

    // stJumpGround options
    public int jumpGroundCount = 1;
    public float jumpGroundSpeed = 5.5f;
    public int jumpGroundFrame = 18;
    private DiscreteGraph jumpGroundGraph;
    private int leftJumpGroundCount;
    private int leftJumpGroundFrame;
    private bool isJumpGroundCanceled;

    // stJumpDown options
    public float jumpDownSpeed = 1.5f;
    public int jumpDownFrame = 13;
    private DiscreteGraph jumpDownGraph;
    private int leftJumpDownFrame;

    // stRoll options
    public float rollSpeed = 9.5f;
    public int rollStartFrame = 6;
    public int rollInvincibilityFrame = 18;
    public int rollWakeUpFrame = 6;
    private DiscreteGraph rollGraph;
    private int leftRollStartFrame;
    private int leftRollInvincibilityFrame;
    private int leftRollWakeUpFrame;
    private int leftRollFrame;
    private int rollLookDir;

    // stJumpAir options
    public int jumpAirCount = 1;
    public float jumpAirSpeed = 7.5f;
    public int jumpAirIdleFrame = 3;
    public int jumpAirFrame = 20;
    private DiscreteGraph jumpAirGraph;
    private int leftJumpAirCount;
    private int leftJumpAirIdleFrame;
    private int leftJumpAirFrame;
    private bool isJumpAirCanceled;

    // stDash options
    public int dashCount = 1;
    public float dashSpeed = 36.0f;
    public int dashIdleFrame = 6;
    public int dashInvincibilityFrame = 9;
    private DiscreteGraph dashGraph;
    private int leftDashCount;
    private int leftDashIdleFrame;
    private int leftDashInvincibilityFrame;
    private int dashLookDir;

    // stTakeDown options
    public float takeDownSpeed = 48.0f;
    public int takeDownAirIdleFrame = 18;
    public int takeDownLandingIdleFrame = 12;
    private int leftTakeDownAirIdleFrame;
    private int leftTakeDownLandingIdleFrame;
    private bool isTakeDownAirIdleEnded;
    private bool isLandingAfterTakeDown;

    // stJumpWall options
    public float jumpWallSpeedX = 7.0f;
    public float jumpWallSpeedY = 10.0f;
    public int jumpWallFrame = 13;
    public int jumpWallForceFrame = 6;
    private DiscreteGraph jumpWallGraphX;
    private DiscreteGraph jumpWallGraphY;
    private int leftJumpWallFrame;
    private int leftJumpWallForceFrame;
    private int jumpWallLookDir;
    private bool isJumpWallCanceledX;
    private bool isJumpWallCanceledXY;
    #endregion

    private void m_InitPositions()
    {
        Bounds hBounds = headBox.bounds;
        Bounds fBounds = feetBox.bounds;
        Bounds cBounds = bodyBox.bounds;
        Vector2 pPos = transform.position;

        headBox.usedByComposite = false;
        feetBox.usedByComposite = false;
        bodyBox.usedByComposite = false;

        dir_hPos.Set(hBounds.center.x - pPos.x, hBounds.max.y - pPos.y);
        dir_hlPos.Set(hBounds.min.x - pPos.x, hBounds.center.y - pPos.y);
        dir_hrPos.Set(hBounds.max.x - pPos.x, hBounds.center.y - pPos.y);
        dir_fPos.Set(fBounds.center.x - pPos.x, fBounds.min.y - pPos.y);
        dir_flPos.Set(fBounds.min.x - pPos.x, fBounds.center.y - pPos.y);
        dir_frPos.Set(fBounds.max.x - pPos.x, fBounds.center.y - pPos.y);
        dir_cPos.Set(cBounds.center.x - pPos.x, cBounds.center.y - pPos.y);
        dir_clPos.Set(cBounds.min.x - pPos.x, cBounds.center.y - pPos.y);
        dir_crPos.Set(cBounds.max.x - pPos.x, cBounds.center.y - pPos.y);
        dir_ltPos.Set(hBounds.min.x - pPos.x, hBounds.min.y - pPos.y);
        dir_lbPos.Set(fBounds.min.x - pPos.x, fBounds.max.y - pPos.y);
        dir_rtPos.Set(hBounds.max.x - pPos.x, hBounds.min.y - pPos.y);
        dir_rbPos.Set(fBounds.max.x - pPos.x, fBounds.max.y - pPos.y);

        headBox.usedByComposite = true;
        feetBox.usedByComposite = true;
        bodyBox.usedByComposite = true;
    }

    private void m_UpdatePositions()
    {
        Vector2 pPos = transform.position;

        hPos.Set(pPos.x + dir_hPos.x, pPos.y + dir_hPos.y);
        hlPos.Set(pPos.x + dir_hlPos.x, pPos.y + dir_hlPos.y);
        hrPos.Set(pPos.x + dir_hrPos.x, pPos.y + dir_hrPos.y);
        fPos.Set(pPos.x + dir_fPos.x, pPos.y + dir_fPos.y);
        flPos.Set(pPos.x + dir_flPos.x, pPos.y + dir_flPos.y);
        frPos.Set(pPos.x + dir_frPos.x, pPos.y + dir_frPos.y);
        cPos.Set(pPos.x + dir_cPos.x, pPos.y + dir_cPos.y);
        clPos.Set(pPos.x + dir_clPos.x, pPos.y + dir_clPos.y);
        crPos.Set(pPos.x + dir_crPos.x, pPos.y + dir_crPos.y);
        ltPos.Set(pPos.x + dir_ltPos.x, pPos.y + dir_ltPos.y);
        lbPos.Set(pPos.x + dir_lbPos.x, pPos.y + dir_lbPos.y);
        rtPos.Set(pPos.x + dir_rtPos.x, pPos.y + dir_rtPos.y);
        rbPos.Set(pPos.x + dir_rbPos.x, pPos.y + dir_rbPos.y);
    }

    protected void UpdateLookDir()
    {
        if(lookDir == 0)
            lookDir = 1;

        if(!canUpdateLookDir)
            return;

        int xInput = inputData.xNegative + inputData.xPositive;

        if(xInput != 0)
        {
            lookDir = xInput;
            hexaCol.offset = new Vector2(hexaColOffset.x * lookDir, hexaColOffset.y);
            spRenderer.flipX = (lookDir == 1); 
        }
    }

    protected void UpdateMoveDirection()
    {
        if(isDetectedGround)
        {
            vx = detectedGround.normal.y;
            vy = -detectedGround.normal.x;

            moveDir.Set(vx, vy);
        }
        else
        {
            moveDir.Set(1.0f, 0.0f);
        }
    }

    protected float GetMoveSpeed()
    {
        return isRun ? runSpeed : walkSpeed;
    }

    protected float CheckVelocityX(float currentVx)
    {
        if(inputData.xInput == lookDir && (isHitWallT == lookDir || isHitWallB == lookDir))
            return 0.0f;

        return currentVx;
    }
// (Check Ground)/(Ceil)/(Wall)/(Head-Over Semi Ground)/(Ledge)/(Ledge Hanging)
    protected void CheckGround()
    {
        CheckGroundAll(out detectedGround, out isDetectedGround, fPos, 0.5f);

        if(!isDetectedGround)
        {
            isHitGround = false;
        }
        else if(detectedGround.collider.gameObject.layer == LayerInfo.ground)
            isHitGround = detectedGround.distance <= 0.04f;
        else if(detectedGround.collider.gameObject.layer == LayerInfo.semiGround)
        {
            if(base.CanCollision(detectedGround.collider))
            {
                isHitGround = detectedGround.distance <= 0.04f;
            }
            else
            {
                detectedGround = default(RaycastHit2D);
                isDetectedGround = false;
                isHitGround = false;
            }
        }
    }

    protected void CheckCeil()
    {
        if(canCheckCeil)
        {
            CheckCeil(out detectedCeil, out isHitCeil, hPos, 0.04f);
        }
        else
        {
            detectedCeil = default(RaycastHit2D);
            isHitCeil = false;
        }
    }

    protected void CheckWall()
    {
        Vector2 fsPos = Vector2.zero;
        Vector2 hsPos = Vector2.zero;

        if(lookDir == 1)
        {
            fsPos = rbPos;
            hsPos = rtPos;
        }
        else if(lookDir == -1)
        {
            fsPos = lbPos;
            hsPos = ltPos;
        }

        float detectLength = 0.04f;

        base.CheckWall(out detectedWallB, out isHitWallB, fsPos, detectLength, lookDir);
        base.CheckWall(out detectedWallT, out isHitWallT, hsPos, detectLength, lookDir);
    }

    protected void CheckHeadOverSemiGround()
    {
        float detectLength = base.height + 0.5f;
        int layer = LayerInfo.semiGroundMask;

        headOverSemiGroundBefores = headOverSemiGroundCurrents;
        headOverSemiGroundCurrents = Physics2D.RaycastAll(fPos, Vector2.up, detectLength, layer);

        if(headOverSemiGroundBefores == null || headOverSemiGroundCurrents == null)
            return;

        for(int i = 0; i < headOverSemiGroundCurrents.Length; i++)
        {
            bool exist = Array.Exists<RaycastHit2D>(headOverSemiGroundBefores, (element) => element.collider == headOverSemiGroundCurrents[i].collider);
            bool canCollision = base.CanCollision(headOverSemiGroundCurrents[i].collider);

            if(!exist && canCollision)
                base.IgnoreCollision(headOverSemiGroundCurrents[i].collider);
        }

        for(int i = 0; i < headOverSemiGroundBefores.Length; i++)
        {
            bool exist = Array.Exists<RaycastHit2D>(headOverSemiGroundCurrents, (element) => element.collider == headOverSemiGroundBefores[i].collider);
            bool canCollision = base.CanCollision(headOverSemiGroundBefores[i].collider);

            if(!exist && !canCollision)
                base.AcceptCollision(headOverSemiGroundBefores[i].collider);
        }
    }

    protected void CheckLedge()
    {
        if(!canCheckLedge)
        {
            isHitLedgeHead = false;
            isHitLedgeBody = false;
            detectedLedge = default(RaycastHit2D);
            ledgeCornerTopPos.Set(0.0f, 0.0f);
            ledgeCornerSidePos.Set(0.0f, 0.0f);
            return;
        }

        int layer = LayerInfo.groundMask;
        Vector2 sidePos = Vector2.zero;
        Vector2 sideOverPos = Vector2.zero;
        Vector2 bodyPos = Vector2.zero;
        Vector2 detectDir = Vector2.zero;

        if(lookDir == 1)
        {
            sidePos = rtPos;
            sideOverPos.Set(hrPos.x, hrPos.y + ledgeCheckingHeight);
            bodyPos = crPos;
            detectDir.Set(1.0f, 0.0f);
        }
        else if(lookDir == -1)
        {
            sidePos = ltPos;
            sideOverPos.Set(hlPos.x, hlPos.y + ledgeCheckingHeight);
            bodyPos = clPos;
            detectDir.Set(-1.0f, 0.0f);
        }

        RaycastHit2D sideHit = Physics2D.Raycast(sidePos, detectDir, ledgeCheckingWidth, layer);
        RaycastHit2D sideOverHit = Physics2D.Raycast(sideOverPos, detectDir, ledgeCheckingWidth, layer);
        RaycastHit2D bodyHit = Physics2D.Raycast(bodyPos, detectDir, ledgeCheckingWidth, layer);

        isHitLedgeHead = !sideOverHit &&sideHit;
        isHitLedgeBody = !sideOverHit &&bodyHit;

        bool a = sideOverHit;
        bool b = isHitLedgeHead;
        bool c = isHitLedgeBody;

        if(!sideOverHit && (isHitLedgeHead || isHitLedgeBody))
        {
            float adder = 0.1f;
            float distance = (sideHit.distance + adder) * lookDir;
            float height = base.height;

            detectedLedge = Physics2D.Raycast(sideOverPos + Vector2.right * distance, Vector2.down, height, layer);
            bool d = detectedLedge;
            ledgeCornerTopPos.Set(detectedLedge.point.x, detectedLedge.point.y);
            ledgeCornerSidePos.Set(detectedLedge.point.x - adder * lookDir, detectedLedge.point.y - adder);
        }
        else
        {
            detectedLedge = default(RaycastHit2D);
            ledgeCornerTopPos.Set(0.0f, 0.0f);
            ledgeCornerSidePos.Set(0.0f, 0.0f);
        }
    }

    protected void CheckLedgeHanging()
    {
        float detectLength = 0.04f;
        int layer = LayerInfo.groundMask | LayerInfo.semiGroundMask;

        leftHangingGround = Physics2D.Raycast(flPos, Vector2.down, detectLength, layer);
        rightHangingGround = Physics2D.Raycast(frPos, Vector2.down, detectLength, layer);

        bool lExist = leftHangingGround;
        bool rExist = rightHangingGround;
        Collider2D lCol = null;
        Collider2D rCol = null;
        int lLayer = 0;
        int rLayer = 0;
        bool lRes = false;
        bool rRes = false;

        if(lExist)
        {
            lCol = leftHangingGround.collider;
            lLayer = lCol.gameObject.layer;

            if(lLayer == LayerInfo.ground)
                lRes = true;
            else if(lLayer == LayerInfo.semiGround && base.CanCollision(lCol))
                lRes = true;
        }
        if(rExist)
        {
            rCol = rightHangingGround.collider;
            rLayer = rCol.gameObject.layer;

            if(rLayer == LayerInfo.ground)
                rRes = true;
            else if(rLayer == LayerInfo.semiGround && base.CanCollision(rCol))
                rRes = true;
        }

        isHangingGround = lRes | rRes;
    }

    #region Unity Event Functions
    protected override void Start()
    {
        base.Start();
        this.CheckDataTable(Application.persistentDataPath + "/DataTable.txt");

        initialPosition = transform.position;
        m_InitPositions();

        // spRenderer = GetComponent<SpriteRenderer>();
        // hexaCol = GetComponent<ElongatedHexagonCollider2D>();
        hexaColOffset = hexaCol.offset;

        machine = new StateMachine(stIdleGround);

        machine.SetCallbacks(stIdleGround, Input_IdleGround, Logic_IdleGround, Enter_IdleGround, End_IdleGround);
        machine.SetCallbacks(stIdleGroundLong, Input_IdleGroundLong, Logic_IdleGroundLong, Enter_IdleGroundLong, null);
        machine.SetCallbacks(stSit, Input_Sit, Logic_Sit, Enter_Sit, End_Sit);
        machine.SetCallbacks(stHeadUp, Input_HeadUp, Logic_HeadUp, Enter_HeadUp, End_HeadUp);
        machine.SetCallbacks(stWalk, Input_Walk, Logic_Walk, Enter_Walk, null);
        machine.SetCallbacks(stRun, Input_Run, Logic_Run, Enter_Run, null);
        machine.SetCallbacks(stFreeFall, Input_FreeFall, Logic_FreeFall, Enter_FreeFall, null);
        machine.SetCallbacks(stGliding, Input_Gliding, Logic_Gliding, Enter_Gliding, null);
        machine.SetCallbacks(stIdleWall, Input_IdleWall, Logic_IdleWall, Enter_IdleWall, null);
        machine.SetCallbacks(stWallSliding, Input_WallSliding, Logic_WallSliding, Enter_WallSliding, null);
        machine.SetCallbacks(stLedgeClimbHead, Input_LedgeClimbHead, Logic_LedgeClimbHead, Enter_LedgeClimbHead, End_LedgeClimbHead);
        machine.SetCallbacks(stLedgeClimbBody, Input_LedgeClimbBody, Logic_LedgeClimbBody, Enter_LedgeClimbBody, End_LedgeClimbBody);
        machine.SetCallbacks(stJumpGround, Input_JumpGround, Logic_JumpGround, Enter_JumpGround, null);
        machine.SetCallbacks(stJumpDown, Input_JumpDown, Logic_JumpDown, Enter_JumpDown, null);
        machine.SetCallbacks(stRoll, Input_Roll, Logic_Roll, Enter_Roll, null);
        machine.SetCallbacks(stJumpAir, Input_JumpAir, Logic_JumpAir, Enter_JumpAir, null);
        machine.SetCallbacks(stDash, Input_Dash, Logic_Dash, Enter_Dash, null);
        machine.SetCallbacks(stTakeDown, Input_TakeDown, Logic_TakeDown, Enter_TakeDown, End_TakeDown);
        machine.SetCallbacks(stJumpWall, Input_JumpWall, Logic_JumpWall, Enter_JumpWall, null);

        // TODO: ApplyFile은 테스트할 때만 사용하고, 본 스크립트의 초기값을 설정하고, InitGraphs() 함수만 수행하도록 한다.
        // InitGraphs();
        ApplyFile();
    }

    public void InitGraphs()
    {
        freeFallGraph = new DiscreteLinearGraph(freeFallFrame);
        glidingAccelGraphX = new DiscreteLinearGraph(glidingAccelFrameX);
        glidingDeaccelGraphX = new DiscreteLinearGraph(glidingDeaccelFrameX);
        wallSlidingGraph = new DiscreteLinearGraph(wallSlidingFrame);
        jumpGroundGraph = new DiscreteLinearGraph(jumpGroundFrame);
        jumpDownGraph = new DiscreteLinearGraph(jumpDownFrame);
        rollGraph = new DiscreteLinearGraph(rollStartFrame + rollInvincibilityFrame + rollWakeUpFrame);
        jumpAirGraph = new DiscreteLinearGraph(jumpAirFrame);
        dashGraph = new DiscreteLinearGraph(dashInvincibilityFrame);
        jumpWallGraphX = new DiscreteLinearGraph(jumpWallFrame);
        jumpWallGraphY = new DiscreteLinearGraph(jumpWallFrame);
    }

    protected override void Update()
    {
        base.Update();

        inputData.Copy(InputHandler.data);

        machine.UpdateInput();

        animator.SetInteger("currentState", machine.state);
        // UnityEngine.Debug.Log(string.Format("current state: {0}", machine.state));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        m_UpdatePositions();

        CheckGround();
        CheckCeil();
        CheckWall();
        CheckHeadOverSemiGround();
        CheckLedge();
        CheckLedgeHanging();

        UpdateLookDir();
        UpdateMoveDirection();

        machine.UpdateLogic();
    }
    #endregion

    #region Implement State; stIdleGround
    private void Enter_IdleGround()
    {
        DisableGravity();

        proceedIdleGroundFrame = 0;

        leftJumpGroundCount = jumpGroundCount;
        leftJumpAirCount = jumpAirCount;
        leftDashCount = dashCount;

        // 아래 점프 가능한 발판과의 통과 여부 취소
        if(currentSitGround)
        {
            bool exists = Array.Exists<RaycastHit2D>(headOverSemiGroundCurrents, (element) => element.collider == currentSitGround.collider);
            bool canCollision = base.CanCollision(currentSitGround.collider);

            if(!exists && !canCollision)
                base.AcceptCollision(currentSitGround.collider);

            currentSitGround = default(RaycastHit2D);
        }

        // 선입력 확인
        uint mask_jumpGround = 0b01;
        uint mask_roll = 0b10;
        preInputPressing = 0;
        preInputDown = 0;

        for(int i = 0; i < preInputFrameIdleGround; i++)
        {
            preInputData.Copy(InputHandler.data);

            if((preInputPressing & mask_jumpGround) == 0 && preInputData.jumpPressing)
                preInputPressing |= mask_jumpGround;
            if((preInputDown & mask_jumpGround) == 0 && preInputData.jumpDown)
                preInputDown |= mask_jumpGround;
            if((preInputPressing & mask_roll) == 0 && preInputData.dashPressing)
                preInputPressing |= mask_roll;
            if((preInputDown & mask_roll) == 0 && preInputData.dashDown)
                preInputDown |= mask_roll;
        }

        if((preInputDown & mask_roll) != 0 && (preInputPressing & mask_roll) != 0)
            machine.ChangeState(stRoll);
        else if((preInputDown & mask_jumpGround) != 0 && (preInputPressing & mask_jumpGround) != 0 && leftJumpGroundCount > 0)
            machine.ChangeState(stJumpGround);
    }

    private void Input_IdleGround()
    {
        if(!isHitGround)
            machine.ChangeState(stFreeFall);
        else if(inputData.jumpDown)
            machine.ChangeState(stJumpGround);
        else if(inputData.dashDown)
            machine.ChangeState(stRoll);
        else if(inputData.yNegative != 0)
            machine.ChangeState(stSit);
        else if(inputData.yPositive != 0)
            machine.ChangeState(stHeadUp);
        else if(inputData.xInput != 0)
            machine.ChangeState(isRun ? stRun : stWalk);
        else if(proceedIdleGroundFrame >= idleLongTransitionFrame)
            machine.ChangeState(stIdleGroundLong);
    }

    private void Logic_IdleGround()
    {
        proceedIdleGroundFrame++;
        SetVelocityXY(0.0f, 0.0f);
    }

    private void End_IdleGround()
    {
        proceedIdleGroundFrame = 0;
    }
    #endregion

    #region Implement State; stIdleGroundLong
    private void Enter_IdleGroundLong()
    {
        DisableGravity();
    }

    private void Input_IdleGroundLong()
    {
        if(!isHitGround)
            machine.ChangeState(stFreeFall);
        else if(inputData.jumpDown)
            machine.ChangeState(stJumpGround);
        else if(inputData.dashDown)
            machine.ChangeState(stRoll);
        else if(inputData.yNegative != 0)
            machine.ChangeState(stSit);
        else if(inputData.yPositive != 0)
            machine.ChangeState(stHeadUp);
        else if(inputData.xInput != 0)
            machine.ChangeState(isRun ? stRun : stWalk);
    }

    private void Logic_IdleGroundLong()
    {
        SetVelocityXY(0.0f, 0.0f);
    }
    #endregion

    #region Implement State; stHeadUp
    private void Enter_HeadUp()
    {
        DisableGravity();
        proceedHeadUpFrame = 0;
    }

    private void Input_HeadUp()
    {
        if(!isHitGround)
            machine.ChangeState(stFreeFall);
        else if(inputData.jumpDown)
            machine.ChangeState(stJumpGround);
        else if(inputData.dashDown)
            machine.ChangeState(stRoll);
        else if(inputData.yPositive == 0)
            machine.ChangeState(stIdleGround);
    }

    private void Logic_HeadUp()
    {
        proceedHeadUpFrame++;
        SetVelocityXY(0.0f, 0.0f);
    }

    private void End_HeadUp()
    {
        proceedHeadUpFrame = 0;
    }
    #endregion

    #region Implement State; stSit
    private void Enter_Sit()
    {
        DisableGravity();
        proceedSitFrame = 0;
        currentSitGround = detectedGround;
    }

    private void Input_Sit()
    {
        if(!isHitGround)
            machine.ChangeState(stFreeFall);
        else if(inputData.jumpDown)
        {
            if(detectedGround.collider.gameObject.layer == LayerInfo.semiGround)
                machine.ChangeState(stJumpDown);
            else
                machine.ChangeState(stJumpGround);
        }
        else if(inputData.dashDown)
            machine.ChangeState(stRoll);
        else if(inputData.yNegative == 0)
            machine.ChangeState(stIdleGround);
    }

    private void Logic_Sit()
    {
        proceedSitFrame++;
        SetVelocityXY(0.0f, 0.0f);
    }

    private void End_Sit()
    {
        proceedSitFrame = 0;
    }
    #endregion

    #region Implement State; stWalk
    private void Enter_Walk()
    {
        EnableGravity();
    }

    private void Input_Walk()
    {
        if(!isHitGround)
            machine.ChangeState(stFreeFall);
        else if(inputData.yNegative != 0)
            machine.ChangeState(stSit);
        else if(inputData.yPositive != 0)
            machine.ChangeState(stHeadUp);
        else if(isRun)
            machine.ChangeState(stRun);
        else if(inputData.jumpDown)
            machine.ChangeState(stJumpGround);
        else if(inputData.dashDown)
            machine.ChangeState(stRoll);
        else if(inputData.xInput == 0)
            machine.ChangeState(stIdleGround);
    }

    private void Logic_Walk()
    {
        float speed = CheckVelocityX(walkSpeed);

        if(speed == 0)
            DisableGravity();
        else
            EnableGravity();

        Logic_MoveOnGround(moveDir, speed, lookDir);
    }
    #endregion

    #region Implement State; stRun
    private void Enter_Run()
    {
        EnableGravity();
    }

    private void Input_Run()
    {
        if(!isHitGround)
            machine.ChangeState(stFreeFall);
        else if(inputData.yNegative != 0)
            machine.ChangeState(stSit);
        else if(inputData.yPositive != 0)
            machine.ChangeState(stHeadUp);
        else if(!isRun)
            machine.ChangeState(stWalk);
        else if(inputData.jumpDown)
            machine.ChangeState(stJumpGround);
        else if(inputData.dashDown)
            machine.ChangeState(stRoll);
        else if(inputData.xInput == 0)
            machine.ChangeState(stIdleGround);
    }

    private void Logic_Run()
    {
        float speed = CheckVelocityX(runSpeed);

        if(speed == 0)
            DisableGravity();
        else
            EnableGravity();

        Logic_MoveOnGround(moveDir, speed, lookDir);
    }
    #endregion

    #region Implement State; stFreeFall
    private void Enter_FreeFall()
    {
        EnableGravity();

        if(currentVelocity.y > 0.0f)
        {
            proceedFreeFallFrame = 0;
        }
        else if(currentVelocity.y < -maxFreeFallSpeed)
        {
            proceedFreeFallFrame = freeFallFrame;
        }
        else
        {
            for(int i = 0; i < freeFallFrame; i++)
            {
                if(currentVelocity.y >= -maxFreeFallSpeed * freeFallGraph[i])
                {
                    proceedFreeFallFrame = i;
                    break;
                }
            }
        }

        // 선입력
        uint mask_jumpAir = 0b01;
        preInputPressing = 0b00;
        preInputDown = 0b00;

        for(int i = 0; i < preInputFrameFreeFall; i++)
        {
            preInputData.Copy(InputHandler.data);

            if((preInputPressing & mask_jumpAir) == 0 && preInputData.jumpPressing)
                preInputPressing |= mask_jumpAir;
            if((preInputDown & mask_jumpAir) == 0 && preInputData.jumpDown)
                preInputDown |= mask_jumpAir;
        }

        if((preInputDown & mask_jumpAir) != 0 && (preInputPressing & mask_jumpAir) != 0 && leftJumpAirCount > 0)
            machine.ChangeState(stJumpAir);
    }

    private void Input_FreeFall()
    {
        if(isHitGround)
            machine.ChangeState(stIdleGround);
        else if(inputData.jumpDown)
        {
            if(inputData.yNegative != 0)
                machine.ChangeState(stTakeDown);
            else if(leftJumpAirCount > 0)
                machine.ChangeState((stJumpAir));
        }
        else if(inputData.dashDown && leftDashCount > 0)
            machine.ChangeState(stDash);
        else if(inputData.yPositive != 0)
            machine.ChangeState(stGliding);
        else if(inputData.xInput == lookDir)
        {
            if(isHitLedgeHead)
                machine.ChangeState(stLedgeClimbHead);
            else if(isHitLedgeBody)
                machine.ChangeState(stLedgeClimbBody);
            else if(!isDetectedGround && inputData.yNegative == 0 && isHitWallB == lookDir && isHitWallT == lookDir)
                machine.ChangeState(stIdleWall);
        }
    }

    private void Logic_FreeFall()
    {
        if(isHangingGround)
            proceedFreeFallFrame = 0;
        else
        {
            if(proceedFreeFallFrame < freeFallFrame)
                proceedFreeFallFrame++;

            vx = CheckVelocityX(GetMoveSpeed() * inputData.xInput);
            vy = -maxFreeFallSpeed * freeFallGraph[proceedFreeFallFrame - 1];
            SetVelocityXY(vx, vy);
        }
    }
    #endregion

    #region Implement State; stGliding
    private void Enter_Gliding()
    {
        EnableGravity();

        if(Mathf.Abs(currentVelocity.x) == 0.0f)
        {
            leftGlidingDeaccelFrameX = 0;
            proceedGlidingAccelFrameX = 0;
        }
        else if(Mathf.Abs(currentVelocity.x) > GetMoveSpeed())
        {
            if(inputData.xInput == 0)
            {
                leftGlidingDeaccelFrameX = glidingDeaccelFrameX;
                proceedGlidingAccelFrameX = 0;
            }
            else
            {
                leftGlidingDeaccelFrameX = 0;
                proceedGlidingAccelFrameX = glidingAccelFrameX;
            }
        }
        else
        {
            float tx = Mathf.Abs(currentVelocity.x);

            if(tx > glidingSpeed)
            {
                leftGlidingDeaccelFrameX = glidingDeaccelFrameX;
                proceedGlidingAccelFrameX = glidingAccelFrameX;
            }
            else if(inputData.xInput == 0)
            {
                for(int i = 0; i < glidingDeaccelFrameX; i++)
                {
                    if(Mathf.Abs(currentVelocity.x) >= glidingDeaccelGraphX[i] * glidingSpeed)
                    {
                        leftGlidingDeaccelFrameX = i;
                        proceedGlidingAccelFrameX = glidingAccelFrameX;
                        while(proceedGlidingAccelFrameX > 0 &&  glidingAccelGraphX[proceedGlidingAccelFrameX - 1] > glidingDeaccelGraphX[leftGlidingDeaccelFrameX])
                            proceedGlidingAccelFrameX--;
                        break;
                    }
                }
            }
            else
            {
                for(int i = 0; i < glidingAccelFrameX; i++)
                {
                    if(Mathf.Abs(currentVelocity.x) >= glidingAccelGraphX[i] * glidingSpeed)
                    {
                        proceedGlidingAccelFrameX = i;
                        leftGlidingDeaccelFrameX = 0;
                        while(leftGlidingDeaccelFrameX < glidingDeaccelFrameX && glidingDeaccelGraphX[leftGlidingDeaccelFrameX] < glidingAccelGraphX[proceedGlidingAccelFrameX])
                            leftGlidingDeaccelFrameX++;
                        break;
                    }
                }
            }
        }
    }

    private void Input_Gliding()
    {
        if(isHitGround)
            machine.ChangeState(stIdleGround);
        else if(inputData.jumpDown && leftJumpAirCount > 0)
            machine.ChangeState(stJumpAir);
        else if(inputData.dashDown && leftDashCount > 0)
            machine.ChangeState(stDash);
        else if(inputData.yPositive == 0)
            machine.ChangeState(stFreeFall);
        else if(inputData.xInput == lookDir)
        {
            if(isHitLedgeHead)
                machine.ChangeState(stLedgeClimbHead);
            else if(isHitLedgeBody)
                machine.ChangeState(stLedgeClimbBody);
            else if(!isDetectedGround && inputData.yNegative == 0 && isHitWallB == lookDir && isHitWallT == lookDir)
                machine.ChangeState(stIdleWall);
        }
    }

    private void Logic_Gliding()
    {
        vy = -glidingSpeed;

        if(currentVelocity.x * inputData.xInput < 0.0f) // 방향전환 확인
        {
            vx *= -1.0f;
        }
        else if(inputData.xInput == 0)
        {
            if(leftGlidingDeaccelFrameX > 0)
                leftGlidingDeaccelFrameX--;

            while(proceedGlidingAccelFrameX > 0 &&  glidingAccelGraphX[proceedGlidingAccelFrameX - 1] > glidingDeaccelGraphX[leftGlidingDeaccelFrameX])
                proceedGlidingAccelFrameX--;

            vx = CheckVelocityX(GetMoveSpeed() * glidingDeaccelGraphX[leftGlidingDeaccelFrameX] * lookDir);
        }
        else if(inputData.xInput != 0)
        {
            if(proceedGlidingAccelFrameX < glidingAccelFrameX)
                proceedGlidingAccelFrameX++;

            while(leftGlidingDeaccelFrameX < glidingDeaccelFrameX && glidingDeaccelGraphX[leftGlidingDeaccelFrameX] < glidingAccelGraphX[proceedGlidingAccelFrameX - 1])
                leftGlidingDeaccelFrameX++;

            vx = CheckVelocityX(GetMoveSpeed() * glidingAccelGraphX[proceedGlidingAccelFrameX - 1] * lookDir);
        }

        SetVelocityXY(vx, vy);
    }
    #endregion

    #region Implement State; stIdleWall
    private void Enter_IdleWall()
    {
        DisableGravity();
        leftJumpAirCount = jumpAirCount;
        leftDashCount = dashCount;

        // 아래 점프 가능한 발판과의 통과 여부 취소
        if(currentSitGround)
        {
            bool exists = Array.Exists<RaycastHit2D>(headOverSemiGroundCurrents, (element) => element.collider == currentSitGround.collider);
            bool canCollision = base.CanCollision(currentSitGround.collider);

            if(!exists && !canCollision)
                base.AcceptCollision(currentSitGround.collider);

            currentSitGround = default(RaycastHit2D);
        }

        // 선입력
        uint mask_jumpWall = 0b01;
        preInputPressing = 0b00;
        preInputDown = 0b00;

        for(int i = 0; i < preInputFrameIdleWall; i++)
        {
            preInputData.Copy(InputBuffer.GetBufferedData(i));

            if((preInputPressing & mask_jumpWall) == 0 && preInputData.jumpPressing)
                preInputPressing |= mask_jumpWall;
            if((preInputDown & mask_jumpWall) == 0 && preInputData.jumpDown)
                preInputDown |= mask_jumpWall;
        }

        if((preInputDown & mask_jumpWall) != 0 && (preInputPressing & mask_jumpWall) != 0)
            machine.ChangeState(stJumpWall);
    }

    private void Input_IdleWall()
    {
        if(isDetectedGround || isHitWallB == 0 || isHitWallT == 0 || inputData.yNegDown)
            machine.ChangeState(stFreeFall);
        else if(inputData.xInput == 0)
            machine.ChangeState(stWallSliding);
        else if(inputData.jumpDown)
            machine.ChangeState(stJumpWall);
    }

    private void Logic_IdleWall()
    {
        SetVelocityXY(0.0f, 0.0f);
    }
    #endregion

    #region Implement State; stWallSliding
    private void Enter_WallSliding()
    {
        EnableGravity();
        proceedWallSlidingFrame = 0;
    }

    private void Input_WallSliding()
    {
        if(isHitGround)
            machine.ChangeState(stIdleGround);
        else if(isDetectedGround || isHitWallB == 0 || isHitWallT == 0 || inputData.yNegDown)
            machine.ChangeState(stFreeFall);
        else if(inputData.xInput == lookDir && inputData.yNegative == 0 && !isDetectedGround && isHitWallB == lookDir && isHitWallT == lookDir)
            machine.ChangeState(stIdleWall);
        else if(inputData.jumpDown)
            machine.ChangeState(stJumpWall);
    }

    private void Logic_WallSliding()
    {
        if(proceedWallSlidingFrame < wallSlidingFrame)
            proceedWallSlidingFrame++;

        vx = 0.0f;
        vy = -maxWallSlidingSpeed * wallSlidingGraph[proceedWallSlidingFrame - 1];
        SetVelocityXY(vx, vy);
    }
    #endregion

    #region Implement State; stLedgeClimbHead
    private void Enter_LedgeClimbHead()
    {
        DisableGravity();

        Vector2 sidePos = Vector2.zero;
        Vector2 handDir = Vector2.zero;
        Vector2 feetDir = Vector2.zero;

        if(lookDir == 1)
            sidePos = rtPos;
        else if(lookDir == -1)
            sidePos = ltPos;

        handDir.Set(transform.position.x - sidePos.x, transform.position.y - sidePos.y);
        feetDir.Set(transform.position.x - fPos.x, transform.position.y - fPos.y);

        ledgeHoldPos.Set(ledgeCornerSidePos.x + handDir.x, ledgeCornerSidePos.y + handDir.y);
        ledgeEndPos.Set(ledgeCornerTopPos.x + feetDir.x, ledgeCornerTopPos.y + feetDir.y);

        canUpdateLookDir = false;
        canCheckLedge = false;
        isLedgeAnimationEnded = false;

        // TODO: Animation Clip을 넣고 OnLedgeAnimationEnded 함수를 Animation Event로 등록 후 이 코드는 삭제.
        Action holdLedge = () =>
        {
            Thread.Sleep(500);
            isLedgeAnimationEnded = true;
        };

        Thread holdThread = new Thread(new ThreadStart(holdLedge));
        holdThread.Start();

        transform.position = ledgeHoldPos;
    }

    private void Input_LedgeClimbHead()
    {
        if(isLedgeAnimationEnded)
            machine.ChangeState(stIdleGround);
    }

    private void Logic_LedgeClimbHead()
    {
        SetVelocityXY(0.0f, 0.0f);
    }

    private void End_LedgeClimbHead()
    {
        transform.position = ledgeEndPos;
        canUpdateLookDir = true;
        canCheckLedge = true;
    }
    #endregion

    #region Implement State; stLedgeClimbBody
    private void Enter_LedgeClimbBody()
    {
        DisableGravity();

        Vector2 sidePos = Vector2.zero;
        Vector2 handDir = Vector2.zero;
        Vector2 feetDir = Vector2.zero;

        if(lookDir == 1)
            sidePos = crPos;
        else if(lookDir == -1)
            sidePos = clPos;

        handDir.Set(transform.position.x - sidePos.x, transform.position.y - sidePos.y);
        feetDir.Set(transform.position.x - fPos.x, transform.position.y - fPos.y);

        ledgeHoldPos.Set(ledgeCornerSidePos.x + handDir.x, ledgeCornerSidePos.y + handDir.y);
        ledgeEndPos.Set(ledgeCornerTopPos.x + feetDir.x, ledgeCornerTopPos.y + feetDir.y);

        canUpdateLookDir = false;
        canCheckLedge = false;
        isLedgeAnimationEnded = false;

        // TODO: Animation Clip을 넣고 OnLedgeAnimationEnded 함수를 Animation Event로 등록 후 이 코드는 삭제.
        Action holdLedge = () =>
        {
            Thread.Sleep(500);
            isLedgeAnimationEnded = true;
        };

        Thread holdThread = new Thread(new ThreadStart(holdLedge));
        holdThread.Start();

        transform.position = ledgeHoldPos;
    }

    private void Input_LedgeClimbBody()
    {
        if(isLedgeAnimationEnded)
            machine.ChangeState(stIdleGround);
    }

    private void Logic_LedgeClimbBody()
    {
        SetVelocityXY(0.0f, 0.0f);
    }

    private void End_LedgeClimbBody()
    {
        transform.position = ledgeEndPos;
        canUpdateLookDir = true;
        canCheckLedge = true;
    }
    #endregion

    #region Implement State; stJumpGround
    private void Enter_JumpGround()
    {
        EnableGravity();

        leftJumpGroundCount--;
        leftJumpGroundFrame = jumpGroundFrame;
        isJumpGroundCanceled = false;
    }

    private void Input_JumpGround()
    {
        if(inputData.jumpUp && !isJumpGroundCanceled)
        {
            leftJumpGroundFrame /= 2;
            isJumpGroundCanceled = true;
        }

        if(isHitCeil || leftJumpGroundFrame == 0)
        {
            if(inputData.yPositive == 0)
                machine.ChangeState(stFreeFall);
            else
                machine.ChangeState(stGliding);
        }
        else if(inputData.jumpDown)
        {
            if(inputData.yNegative != 0)
                machine.ChangeState(stTakeDown);
            else if(leftJumpAirCount > 0)
                machine.ChangeState(stJumpAir);
        }
        else if(inputData.dashDown && leftDashCount > 0)
            machine.ChangeState(stDash);
        else if(inputData.xInput == lookDir)
        {
            if(isHitLedgeHead)
                machine.ChangeState(stLedgeClimbHead);
            else if(isHitLedgeBody)
                machine.ChangeState(stLedgeClimbBody);
            else if(!isDetectedGround && inputData.yNegative == 0 && isHitWallB == lookDir && isHitWallT == lookDir)
                machine.ChangeState(stIdleWall);
        }
    }

    private void Logic_JumpGround()
    {
        if(leftJumpGroundFrame > 0)
            leftJumpGroundFrame--;

        vx = CheckVelocityX(inputData.xInput * GetMoveSpeed());
        vy = jumpGroundSpeed * jumpGroundGraph[leftJumpGroundFrame];

        SetVelocityXY(vx, vy);
    }
    #endregion

    #region Implement State; stJumpDown
    private void Enter_JumpDown()
    {
        EnableGravity();

        leftJumpDownFrame = jumpDownFrame;

        IgnoreCollision(currentSitGround.collider);
    }

    private void Input_JumpDown()
    {
        // if(leftJumpDownFrame == 0 || isHitCeil) machine.ChangeState(stFreeFall);

        if(isHitCeil)
            machine.ChangeState(stFreeFall);
        else if(Array.Exists<RaycastHit2D>(headOverSemiGroundCurrents, (element) => element.collider == currentSitGround.collider && element.distance >= 0.1f))
            machine.ChangeState(stFreeFall);
    }

    private void Logic_JumpDown()
    {
        if(leftJumpDownFrame > 0)
        {
            leftJumpDownFrame--;
            // vx = GetMoveSpeed() * inputData.xInput;
            vx = 0.0f;
            vy = jumpDownSpeed * jumpDownGraph[leftJumpDownFrame];
            SetVelocityXY(vx, vy);

            if(leftJumpDownFrame == 0)
                proceedFreeFallFrame = 0;
        }
        else
        {
            if(proceedFreeFallFrame < freeFallFrame)
                proceedFreeFallFrame++;

            vx = 0.0f;
            vy = -maxFreeFallSpeed * freeFallGraph[proceedFreeFallFrame - 1];
            SetVelocityXY(vx, vy);
        }
    }
    #endregion

    #region Implement State; stRoll
    private void Enter_Roll()
    {
        EnableGravity();
        leftRollStartFrame = rollStartFrame;
        leftRollInvincibilityFrame = 0;
        leftRollWakeUpFrame = 0;
        leftRollFrame = rollStartFrame + rollInvincibilityFrame + rollWakeUpFrame;
        rollLookDir = lookDir;
    }

    private void Input_Roll()
    {
        if(!isDetectedGround)
            machine.ChangeState(stFreeFall);
        else if(inputData.jumpDown && leftJumpGroundCount > 0 && leftRollFrame < rollInvincibilityFrame + rollWakeUpFrame)
            machine.ChangeState(stJumpGround);
        else if(inputData.xInput != 0 && leftRollFrame < rollWakeUpFrame)
            machine.ChangeState(isRun ? stRun : stWalk);
        else if(leftRollFrame == 0)
        {
            if(inputData.yNegative != 0)
                machine.ChangeState(stSit);
            else
                machine.ChangeState(stIdleGround);
        }
    }

    private void Logic_Roll()
    {
        // 프레임 갱신 및 구르기 내부 상태 전환
        // 구르기 초반->중반(무적)->후반(일어나기)
        if(leftRollStartFrame > 0)
        {
            leftRollStartFrame--;

            if(leftRollStartFrame == 0)
                leftRollInvincibilityFrame = rollInvincibilityFrame;
        }
        else if(leftRollInvincibilityFrame > 0)
        {
            leftRollInvincibilityFrame--;

            if(leftRollInvincibilityFrame == 0)
                leftRollWakeUpFrame = rollWakeUpFrame;
        }
        else if(leftRollWakeUpFrame > 0)
        {
            leftRollWakeUpFrame--;
        }

        // 전체 프레임 갱신
        if(leftRollFrame > 0)
            leftRollFrame--;

        float speed = CheckVelocityX(rollSpeed);

        if(speed == 0)
            DisableGravity();
        else
            EnableGravity();

        Logic_MoveOnGround(moveDir, speed * rollGraph[leftRollFrame], rollLookDir);
    }
    #endregion

    #region Implement State; stJumpAir
    private void Enter_JumpAir()
    {
        DisableGravity();
        leftJumpAirCount--;
        leftJumpAirIdleFrame = jumpAirIdleFrame;
        leftJumpAirFrame = 0;
        isJumpAirCanceled = false;
    }

    private void Input_JumpAir()
    {
        if(inputData.jumpUp && !isJumpAirCanceled)
        {
            leftJumpAirFrame /= 2;
            isJumpAirCanceled = true;
        }

        if((leftJumpAirIdleFrame == 0 && leftJumpAirFrame == 0) || isHitCeil)
        {
            if(inputData.yPositive == 0)
                machine.ChangeState(stFreeFall);
            else
                machine.ChangeState(stGliding);
        }
        else if(inputData.jumpDown)
        {
            if(leftJumpAirCount > 0)
                machine.RestartState();
            else if(inputData.yNegative != 0)
                machine.ChangeState(stTakeDown);
        }
        else if(inputData.dashDown && leftDashCount > 0)
            machine.ChangeState(stDash);
        else if(inputData.xInput == lookDir)
        {
            if(isHitLedgeHead)
                machine.ChangeState(stLedgeClimbHead);
            else if(isHitLedgeBody)
                machine.ChangeState(stLedgeClimbBody);
            else if(!isDetectedGround && inputData.yNegative == 0 && isHitWallB == lookDir && isHitWallT == lookDir)
                machine.ChangeState(stIdleWall);
        }
    }

    private void Logic_JumpAir()
    {
        if(leftJumpAirIdleFrame > 0)
        {
            leftJumpAirIdleFrame--;

            DisableGravity();
            SetVelocityXY(0.0f, 0.0f);

            if(leftJumpAirIdleFrame == 0)
                leftJumpAirFrame = jumpAirFrame;
        }
        else if(leftJumpAirFrame > 0)
        {
            EnableGravity();
            leftJumpAirFrame--;
            vx = CheckVelocityX(inputData.xInput * GetMoveSpeed());
            vy = jumpAirSpeed * jumpAirGraph[leftJumpAirFrame];
            SetVelocityXY(vx, vy);
        }
    }
    #endregion

    #region Implement State; stDash
    private void Enter_Dash()
    {
        DisableGravity();
        leftDashCount--;
        leftDashIdleFrame = dashIdleFrame;
        leftDashInvincibilityFrame = 0;
        dashLookDir = lookDir;
    }

    private void Input_Dash()
    {
        if(leftDashIdleFrame == 0 && leftDashInvincibilityFrame == 0)
            machine.ChangeState(stFreeFall);
        else if(inputData.xInput == lookDir)
        {
            if(isHitLedgeHead)
                machine.ChangeState(stLedgeClimbHead);
            else if(isHitLedgeBody)
                machine.ChangeState(stLedgeClimbBody);
            else if(!isDetectedGround && inputData.yNegative == 0 && isHitWallB == lookDir && isHitWallT == lookDir)
                machine.ChangeState(stIdleWall);
        }
    }

    private void Logic_Dash()
    {
        if(leftDashIdleFrame > 0)
        {
            DisableGravity();

            leftDashIdleFrame--;
            SetVelocityXY(0.0f, 0.0f);

            if(leftDashIdleFrame == 0)
                leftDashInvincibilityFrame = dashInvincibilityFrame;
        }
        else if(leftDashInvincibilityFrame > 0)
        {
            EnableGravity();

            leftDashInvincibilityFrame--;
            vx = CheckVelocityX(dashSpeed * dashGraph[leftDashInvincibilityFrame] * dashLookDir);
            vy = 0.0f;
            SetVelocityXY(vx, vy);
        }
    }
    #endregion

    #region Implement State; stTakeDown
    private void Enter_TakeDown()
    {
        leftTakeDownAirIdleFrame = takeDownAirIdleFrame;
        leftTakeDownLandingIdleFrame = 0;
        isTakeDownAirIdleEnded = false;
        isLandingAfterTakeDown = false;

        // rigid.constraints |= RigidbodyConstraints2D.FreezePositionX;
    }

    private void Input_TakeDown()
    {
        if(isTakeDownAirIdleEnded && isLandingAfterTakeDown && leftTakeDownAirIdleFrame == 0 && leftTakeDownLandingIdleFrame == 0)
        {
            if(isHitGround)
                machine.ChangeState(stIdleGround);
            else
                machine.ChangeState(stFreeFall);
        }
    }

    private void Logic_TakeDown()
    {
        if(leftTakeDownAirIdleFrame > 0)
        {
            DisableGravity();
            leftTakeDownAirIdleFrame--;

            SetVelocityXY(0.0f, 0.0f);

            if(leftTakeDownAirIdleFrame == 0)
                isTakeDownAirIdleEnded = true;
        }
        else if(isTakeDownAirIdleEnded && !isLandingAfterTakeDown)
        {
            EnableGravity();
            SetVelocityXY(0.0f, -takeDownSpeed);

            if(isHitGround)
            {
                leftTakeDownLandingIdleFrame = takeDownLandingIdleFrame;
                isLandingAfterTakeDown = true;
            }
        }
        else if(leftTakeDownLandingIdleFrame > 0)
        {
            DisableGravity();
            leftTakeDownLandingIdleFrame--;

            SetVelocityXY(0.0f, 0.0f);

            // TODO: 부서지는 바닥 탐지를 이 곳에서 수행하고, 바닥을 부순다.
        }
    }

    private void End_TakeDown()
    {
        // rigid.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
    }
    #endregion

    #region Implement State; stJumpWall
    private void Enter_JumpWall()
    {
        EnableGravity();
        leftJumpWallForceFrame = jumpWallForceFrame;
        leftJumpWallFrame = jumpWallFrame;
        jumpWallLookDir = -lookDir;
        isJumpWallCanceledX = false;
        isJumpWallCanceledXY = false;
    }

    private void Input_JumpWall()
    {
        if(leftJumpWallForceFrame == 0)
        {
            // 상태 기계의 상태변경을 위한 제어 함수
            if(inputData.jumpDown)
            {
                if(inputData.yNegative != 0 && !isDetectedGround)
                    machine.ChangeState(stTakeDown);
                else if(leftJumpAirCount > 0)
                    machine.ChangeState(stJumpAir);
            }
            else if(inputData.dashDown && leftDashCount > 0)
            {
                machine.ChangeState(stDash);
            }
            else if(isHitCeil || leftJumpWallFrame == 0)
            {
                if(inputData.yPositive == 0)
                    machine.ChangeState(stFreeFall);
                else
                    machine.ChangeState(stGliding);
            }
            else if(inputData.xInput == lookDir)
            {
                if(isHitLedgeHead)
                    machine.ChangeState(stLedgeClimbHead);
                else if(isHitLedgeBody)
                    machine.ChangeState(stLedgeClimbBody);
                else if(!isDetectedGround && inputData.yNegative == 0 && isHitWallB == lookDir && isHitWallT == lookDir)
                    machine.ChangeState(stIdleWall);
            }

            // 내부 상태의 변경을 위한 제어 함수
            if(inputData.jumpUp && !isJumpWallCanceledXY)
            {
                isJumpWallCanceledXY = true;
                leftJumpWallFrame /= 2;
            }
            if(inputData.xInput != 0 && !isJumpWallCanceledX)
            {
                isJumpWallCanceledXY = true;
            }
        }
    }

    private void Logic_JumpWall()
    {
        if(leftJumpWallForceFrame > 0)
            leftJumpWallForceFrame--;

        if(leftJumpWallFrame > 0)
        {
            leftJumpWallFrame--;

            vx = (isJumpWallCanceledX ? GetMoveSpeed() * inputData.xInput : jumpWallSpeedX * jumpWallGraphX[leftJumpWallFrame] * jumpWallLookDir);
            vy = jumpWallSpeedY * jumpWallGraphY[leftJumpWallFrame];

            SetVelocityXY(vx, vy);
        }
    }
    #endregion

    #region FILE_INPUT
    public void ApplyFile()
    {
        string path = Application.persistentDataPath + "/DataTable.txt";

        Action loadFunction = () =>
        {
            this.LoadDataTable(path);
        };

        Thread loadThread = new Thread(new ThreadStart(loadFunction));

        try
        {
            loadThread.Start();
        }
        catch(Exception)
        {
            UnityWinAPI.Exit();
        }

        transform.position = initialPosition;
    }

    public void OpenExplorer()
    {
        string path = Application.persistentDataPath.Replace("/", "\\");
        UnityWinAPI.OpenExplorer(path);
    }
    #endregion
}