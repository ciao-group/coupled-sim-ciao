using UnityEditor;
using UnityEngine;

public class FindMissingScripts : Editor
{
    [MenuItem("Tools/Find Missing Scripts in Scene")]
    public static void FindMissingScriptsInScene()
    {
        Debug.Log("FindMissingScriptsInScene script executed."); // Confirm the script is triggered

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int missingScriptCount = 0; // Keep track of how many missing scripts are found

        foreach (GameObject go in allObjects)
        {
            Debug.Log($"Checking GameObject: {go.name}"); // Trace which GameObject is being checked

            Component[] components = go.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component == null)
                {
                    missingScriptCount++;
                    Debug.LogError("Missing script found on: " + FullGameObjectPath(go), go);
                }
            }
        }

        if (missingScriptCount == 0)
        {
            Debug.Log("No missing scripts found in the scene.");
        }
        else
        {
            Debug.Log($"Total missing scripts found: {missingScriptCount}");
        }
    }

    private static string FullGameObjectPath(GameObject go)
    {
        return go.transform.parent == null ? go.name : FullGameObjectPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}
