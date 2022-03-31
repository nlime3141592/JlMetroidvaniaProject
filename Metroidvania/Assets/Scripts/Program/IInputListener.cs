namespace JlMetroidvaniaProject
{
    public interface IInputListener
    {
        bool canListenInput { get; }
        void Ignore();
        void Listen();
    }
}