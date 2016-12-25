#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;

public class ScenesWindow : EditorWindow
{

    public void OnGUI()
    {
        this.titleContent = new GUIContent("All Scenes");
        GUILayout.BeginVertical();
        string[] assetguids = AssetDatabase.FindAssets("t:Scene");
        if (assetguids.Length <= 0)
        {
            EditorGUILayout.HelpBox("There is nothing to Show here..!", MessageType.Warning);
        }
        foreach (var assetguid in assetguids)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetguid);
            string assetname = path.Substring(path.LastIndexOf("/") + 1);

            if (GUILayout.Button(new GUIContent(assetname, path)))
            {

                if (!EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.OpenScene(path);
                }
                else
                {
                    if (EditorUtility.DisplayDialog("Alert", "You want to Save?", "Yes", "Cancel"))
                    {
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        EditorSceneManager.OpenScene(path);
                    }
                }
            }
        }
        GUILayout.EndVertical();
    }
}

public class BuildScenesWindow : EditorWindow
{
    public void OnGUI()
    {
        this.titleContent = new GUIContent("All Build Scenes");

        GUILayout.BeginVertical();
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        if (scenes.Length <= 0)
        {
            EditorGUILayout.HelpBox("There is nothing to Show here..!", MessageType.Warning);
            if (GUILayout.Button("Add Scene To Build Setting"))
            {
                EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
            }
        }
        foreach (var scene in scenes)
        {
            string path = scene.path;
            string name = path.Substring(path.LastIndexOf('/') + 1);
            if (GUILayout.Button(new GUIContent(name, path)))
            {
                if (!EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.OpenScene(path);
                }
                else
                {
                    if (EditorUtility.DisplayDialog("Alert", "You want to Save?", "Yes", "No"))
                    {
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        EditorSceneManager.OpenScene(path);
                    }
                }
            }
        }
        GUILayout.EndVertical();
    }
}
#endif