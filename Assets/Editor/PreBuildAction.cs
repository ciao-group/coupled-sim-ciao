using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreBuildAction : IPreprocessBuildWithReport

{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    { Debug.Log("Pre-Build script executed."); }
}