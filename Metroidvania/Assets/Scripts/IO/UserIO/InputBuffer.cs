using System;
using System.Collections.Generic;
using UnityEngine;

public static class InputBuffer
{
    // 혹시 모를 시스템 변동 및 확장을 대비해 60프레임 이전의 정보까지 저장할 수 있도록 배열을 만듦.
    public const int bufferCount = 60;
    private static readonly InputData[] c_buffers = new InputData[bufferCount];

    // 현재 프레임 카운트, bufferCount를 넘어가면 나머지 연산을 통해 다시 0으로 돌림.
    private static int currentFrame = 0;

    // InputManager 클래스에서 수행함.
    public static void FixedUpdate()
    {
        c_buffers[currentFrame].Copy(InputHandler.data);

        currentFrame++;
        currentFrame %= bufferCount;
    }

    // NOTE: 이전 프레임의 InputBuffer의 Data를 불러옵니다.
    // 기본 제공하는 나머지 연산(% 연산자)은 음수에서 오류를 발생할 여지가 있음.
    // 따라서, a%p = a라 가정하면, (p+a)%p = a임을 이용함.
    // 즉, currentFrame에 bufferCount를 더해서 bufferCount로 나머지 연산함.
    // backframe을 빼는 것은, currentFrame으로부터 몇 프레임 이전의 input 값을 얻기 위함.
    // 따라서, backframe은 0이상 bufferCount 미만이어야 함.
    // 0 <= backframe < bufferCount
    public static InputData GetBufferedData(int backframe)
    {
        int index = (bufferCount + currentFrame - backframe) % bufferCount;
        return c_buffers[index];
    }
}