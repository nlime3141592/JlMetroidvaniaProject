using UnityEngine;

public class MapManager : Manager
{
    public PolygonCollider2D virtualCameraBoundary;

    protected override void SetManager()
    {
        ManagerHub.SetMapManager(this);
    }

    protected override void ResetManager()
    {
        ManagerHub.SetMapManager(null);
    }
}