using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CSCS
{
    public class CscsScriptingController : MonoBehaviour
    {
        public GameObject UnityEntityPrefab;
        static ConcurrentQueue<ScriptCommand> m_scriptQueue =new ConcurrentQueue <ScriptCommand>();
        static AutoResetEvent m_ScriptLoopEvent = new AutoResetEvent (false);
        static AutoResetEvent m_ScriptQuitEvent = new AutoResetEvent (false);
        static ConcurrentQueue<Action> m_actionQueue = new ConcurrentQueue<Action>();

        void Awake()
        {
            SplitAndMerge.Interpreter.Instance.Init();
            SplitAndMerge.DebuggerServer.StartServer(13337);
            OnStartup();
        }

        public void OnDestroy()
        {
            OnShutdown();
        }

        public void OnStartup()
        {
            CscsFunctions.DefineScriptFunctions(UnityEntityPrefab);
            Task.Run(() => { RunScriptingEngineThread(); });
            ExecuteScript("Assets/Scripts/cscs_scripts/Test.cscs");

        }
        public void OnShutdown()
        {
            m_ScriptQuitEvent.Set();
            m_ScriptLoopEvent.Set();
        }
        public static void ExecuteScript(string scriptFile)
        {
            if (File.Exists(scriptFile))
            {
                string sCode = "include(\"" + scriptFile + "\");";
                AddScriptToQueue( sCode );
            }
        }
        public static void AddScriptToQueue( string sCode )
        {
            ScriptCommand command = new ScriptCommand(sCode);
            m_scriptQueue.Enqueue(command);
        }
        public void Update()
        {
            m_ScriptLoopEvent.Set();
            while (m_actionQueue.Count != 0)
            {
                Action action;
                if (m_actionQueue.TryDequeue(out action)) {
                    action.Invoke();
                }
            }
        }
        public static void ExecuteInUpdate(Action action)
        {
            m_actionQueue.Enqueue(action);
        }
        
        public static void RunScriptingEngineThread()
        {
            while (!m_ScriptQuitEvent.WaitOne(0)) {
                if (SplitAndMerge.DebuggerServer.DebuggerAttached) 
                {
                    SplitAndMerge.DebuggerServer.ProcessQueue();
                }
                while (m_scriptQueue.Count != 0) {
                    ScriptCommand next;
                    
                    if (m_scriptQueue.TryDequeue(out next))
                    {
                        next.Execute();
                    }
                }
                m_ScriptLoopEvent.WaitOne(500);
            }
        }
    }
}