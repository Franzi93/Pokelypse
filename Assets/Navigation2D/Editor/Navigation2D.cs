﻿// Navigation2D Script (c) noobtuts.com
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Navigation2D : EditorWindow
{
    // options
    float navmeshExtends = 1;
    static int showNavMesh = 1; // 0 = hide, 1 = wireframe, 2 = full

    bool IsValidCollider(Collider2D co)
    {
        // usable for navmesh generation if not trigger and if navigation static
        bool navstatic = GameObjectUtility.AreStaticEditorFlagsSet(co.gameObject, StaticEditorFlags.NavigationStatic);
        return navstatic && co.enabled && !co.isTrigger;
    }

    // set area as not walkable so that huge objects dont get walkable areas
    // inside of them
    void MakeUnwalkable(GameObject go)
    {
        int layer = GameObjectUtility.GetNavMeshAreaFromName("Not Walkable");
        GameObjectUtility.SetNavMeshArea(go, layer);
    }

    void AddBoxCollider2Ds(Transform parent)
    {
        // find all valid colliders, add them to projection
        var colliders = GameObject.FindObjectsOfType<BoxCollider2D>();
        var filtered = colliders.Where(co => IsValidCollider(co)).ToList();
        foreach (var collider in filtered)
        {
            // note: creating a primitive is necessary in order for it to bake properly
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.isStatic = true;
            go.transform.parent = parent;
            // position via offset and transformpoint
            var localPos = new Vector3(collider.offset.x, collider.offset.y, 0);
            var worldPos = collider.transform.TransformPoint(localPos);
            go.transform.position = new Vector3(worldPos.x, 0, worldPos.y);
            // scale depending on scale * collider size (circle=radius/box=size/...)
            go.transform.localScale = NavMeshUtils2D.ScaleFromBoxCollider2D(collider);
            // rotation
            go.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(collider.transform.eulerAngles));

            MakeUnwalkable(go);
        }
    }

    void AddCircleCollider2Ds(Transform parent)
    {
        // find all valid colliders, add them to projection
        var colliders = GameObject.FindObjectsOfType<CircleCollider2D>();
        var filtered = colliders.Where(co => IsValidCollider(co)).ToList();
        foreach (var collider in filtered)
        {
            // note: creating a primitive is necessary in order for it to bake properly
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.isStatic = true;
            go.transform.parent = parent;
            // position via offset and transformpoint
            var localPos = new Vector3(collider.offset.x, collider.offset.y, 0);
            var worldPos = collider.transform.TransformPoint(localPos);
            go.transform.position = new Vector3(worldPos.x, 0, worldPos.y);
            // scale depending on scale * collider size (circle=radius/box=size/...)
            go.transform.localScale = NavMeshUtils2D.ScaleFromCircleCollider2D(collider);
            // rotation
            go.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(collider.transform.eulerAngles));

            MakeUnwalkable(go);
        }
    }

    void AddPolygonCollider2Ds(Transform parent)
    {
        // find all valid colliders, add them to projection
        var colliders = GameObject.FindObjectsOfType<PolygonCollider2D>();
        var filtered = colliders.Where(co => IsValidCollider(co)).ToList();
        foreach (var collider in filtered)
        {
            // note: creating a primitive is necessary in order for it to bake properly
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.isStatic = true;
            go.transform.parent = parent;

            // position via offset and transformpoint
            var localPos = new Vector3(collider.offset.x, collider.offset.y, 0);
            var worldPos = collider.transform.TransformPoint(localPos);
            go.transform.position = new Vector3(worldPos.x, 0, worldPos.y);
            // scale depending on scale * collider size (circle=radius/box=size/...)
            go.transform.localScale = NavMeshUtils2D.ScaleFromPolygonCollider2D(collider);
            // rotation
            go.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(collider.transform.eulerAngles));

            // remove box collider. note that baking uses the meshfilter, so
            // the collider doesn't really matter anyway.
            DestroyImmediate(go.GetComponent<BoxCollider>());

            // Use the triangulator to get indices for creating triangles
            int[] indices = Triangulation.Triangulate(collider.points.ToList()).ToArray();

            // convert vector2 points to vector3 vertices
            var vertices = collider.points.Select(p => new Vector3(p.x, 0, p.y)).ToList();

            // create mesh
            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices;
            //mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // assign it to the mesh filter
            go.GetComponent<MeshFilter>().sharedMesh = mesh;

            MakeUnwalkable(go);
        }
    }

    void BakeNavMesh2D()
    {
        // create a temporary parent GameObject
        var obj = new GameObject();

        // find all static box colliders, add them to projection
        AddBoxCollider2Ds(obj.transform);
        // find all static circle colliders, add them to projection
        AddCircleCollider2Ds(obj.transform);
        // find all static polygon colliders, add them to projection
        AddPolygonCollider2Ds(obj.transform);

        // min and max point from 2D colliders projected to 3D.
        // (scanning through 3D colliders doesn't work well because the polygon
        //  GameObjects are pure meshes without colliders)
        var cols = GameObject.FindObjectsOfType<Collider2D>();
        if (cols.Length > 0)
        {
            var min = new Vector2(Mathf.Infinity, Mathf.Infinity);
            var max = -min;
            foreach (var c in cols)
            {
                var minmax = NavMeshUtils2D.AdjustMinMax(c, min, max);
                min = minmax[0];
                max = minmax[1];
            }

            // create ground (cube instead of plane because it has unit size)
            // (pos between min and max; scaled to fit min and max * scale)
            // note: scale.y=0 so that *navmeshExtends doesn't make it too high
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Ground"; // for debugging
            go.isStatic = true;
            go.transform.parent = obj.transform;
            float w = max.x - min.x;
            float h = max.y - min.y;
            go.transform.position = new Vector3(min.x + w/2, -0.5f, min.y + h/2);
            go.transform.localScale = new Vector3(w, 0, h) * navmeshExtends;
        }


        // bake navmesh asynchronously, clear mesh
        UnityEditor.AI.NavMeshBuilder.BuildNavMeshAsync(); // Async causes weird results
        if (gizmesh != null) gizmesh.Clear();
        needsRebuild = true; // rebuild as soon as async baking is finished

        // delete the gameobjects now that the path was created
        GameObject.DestroyImmediate(obj);
    }

    // editor window ///////////////////////////////////////////////////////////
    [MenuItem("Window/Navigation2D")]
    public static void ShowWindow()
    {
        // Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(Navigation2D));
    }

    static Texture2D logo
    {
        get
        {
            return (Texture2D)EditorGUIUtility.Load("Assets/Navigation2D/logo.png");
        }
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();

        // colored logo
        //var backup = GUI.backgroundColor;
        //GUI.backgroundColor = Color.white;
        //GUILayout.BeginVertical("box");
        GUILayout.Label(logo, new GUIStyle(GUI.skin.label){alignment=TextAnchor.MiddleCenter});
        //GUILayout.EndVertical();
        //GUI.backgroundColor = backup;
        GUILayout.Label("<b>by noobtuts.com</b>", new GUIStyle(GUI.skin.label){richText=true,alignment=TextAnchor.MiddleCenter});

        // instructions
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Instructions", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(@"1. Make your 2D Colliders Static
2. Press Bake and wait until it's done
3. Add NavMeshAgent2D to agents", MessageType.Info);
        EditorGUILayout.Space();

        // get access to original navigation settings
        // (that's how Unity's Navigation window does it too)
        var settings = new SerializedObject(UnityEditor.AI.NavMeshBuilder.navMeshSettingsObject);
        SerializedProperty agentRadius = settings.FindProperty("m_BuildSettings.agentRadius");
        SerializedProperty agentHeight = settings.FindProperty("m_BuildSettings.agentHeight");

        // settings header
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

        // diagram
        Rect controlRect = EditorGUILayout.GetControlRect(false, 120f, new GUILayoutOption[0]);
        NavMeshEditorHelpers.DrawAgentDiagram(controlRect, agentRadius.floatValue, agentHeight.floatValue, 0, 0);

        // agent radius
        agentRadius.floatValue = EditorGUILayout.FloatField(new GUIContent("Agent Radius", "Modifies the built in Agent Radius from Window->Navigation"), agentRadius.floatValue);
        settings.ApplyModifiedProperties();

        // ground extends
        navmeshExtends = EditorGUILayout.Slider(new GUIContent("NavMesh Extends", "Can be used to cover the outside of your scene with a NavMesh"), navmeshExtends, 1, 100);

        // visibility
        showNavMesh = EditorGUILayout.IntPopup("Show Navmesh", showNavMesh, new string[]{"Hide", "Wireframe", "Full"}, new int[]{0, 1, 2});

        // repaint scene if showNavMesh option changed
        if (GUI.changed)
            SceneView.RepaintAll();

        // buttons
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Clear", GUILayout.Width(95f)))
        {
            UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
            if (gizmesh != null) gizmesh.Clear();
        }
        if (UnityEditor.AI.NavMeshBuilder.isRunning)
        {
            if (GUILayout.Button("Cancel", GUILayout.Width(95f))) UnityEditor.AI.NavMeshBuilder.Cancel();
        }
        else
        {
            if (GUILayout.Button("Bake", GUILayout.Width(95f))) BakeNavMesh2D();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    // save & load editor settings /////////////////////////////////////////////
    void LoadSettings()
    {
        if (EditorPrefs.HasKey("Navigation2D_navmeshExtends"))
            navmeshExtends = EditorPrefs.GetFloat("Navigation2D_navmeshExtends");

        if (EditorPrefs.HasKey("Navigation2D_showNavMesh"))
            showNavMesh = EditorPrefs.GetInt("Navigation2D_showNavMesh");
    }

    void SaveSettings()
    {
        EditorPrefs.SetFloat("Navigation2D_navmeshExtends", navmeshExtends);
        EditorPrefs.SetInt("Navigation2D_showNavMesh", showNavMesh);
    }

    void OnFocus()
    {
        LoadSettings();
    }

    void OnLostFocus()
    {
        SaveSettings();
    }

    void OnDestroy()
    {
        SaveSettings();
    }

    // gizmo ///////////////////////////////////////////////////////////////////
    static bool needsRebuild = false;
    static Mesh gizmesh;
    static void RebuildGizmesh(NavMeshTriangulation nm)
    {
        // the mesh is cleared after stopping the game, rebuild if necessary
        if (!gizmesh) gizmesh = new Mesh();
        gizmesh.vertices = nm.vertices.Select(v => new Vector3(v.x, v.z, 0)).ToArray(); // TODO utils?
        gizmesh.triangles = nm.indices;
        gizmesh.normals = gizmesh.vertices.Select(_ => new Vector3(0, 0, -1)).ToArray();
        needsRebuild = false;
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void OnGizmo(Transform tf, GizmoType gt)
    {
        // rebuild if necessary
        if (!gizmesh || needsRebuild)
            if (!UnityEditor.AI.NavMeshBuilder.isRunning)
                RebuildGizmesh(NavMesh.CalculateTriangulation());

        // draw if not empty
        if (gizmesh.vertices.Length > 0)
        {
            Gizmos.color = Color.cyan;
            if (showNavMesh == 1)
            {
                Gizmos.DrawWireMesh(gizmesh);
            }
            else if (showNavMesh == 2)
            {
                Gizmos.DrawMesh(gizmesh);
                Gizmos.DrawWireMesh(gizmesh);
            }
        }
    }
}
