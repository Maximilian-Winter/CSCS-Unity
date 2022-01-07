using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SplitAndMerge;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class GameObjectProxy : ScriptObject
    {
        private static readonly List<string> s_properties = new()
        {
            "transform", "layer", "active", "activeSelf", "activeInHierarchy", "isStatic", "tag", "scene",
            "sceneCullingMask", "gameObject", "rigidbody", "rigidbody2D", "camera", "light", "animation",
            "constantForce", "renderer", "audio", "networkView", "collider", "collider2D", "hingeJoint",
            "particleSystem", "name", "hideFlags",
        };

        public List<string> GetProperties()
        {
            return s_properties;
        }

        public Task<Variable> SetProperty(string name, Variable value)
        {
            var newValue = Variable.EmptyInstance;
            var mre = new ManualResetEvent(false);

            CscsScriptingController.ExecuteInUpdate(() =>
            {
                switch (name)
                {
                    case "transform":
                        break;
                    case "layer":
                        break;
                    case "active":
                        break;
                    case "activeSelf":
                        break;
                    case "activeInHierarchy":
                        break;
                    case "isStatic":
                        break;
                    case "tag":
                        break;
                    case "scene":
                        break;
                    case "sceneCullingMask":
                        break;
                    case "gameObject":
                        break;
                    case "rigidbody":
                        break;
                    case "rigidbody2D":
                        break;
                    case "camera":
                        break;
                    case "light":
                        break;
                    case "animation":
                        break;
                    case "constantForce":
                        break;
                    case "renderer":
                        break;
                    case "audio":
                        break;
                    case "networkView":
                        break;
                    case "collider":
                        break;
                    case "collider2D":
                        break;
                    case "hingeJoint":
                        break;
                    case "particleSystem":
                        break;
                    case "name":
                        break;
                    case "hideFlags":
                        break;
                }

                mre.Set();
            });

            mre.WaitOne();
            return Task.FromResult(newValue);
        }

        public Task<Variable> GetProperty(string sPropertyName,
            List<Variable> args = null, ParsingScript script = null)
        {
            var newValue = Variable.EmptyInstance;
            var mre = new ManualResetEvent(false);

            CscsScriptingController.ExecuteInUpdate(() =>
            {
                switch (sPropertyName)
                {
                    case "transform":
                        break;
                    case "layer":
                        break;
                    case "active":
                        break;
                    case "activeSelf":
                        break;
                    case "activeInHierarchy":
                        break;
                    case "isStatic":
                        break;
                    case "tag":
                        break;
                    case "scene":
                        break;
                    case "sceneCullingMask":
                        break;
                    case "gameObject":
                        break;
                    case "rigidbody":
                        break;
                    case "rigidbody2D":
                        break;
                    case "camera":
                        break;
                    case "light":
                        break;
                    case "animation":
                        break;
                    case "constantForce":
                        break;
                    case "renderer":
                        break;
                    case "audio":
                        break;
                    case "networkView":
                        break;
                    case "collider":
                        break;
                    case "collider2D":
                        break;
                    case "hingeJoint":
                        break;
                    case "particleSystem":
                        break;
                    case "name":
                        break;
                    case "hideFlags":
                        break;
                }

                mre.Set();
            });

            mre.WaitOne();
            return Task.FromResult(newValue);
        }
    }
}


namespace CSCS
{
    public class ScriptObjectProxyTemplate : ScriptObject
    {
        private static readonly List<string> s_properties = new()
        {
            "debuglog", "position", "rotation", "scale", "translate"
        };

        public Task<Variable> SetProperty(string name, Variable value)
        {
            var newValue = Variable.EmptyInstance;
            var mre = new ManualResetEvent(false);

            CscsScriptingController.ExecuteInUpdate(() =>
            {
                switch (name)
                {
                    case "debuglog":
                        break;
                    case "position":
                        break;
                    case "rotation":
                        break;
                    case "scale":
                        break;
                    case "translate":
                        break;
                }

                mre.Set();
            });

            mre.WaitOne();
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
                    case "debuglog":
                        break;
                    case "position":
                        break;
                    case "rotation":
                        break;
                    case "scale":
                        break;
                    case "translate":
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

namespace CSCS
{
    public class CodeGenerator : MonoBehaviour
    {
        public static string GenerateCode(Type classToGenerateCodeFrom)
        {
            string test;
            // Get the public properties, fields and methods.
            PropertyInfo[] publicProperties =
                classToGenerateCodeFrom.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            MethodInfo[] publicMethods =
                classToGenerateCodeFrom.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            
            FieldInfo[] fields = classToGenerateCodeFrom.GetFields();
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public class " + classToGenerateCodeFrom.Name + @"Proxy : ScriptObject");
            sb.AppendLine("{");
            sb.AppendLine("private static readonly List<string> s_properties = new()");
            sb.AppendLine("{");
            foreach (PropertyInfo p in publicProperties)
            {
                sb.Append( "\"" + p.Name + "\" ,");
            }
            
            foreach (MethodInfo p in publicMethods)
            {
                if (p.GetGenericArguments().Length == 0)
                {
                    sb.Append( "\"" + p.Name + "\" ,");
                }
            }
            sb.AppendLine("};");
            sb.AppendLine();
            sb.AppendLine("public List<string> GetProperties()");
            sb.AppendLine(@" {
                                return s_properties;
                             }");
            sb.AppendLine(@" public Task<Variable> SetProperty(string name, Variable value)
                                {
                                    var newValue = Variable.EmptyInstance;
                                    var mre = new ManualResetEvent(false);
                                    CscsScriptingController.ExecuteInUpdate(() =>
                                    {
                                        switch (name)
                                        {
                                                ");
            foreach (PropertyInfo p in publicProperties)
            {
                sb.AppendLine("case \"" + p.Name + "\":\n break;\n");
            }

            foreach (MethodInfo p in publicMethods)
            {
                if (p.GetGenericArguments().Length == 0)
                {
                    sb.AppendLine( "\"" + p.Name + "\" ,");
                }
            }
            
            sb.AppendLine(@"}
                                    mre.Set();
                                });

                                mre.WaitOne();
                                return Task.FromResult(newValue);
                            }");
            sb.AppendLine(@"
                            public Task<Variable> GetProperty(string sPropertyName,
                                List<Variable> args = null, ParsingScript script = null)
                            {
                                var newValue = Variable.EmptyInstance;
                                var mre = new ManualResetEvent(false);

                                CscsScriptingController.ExecuteInUpdate(() =>
                                {
                                    switch (sPropertyName)
                                    {
                                       ");
            
            foreach (PropertyInfo p in publicProperties)
            {
                sb.AppendLine("case \"" + p.Name + "\":\n break;\n");
            }
            
            foreach (MethodInfo p in publicMethods)
            {
                if (p.GetGenericArguments().Length == 0)
                {
                    sb.AppendLine("\"" + p.Name + "\" ,");
                }
            }
            
            sb.AppendLine(@"}

                                                mre.Set();
                                            });

                                            mre.WaitOne();
                                            return Task.FromResult(newValue);
                                        }
                            }");
            return sb.ToString();
        }

        [ContextMenu("GenerateCode")]
        public void StartCodeGeneration()
        {
            Debug.Log(GenerateCode(typeof(Component)));
        }
    }
}