#define UNITY_ASSERTIONS

#nullable enable

using System;

using UnityEngine;

namespace JlMetroidvaniaProject.Utility
{
    public static class DebugUtility
    {
        /// <summary>
        /// 코드 흐름 중간에 이 함수를 삽입하면, Unity Editor 상에서 코드의 흐름을 추적할 수 있습니다.
        /// 오류가 없는 코드의 코드 실행 중 값 확인 목적으로만 사용하도록 합니다.
        /// </summary>
        /// <param name="format">문자열 형식</param>
        /// <param name="args">형식 인수</param>
        /// <exception cref="ArgumentNullException">format == null 또는 args == null</exception>
        /// <exception cref="FormatException">format의 형식이 잘못되거나, format과 args의 인수 갯수가 다른 경우</exception>
        public static void UnityLog(string format, params object?[] args)
        {
            try
            {
                UnityEngine.Debug.Log(string.Format(format, args));
            }
            catch(ArgumentNullException)
            {
                UnityEngine.Debug.Assert(false, "format or args cannot be null.");
            }
            catch(FormatException)
            {
                UnityEngine.Debug.Assert(false, "invalid format. please match count of format and args each contains.");
            }
        }

        /// <summary>
        /// 코드 흐름 중간에 이 함수를 삽입하면, Unity Editor 상에서 코드의 흐름을 추적할 수 있습니다.
        /// 수정하지 않아도 큰 문제가 없는 코드 로직 오류를 표기하려면 이 코드를 삽입하세요.
        /// </summary>
        /// <param name="format">문자열 형식</param>
        /// <param name="args">형식 인수</param>
        /// <exception cref="ArgumentNullException">format == null 또는 args == null</exception>
        /// <exception cref="FormatException">format의 형식이 잘못되거나, format과 args의 인수 갯수가 다른 경우</exception>
        public static void UnityLogWarning(string format, params object?[] args)
        {
            try
            {
                UnityEngine.Debug.LogWarning(string.Format(format, args));
            }
            catch(ArgumentNullException)
            {
                UnityEngine.Debug.Assert(false, "format or args cannot be null.");
            }
            catch(FormatException)
            {
                UnityEngine.Debug.Assert(false, "invalid format. please match count of format and args each contains.");
            }
        }

        /// <summary>
        /// 코드 흐름 중간에 이 함수를 삽입하면, Unity Editor 상에서 코드의 흐름을 추적할 수 있습니다.
        /// 반드시 고쳐야 하는 코드 로직 오류를 표기하려면 이 코드를 삽입하세요.
        /// </summary>
        /// <param name="format">문자열 형식</param>
        /// <param name="args">형식 인수</param>
        /// <exception cref="ArgumentNullException">format == null 또는 args == null</exception>
        /// <exception cref="FormatException">format의 형식이 잘못되거나, format과 args의 인수 갯수가 다른 경우</exception>
        public static void UnityAssert(string format, params object?[] args)
        {
            try
            {
                UnityEngine.Debug.Assert(false, string.Format(format, args));
            }
            catch(ArgumentNullException)
            {
                UnityEngine.Debug.Assert(false, "format or args cannot be null.");
            }
            catch(FormatException)
            {
                UnityEngine.Debug.Assert(false, "invalid format. please match count of format and args each contains.");
            }
        }
    }
}