using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{
    [CreateAssetMenu(fileName = "ScriptableTagRuntimeSet",menuName ="Variables/ScriptableTagRuntimeSet")]
    public class ScriptableTagRuntimeSet : RuntimeSet<ScriptableTag>
    {
        public bool ScriptableTagIsInRuntimeSet(ScriptableTag scriptableTag)
        {
            foreach (ScriptableTag tag in Items)
            {
                if (tag.GetType() == scriptableTag.GetType())
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool RuntimeSetIsInRuntimeSet(RuntimeSet<ScriptableTag> scriptableTags)
        {
            bool isInRuntimeSet = true;
            foreach (ScriptableTag tag2 in scriptableTags.Items)
            {
                bool breakForEach = true;
                foreach (ScriptableTag tag in Items)
                {
                    if (tag.GetType() == tag2.GetType())
                    {
                        breakForEach = false;
                    }
                }

                if (breakForEach)
                {
                    return false;
                }
            }
            return isInRuntimeSet;
        }
        
        public bool RuntimeSetIsPartiallyInRuntimeSet(RuntimeSet<ScriptableTag> scriptableTags)
        {
            bool isInRuntimeSet = false;
            foreach (ScriptableTag tag2 in scriptableTags.Items)
            {
                foreach (ScriptableTag tag in Items)
                {
                    if (tag.GetType() == tag2.GetType())
                    {
                        isInRuntimeSet = true;
                    }
                }

                if (isInRuntimeSet)
                {
                    return isInRuntimeSet;
                }
            }
            return isInRuntimeSet;
        }
    }
}