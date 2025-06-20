

// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;
// using System.IO;
// using System.Text.RegularExpressions;

// public class EventBusSubscribeChecker : EditorWindow
// {
//     [MenuItem("Tools/EventBus Subscribe Checker")]
//     public static void ShowWindow()
//     {
//         GetWindow<EventBusSubscribeChecker>("EventBus Subscribe Checker");
//     }

//     private Vector2 scroll;
//     private string results = "";

//     void OnGUI()
//     {
//         if (GUILayout.Button("Check Subscribe/Unsubscribe Balance"))
//         {
//             results = CheckSubscribeUnsubscribeBalance();
//         }

//         scroll = GUILayout.BeginScrollView(scroll);
//         GUILayout.Label(results);
//         GUILayout.EndScrollView();
//     }

//     private string CheckSubscribeUnsubscribeBalance()
//     {
//         string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
//         string subscribePattern = @"EventBus\.Subscribe<.*?>\s*\(";
//         string unsubscribePattern = @"EventBus\.Unsubscribe<.*?>\s*\(";

//         string report = "";

//         foreach (var file in files)
//         {
//             string code = File.ReadAllText(file);

//             var subscribeMatches = Regex.Matches(code, subscribePattern);
//             var unsubscribeMatches = Regex.Matches(code, unsubscribePattern);

//             if (subscribeMatches.Count > 0 && unsubscribeMatches.Count == 0)
//             {
//                 report += $"⚠️ {Path.GetFileName(file)} subscribes but does NOT unsubscribe!\n";
//             }
//             else if (subscribeMatches.Count > 0)
//             {
//                 report += $"{Path.GetFileName(file)} subscribes and unsubscribes properly.\n";
//             }
//         }

//         if (string.IsNullOrEmpty(report))
//             report = "No subscriptions found.";

//         return report;
//     }
// }
// #endif
