namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public class GenericValueWrapper < T >
{
    public T Value { get; set; }

    #region Public

    public GenericValueWrapper( T value )
    {
        Value = value;
    }

    public override string ToString()
    {
        return $"{nameof( Value )}: {Value}";
    }

    #endregion
}

}
