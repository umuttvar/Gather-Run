using UnityEngine;
using UnityEditor;

public class MissingScriptRemover : EditorWindow
{
    [MenuItem("Tools/Remove Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MissingScriptRemover));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Find and Remove Missing Scripts in Selected GameObjects"))
        {
            RemoveMissingScripts();
        }
    }

    private static void RemoveMissingScripts()
    {
        GameObject[] go = Selection.gameObjects;
        int compCount = 0;
        int goCount = 0;
        foreach (GameObject g in go)
        {
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(g);
            if (count > 0)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
                compCount += count;
                goCount++;
            }
        }
        Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
    }
}