using UnityEngine;

public class MissingScriptLogger : MonoBehaviour
{
    void Start()
    {
        CheckForMissingScripts();
    }

    void CheckForMissingScripts()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Component[] components = obj.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component == null)
                {
                    Debug.LogError("Missing script found on: " + GetGameObjectPath(obj), obj);
                }
            }
        }
    }

    string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "/" + path;
        }
        return path;
    }
}