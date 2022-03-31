namespace JlMetroidvaniaProject.Datas
{
    /// <summary>
    /// 상태 테이블을 만들어서 관리할 수 있는 클래스입니다.
    /// </summary>
    public abstract class StateBase
    {
        /// <summary>
        /// 상태 테이블을 갱신합니다.
        /// </summary>
        public abstract void Update();
    }
}