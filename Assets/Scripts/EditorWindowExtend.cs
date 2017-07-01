using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

//The editorwindow with all tool options
[System.Serializable]
public class EditorWindowExtend : EditorWindow
{
    private string itemsJsonPath = (Directory.GetCurrentDirectory() + "/Assets/Resources/items.json");
    private int tab = 0;
    private Vector2 scrollPos;


    [SerializeField]
    ItemDatabase coll;
    /// <summary>
    /// opens the question editorwindow
    /// </summary>
    [MenuItem("Poke/Window", false, 1)]
    public static void openWindow()
    {
        EditorWindowExtend window = (EditorWindowExtend)GetWindow(typeof(EditorWindowExtend), false, "edit");
        window.Show();
    }
    void OnEnable() {
        string dataAsJson = File.ReadAllText(itemsJsonPath);
        if (dataAsJson.Equals(""))
        {
            coll = new ItemDatabase();
        }
        else {
            coll = JsonUtility.FromJson<ItemDatabase>(dataAsJson);
        }

    }

    void Update()
    {
        if (coll == null) {
            File.Create(itemsJsonPath);
        }

    }

    void OnGUI()
    {
        //draws editor gui
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
        GUILayout.Space(5);
        tab = GUILayout.Toolbar(tab, new string[] { "Items", "Creatures", "Characters" });
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        if (coll != null)
        {
            switch (tab)
            {
                case 0:
                    questionnairesArea();
                    break;
                case 1:
                    outputOptions();
                    break;
                case 2:
                    settingOptions();
                    break;
                default:
                    break;
            }


        }

        GUILayout.Space(20);
        EditorGUILayout.EndScrollView();
    }


    void OnLostFocus()
    {
        string Json = JsonUtility.ToJson(coll);
        if (!File.Exists(itemsJsonPath))
        {
            File.Create(itemsJsonPath);
        }
        File.WriteAllText(itemsJsonPath, Json);
    }
    /// <summary>
    /// draws gui for output tab
    /// </summary>
    void outputOptions()
    {
     
        GUILayout.Label("-");

    }
    /// <summary>
    /// draws gui for settings tab
    /// </summary>
    void settingOptions()
    {
       
    }

    /// <summary>
    /// draws gui for questions tab
    /// </summary>
    void questionnairesArea()
    {
        EditorGUILayout.Separator();
        items();
        if (GUILayout.Button("save", GUILayout.Width(100)))
        {
            string Json = JsonUtility.ToJson(coll);
            if (!File.Exists(itemsJsonPath))
            {
                File.Create(itemsJsonPath);
            }
            File.WriteAllText(itemsJsonPath, Json);
        }
        if (GUILayout.Button("New Item", GUILayout.Width(100)))
        {
            coll.newItem();
        }
            
    }


    void items()
    {
        foreach (Item i in coll.items) {
            i.id = EditorGUILayout.IntField("id", i.id);
            i.title = EditorGUILayout.TextField("name",i.title);
            i.stackable = EditorGUILayout.Toggle("stackable",i.stackable);
            if (i.stackable) {
                i.stacksizemax = EditorGUILayout.IntField("max stack", i.stacksizemax);
            }
            GUILayout.Space(10);
        }
        
    }
}
