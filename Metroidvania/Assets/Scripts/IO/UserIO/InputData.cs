// 모든 입력 정보를 기록하는 구조체이다.
// InputHandler 에서 입력 값을 인식하고 InputBuffer에서 입력 값을 저장하는데,
// 두 클래스 간 상호작용에서 속성의 통일성을 유지하기 위해 존재하는 구조체이다.
public struct InputData
{
    public bool xNegDown;
    public bool xNegUp;
    public bool xPosDown;
    public bool xPosUp;
    public int xNegative;
    public int xPositive;
    public int xInput;

    public bool yNegDown;
    public bool yNegUp;
    public bool yPosDown;
    public bool yPosUp;
    public int yNegative;
    public int yPositive;
    public int yInput;

    public bool jumpDown;
    public bool jumpUp;
    public bool jumpPressing;

    public bool dashDown;
    public bool dashUp;
    public bool dashPressing;

    public void Copy(InputData from)
    {
        xNegDown = from.xNegDown;
        xNegUp = from.xNegUp;
        xPosDown = from.xPosDown;
        xPosUp = from.xPosUp;
        xNegative = from.xNegative;
        xPositive = from.xPositive;
        xInput = from.xInput;

        yNegDown = from.yNegDown;
        yNegUp = from.yNegUp;
        yPosDown = from.yPosDown;
        yPosUp = from.yPosUp;
        yNegative = from.yNegative;
        yPositive = from.yPositive;
        yInput = from.yInput;

        jumpDown = from.jumpDown;
        jumpUp = from.jumpUp;
        jumpPressing = from.jumpPressing;

        dashDown = from.dashDown;
        dashUp = from.dashUp;
        dashPressing = from.dashPressing;
    }
}