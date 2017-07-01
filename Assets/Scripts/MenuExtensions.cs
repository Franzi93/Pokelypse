using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuExtensions : MonoBehaviour {
    [MenuItem("Poke/AddAttack")]
    static void DoSomething()
    {
         Attack asset = ScriptableObject.CreateInstance<Attack>();

        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    
}

