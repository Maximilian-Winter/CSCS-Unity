namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public enum AreaMasks : int
{
    WalkableMask = 1 << AreaBits.Walkable,
    NotWalkableMask = 1 << AreaBits.NotWalkable,
    JumpMask = 1 << AreaBits.Jump,
}

}
