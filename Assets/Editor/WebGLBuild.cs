#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Optional local build helper. GameCI (unity-builder) does NOT need this —
/// it builds the enabled scenes itself. This exists only so a human with a
/// local Editor can reproduce the same WebGL build from the menu.
/// </summary>
public static class WebGLBuild
{
    [MenuItem("Yajang/Build WebGL (PoC)")]
    public static void Build()
    {
        string[] scenes = { "Assets/Scenes/Main.unity" };
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Build/WebGL",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };
        BuildReport report = BuildPipeline.BuildPlayer(options);
        Debug.Log("[Yajang PoC] WebGL build finished: " + report.summary.result);
    }
}
#endif
