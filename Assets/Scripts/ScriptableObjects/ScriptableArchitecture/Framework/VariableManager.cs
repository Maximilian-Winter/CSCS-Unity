using System.Collections.Generic;
using ScriptableObjects.ScriptableArchitecture.Framework;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "varManager", menuName = "Variables/Variable Manager")]
public class VariableManager : ScriptableObject
{
	public List<ScriptableBase> Variables = new List<ScriptableBase>();
#if UNITY_EDITOR
	public void RefreshList()
	{
		Variables = new List<ScriptableBase>(GetAllVariables());
	}

	// This method works only in Editor
	private static ScriptableBase[] GetAllVariables()
	{
		string[] guids = AssetDatabase.FindAssets("t:ScriptableBase");
		ScriptableBase[] vars = new ScriptableBase[guids.Length];
		for (int i = 0; i < guids.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			vars[i] = AssetDatabase.LoadAssetAtPath<ScriptableBase>(path);
			if (string.IsNullOrEmpty(vars[i].Guid))
			{
				vars[i].Guid = guids[i];
			}
		}
		return vars;
	}
#endif
}