using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;

public class AuthSetup : MonoBehaviour
{
  public AuthManager authManager;
  public DependencyStatus dependencyStatus;

  [Tooltip("Optional GUI Text element to output debug information.")]
  public Text DebugText;
  
  void Start()
  {
    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        var dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
            authManager.InitializeFirebase();
        } else {
            LogText(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}",
                dependencyStatus
            ));
        }
    });
  }

  void LogText(string message) {

        // Output to worldspace to help with debugging.
        if (DebugText) {
            DebugText.text += "\n" + message;
        }

        Debug.Log(message);
    }
}