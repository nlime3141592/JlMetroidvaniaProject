public static class ManagerHub
{
    private static MapManager m_mapManager;
    private static PlayerSceneManager m_plscManager;

    public static MapManager GetMapManager() => m_mapManager;
    public static PlayerSceneManager GetPlayerSceneManager() => m_plscManager;

    public static void SetMapManager(MapManager manager) => m_mapManager = manager;
    public static void SetPlayerSceneManager(PlayerSceneManager manager) => m_plscManager = manager;
}