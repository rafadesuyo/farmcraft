// using System.IO;
// using UnityEditor;
// using UnityEditor.Callbacks;
// using UnityEditor.iOS.Xcode;

// public class iOSBuildProcessor
// {
// [PostProcessBuildAttribute(1)]
// public static void OnPostprocessorBuild(BuildTarget target, string pathToBuiltProject)
//{
// TODO: Review this code to add capabilities after iOS build
// https://ocarinastudios.atlassian.net/browse/DQG-2307

// if (target == BuildTarget.iOS)
// {
//    string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
//    PBXProject pbxProject = new PBXProject();
//    pbxProject.ReadFromString(File.ReadAllText(projectPath));
//    string mainGuid = pbxProject.GetUnityMainTargetGuid();
//    var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, mainGuid);
//    manager.AddGameCenter();
//    manager.AddSignInWithApple();
//    manager.WriteToFile();
//}
//}
//}
