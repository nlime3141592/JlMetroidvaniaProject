public class PlayerSceneManager : Manager
{
    public Player player;

    protected override void SetManager()
    {
        ManagerHub.SetPlayerSceneManager(this);
    }

    protected override void ResetManager()
    {
        ManagerHub.SetPlayerSceneManager(null);
    }
}