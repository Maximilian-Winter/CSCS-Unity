namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public class KeyVal < Key, Val >
{
    public Key Id { get; set; }

    public Val Value { get; set; }

    #region Public

    public KeyVal()
    {
    }

    public KeyVal( Key key, Val val )
    {
        Id = key;
        Value = val;
    }

    #endregion
}

}
