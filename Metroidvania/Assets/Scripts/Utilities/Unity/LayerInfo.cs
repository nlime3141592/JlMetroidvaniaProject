using UnityEngine;

public static class LayerInfo
{
    public static int ground { get; private set; }
    public static int groundMask { get; private set; }

    public static int semiGround { get; private set; }
    public static int semiGroundMask { get; private set; }

    public static int entity { get; private set; }
    public static int entityMask { get; private set; }

    static LayerInfo()
    {
        ground = LayerMask.NameToLayer("Ground");
        groundMask = 1 << ground;

        semiGround = LayerMask.NameToLayer("SemiGround");
        semiGroundMask = 1 << semiGround;

        entity = LayerMask.NameToLayer("Entity");
        entityMask = 1 << entity;
    }
}