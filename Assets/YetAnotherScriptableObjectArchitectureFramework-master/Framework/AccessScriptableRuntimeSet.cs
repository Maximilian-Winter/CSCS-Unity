using System.Collections.Generic;
using OdinSerializer;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{
    public class AccessScriptableRuntimeSet  <T, V> : SerializedMonoBehaviour  where T : RuntimeSet<V>
    {
        public T ScriptableRuntimeSet;
        public bool InstantiateLocalRuntimeSetOnEnable;
        public bool CopyRuntimeSetObjectsToLocalRuntimeSetOnEnable;
        public bool IsInitialized = false;
        protected virtual void OnEnable()
        {
            if (InstantiateLocalRuntimeSetOnEnable && IsInitialized == false)
            {
                if ( CopyRuntimeSetObjectsToLocalRuntimeSetOnEnable )
                {
                    List<V> oldRuntimeSet = new List < V >();
                    ScriptableRuntimeSet.CopyTo( oldRuntimeSet );
                    ScriptableRuntimeSet = ScriptableObject.CreateInstance<T>();
                    ScriptableRuntimeSet.Items = new List < V >();
                    ScriptableRuntimeSet.Items = oldRuntimeSet;
                }
                else
                {
                    ScriptableRuntimeSet = ScriptableObject.CreateInstance<T>();
                }
            }

            IsInitialized = true;
        }

        public void AddObject(V go)
        {
            ScriptableRuntimeSet.Items.Add(go);
        }
    
        public void RemoveObject(V go)
        {
            ScriptableRuntimeSet.Items.Remove(go);
        }
    
        public List<V> GetAllObjects()
        {
            return ScriptableRuntimeSet.Items;
        }
    }
}