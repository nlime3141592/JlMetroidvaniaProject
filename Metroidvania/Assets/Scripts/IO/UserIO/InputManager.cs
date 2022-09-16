using UnityEngine;

// InputManager 클래스의 존재 의의
// InputHandler와 InputBuffer는 static 클래스로, 상속이 불가능하다.
// MonoBehaviour를 상속한 개체만이 GameObject에 Component로 활용 가능하기 때문에,
// InputHandler와 InputBuffer의 static 속성을 유지하며 Update 및 FixedUpdate 함수를 수행하기 위함에
// 본 클래스의 존재 의의가 있다.
public sealed class InputManager : MonoBehaviour
{
    private void Update()
    {
        InputHandler.Update();
    }

    private void FixedUpdate()
    {
        InputBuffer.FixedUpdate();
    }
}