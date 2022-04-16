namespace JlMetroidvaniaProject.Utility
{
    public static class MathUtility
    {
        /// <summary>
        /// 두 실수 중 더 작은 값을 반환합니다.
        /// </summary>
        /// <param name="a">비교할 대상 1</param>
        /// <param name="b">비교할 대상 2</param>
        /// <returns>a, b 중 더 작은 값</returns>
        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// 두 실수 중 더 큰 값을 반환합니다.
        /// </summary>
        /// <param name="a">비교할 대상 1</param>
        /// <param name="b">비교할 대상 2</param>
        /// <returns>a, b 중 더 큰 값</returns>
        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }
    }
}