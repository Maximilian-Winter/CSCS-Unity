using System.Collections.Generic;

namespace Utility
{

public class GenericValueWrapper<T>
{
    public T Value { get; set; }
    public GenericValueWrapper(T value) { this.Value = value; }
    
    public override string ToString()
    {
        return $"{nameof( Value )}: {Value}";
    }
}

}
