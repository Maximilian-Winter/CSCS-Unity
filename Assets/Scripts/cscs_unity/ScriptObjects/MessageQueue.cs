using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSCS;
using SplitAndMerge;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{

public class MessageQueue : Queue<Message>, ScriptObject
{
    private static readonly List<string> s_properties = new()
    {
        "DequeueMessage"
    };

    public Task<Variable> SetProperty(string name, Variable value)
    {
        var newValue = Variable.EmptyInstance;
        /*switch (name)
        {
           default:
              break;
        }*/
        return Task.FromResult(newValue);
    }

    public Task<Variable> GetProperty(string name, List<Variable> args = null, ParsingScript script = null)
    {
        var newValue = Variable.EmptyInstance;
        var mre = new ManualResetEvent(false);
        CscsScriptingController.ExecuteInUpdate(() =>
        {
            switch (name)
            { 
                case "DequeueMessage":
                    if ( script != null )
                    {
                        newValue = new Variable(Dequeue());
                    }
                    break;
                default:
                    break;
            }

            mre.Set();
        });

        mre.WaitOne();
              
        return Task.FromResult(newValue);
    }

    public List<string> GetProperties()
    {
        return s_properties;
    }
}

}
