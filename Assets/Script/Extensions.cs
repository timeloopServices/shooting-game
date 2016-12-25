#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

public class Extensions
{
    static string[] _folders = new string[] { "AnimationClip", "AudioClip", "AudioMixer", "Font", "GUISkin", "Material", "Mesh", "Model", "PhysicsMaterial", "Prefab", "Scene", "Script", "Shader", "Sprite", "Texture" };
    static List<string> folders = new List<string>(_folders);
    static List<string> assets = new List<string>(_folders);
    static string backUpPath = @"D:\Kashyap\Unity_Backup";
    //public Extensions()
    //{
    //    folders = new List<string>();
    //    folders.Add("AnimationClip");
    //    folders.Add("AudioClip");
    //    folders.Add("AudioMixer");
    //    folders.Add("Font");
    //    folders.Add("GUISkin");
    //    folders.Add("Material");
    //    folders.Add("Mesh");
    //    folders.Add("Model");
    //    folders.Add("PhysicsMaterial");
    //    folders.Add("Prefab");
    //    folders.Add("Scene");
    //    folders.Add("Script");
    //    folders.Add("Shader");
    //    folders.Add("Sprite");
    //    folders.Add("Texture");

    //    assets = new List<string>(folders);
    //}

    /// <summary>
    /// Create all Types of Folders like Scenes, Scripts, Textures etc...
    /// </summary>
    [MenuItem("Kashyap/Create Folders")]
    public static void CreateFolders()
    {
        string[] subDirectories = AssetDatabase.GetSubFolders("Assets");
        folders.ForEach(folder =>
        {
            if (!subDirectories.Contains("Assets/" + folder))
            {
                AssetDatabase.CreateFolder("Assets", folder);
            }
        });
    }

    /// <summary>
    /// Arrange All Assets as it's type and move it to folder
    /// </summary>
    [MenuItem("Kashyap/Arrange Assets")]
    public static void ArrangeAssets()
    {
        assets.ForEach(asset =>
        {
            string[] assetguids = AssetDatabase.FindAssets("t:" + asset);
            foreach (var assetguid in assetguids)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetguid);
                string assetname = path.Substring(path.LastIndexOf("/") + 1);
                string validateMessage = AssetDatabase.ValidateMoveAsset(path, "Assets/" + asset + "/" + assetname);
                if (string.IsNullOrEmpty(validateMessage))
                {
                    string finalPath = "Assets/" + asset + "/" + assetname;
                    AssetDatabase.MoveAsset(path, finalPath);
                }
                else
                {
                    Debug.Log("Moving Error: " + validateMessage);
                }
            }
        });
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Take Backup At BackUp location
    /// </summary>
    [MenuItem("Kashyap/Backup")]
    public static void Backup()
    {
        string projectPath = Application.dataPath;
        projectPath = projectPath.Substring(0, projectPath.LastIndexOf("/"));
        string projectName = projectPath.Substring(projectPath.LastIndexOf("/") + 1);
        projectPath = projectPath.Substring(0, projectPath.LastIndexOf("/"));
        string date = " " + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + " " + DateTime.Now.Hour + "-" + DateTime.Now.Minute;
        CopyDirectory(projectPath, backUpPath);
        string project = Path.Combine(backUpPath, projectName);
        string newproject = Path.Combine(backUpPath, projectName + date);
        Directory.Move(project, newproject);
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        //p.StartInfo = new System.Diagnostics.ProcessStartInfo(string.Format("explorer.exe {0}", backUpPath));
        p.StartInfo = new System.Diagnostics.ProcessStartInfo(string.Format("explorer.exe"));
        p.Start();
    }

    /// <summary>
    /// Toggle Shot/Hide selected GameObject
    /// </summary>
    [MenuItem("Kashyap/Utility/Toggle Active %U")]
    public static void ToggleShowHide()
    {
        foreach (var gameObject in Selection.gameObjects)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    /// <summary>
    /// Toggle Lock Inspector 
    /// </summary>
    [MenuItem("Kashyap/Utility/Toggle Lock %I")]
    public static void ToggleLockUnlock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
    }

    /// <summary>
    /// Create basic materials like red, white etc...
    /// </summary>
    [MenuItem("Kashyap/Basic Material")]
    public static void CreateMaterials()
    {
        Dictionary<string, Color> colors = new Dictionary<string, Color>();
        colors.Add("red", Color.red);
        colors.Add("green", Color.green);
        colors.Add("blue", Color.blue);
        colors.Add("cyan", Color.cyan);
        colors.Add("yellow", Color.yellow);
        colors.Add("black", Color.black);
        colors.Add("magenta", Color.magenta);
        colors.Add("white", Color.white);

        foreach (KeyValuePair<string, Color> color in colors)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color.Value;
            string matName = @"Assets\Material\" + color.Key + ".mat";
            AssetDatabase.CreateAsset(mat, matName);
        }
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Opens window with all scenes into the project
    /// </summary>
    [MenuItem("Kashyap/Scenes/All Scenes %L")]
    public static void Scenes()
    {
        EditorWindow.GetWindow(typeof(ScenesWindow));
    }

    /// <summary>
    /// Opens window with all build scenes means all scenes that are in Build Setting
    /// </summary>
    [MenuItem("Kashyap/Scenes/Build Scenes")]
    public static void BuildScenes()
    {
        EditorWindow.GetWindow(typeof(BuildScenesWindow));
    }

    #region Build Assets Bundle

    [MenuItem("Kashyap/Assets/Build AssetBundle From Selection - Track dependencies")]
    static void ExportResource()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {

            // Build the resource file from the active selection.
            UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies);
            Selection.objects = selection;
        }
    }


    [MenuItem("Kashyap/Assets/Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrack()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
        }
    }
    #endregion

    #region Helper Functions
    private static void CopyDirectory(string source, string destination)
    {
        DirectoryInfo dirSource = new DirectoryInfo(source);
        DirectoryInfo targetSource = new DirectoryInfo(destination);

        CopyDirectoryContents(dirSource, targetSource);
    }
    private static void CopyDirectoryContents(DirectoryInfo dirSource, DirectoryInfo targetSource)
    {
        try
        {
            Directory.CreateDirectory(targetSource.FullName);
            foreach (var file in dirSource.GetFiles())
            {
                Debug.Log(string.Format(@"Coping : {0}\{1}", targetSource.FullName, file.Name));
                file.CopyTo(Path.Combine(targetSource.FullName, file.Name), true);
            }
            foreach (var directory in dirSource.GetDirectories())
            {
                DirectoryInfo nextTargetDirectory = targetSource.CreateSubdirectory(directory.Name);
                CopyDirectoryContents(directory, nextTargetDirectory);
            }
        }
        catch (Exception)
        {


        }

    }
    #endregion
}
#endif
