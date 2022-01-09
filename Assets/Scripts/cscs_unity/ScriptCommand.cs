using System;
using UnityEngine;

namespace CSCS
{
    public struct ScriptCommand
    {
        public string command;
        public SplitAndMerge.Variable result;
        public string output;
        public string errorMessage;

        public ScriptCommand(string sCommand)
        {
            command = sCommand;
            result = null;
            output = "";
            errorMessage = "";
        }
        
        public void Execute()
        {
            output = "";
            try
            {
                result = SplitAndMerge.Interpreter.Instance.Process(command);
                output = SplitAndMerge.Interpreter.Instance.Output;
                
                errorMessage = "";
            }
            catch (Exception exception) {
                errorMessage = exception.Message;
                SplitAndMerge.ParserFunction.InvalidateStacksAfterLevel(0);
            }
        }
    }
}