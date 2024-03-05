using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PostBuildAction : IPostprocessBuildWithReport

{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    { Debug.Log("Post-Build script executed."); }
}