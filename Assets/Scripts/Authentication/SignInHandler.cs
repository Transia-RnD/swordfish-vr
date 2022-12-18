using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;

public class SignInHandler : MonoBehaviour
{
  public AuthManager authManager;
  public InputField emailTextBox;
  public InputField passwordTextBox;
  public Button signinButton;
  public Button backButton;
  public Text emailErrorText;
  public Text passwordErrorText;
  private FirebaseAuth auth;
  // Whether to sign in / link or reauthentication *and* fetch user profile data.
  private bool signInAndFetchProfile = false;

  // Start is called before the first frame update
  void Start()
  {
    emailTextBox.text = "playerone@swordfish.io";
    passwordTextBox.text = "123456";
    auth = FirebaseAuth.DefaultInstance;
    signinButton.onClick.AddListener(() => SigninWithEmailAsync());
    backButton.onClick.AddListener(() => SceneManager.LoadScene("SignUpScene"));
  }

  public void Update()
  {
      if (Input.GetKeyUp(KeyCode.L))
      {
          SigninWithEmailAsync();
      }
      if (Input.GetKeyUp(KeyCode.P))
      {
          SceneManager.LoadScene("SignUpScene");
      }
  }

  // Sign-in with an email and password.
  public Task SigninWithEmailAsync()
  {
    var email = emailTextBox.text;
    var password = passwordTextBox.text;
    Debug.Log(String.Format("Attempting to sign in as {0}...", email));
    DisableUI();
    if (signInAndFetchProfile)
    {
      return auth.SignInAndRetrieveDataWithCredentialAsync(
        Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(
          HandleSignInWithSignInResult);
    }
    else
    {
      return auth.SignInWithEmailAndPasswordAsync(email, password)
        .ContinueWithOnMainThread(HandleSignInWithUser);
    }
  }

  // Called when a sign-in with profile data completes.
  void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task)
  {
    EnableUI();
    if (LogTaskCompletion(task, "Sign-in"))
    {
      SceneManager.LoadScene("MetaXrplorer");
    }
  }

  void DisableUI()
  {
    emailTextBox.DeactivateInputField();
    passwordTextBox.DeactivateInputField();
    signinButton.interactable = false;
    backButton.interactable = false;
    // emailErrorText.enabled = false;
    // passwordErrorText.enabled = false;
  }

  void EnableUI()
  {
    emailTextBox.ActivateInputField();
    passwordTextBox.ActivateInputField();
    signinButton.interactable = true;
    backButton.interactable = true;
  }

  // Log the result of the specified task, returning true if the task
  // completed successfully, false otherwise.
  protected bool LogTaskCompletion(Task task, string operation)
  {
    bool complete = false;
    if (task.IsCanceled)
    {
      Debug.Log(operation + " canceled.");
    }
    else if (task.IsFaulted)
    {
      Debug.Log(operation + " encounted an error.");
      foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
      {
        string authErrorCode = "";
        Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
        if (firebaseEx != null)
        {
          authErrorCode = String.Format("AuthError.{0}: ",
            ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
          GetErrorMessage((Firebase.Auth.AuthError)firebaseEx.ErrorCode);
        }
        Debug.Log(authErrorCode + exception.ToString());
      }
    }
    else if (task.IsCompleted)
    {
      Debug.Log(operation + " completed");
      complete = true;
    }
    return complete;
  }

  // Called when a sign-in without fetching profile data completes.
  void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
  {
    EnableUI();
    if (LogTaskCompletion(task, "Sign-in"))
    {
      FirebaseUser user = task.Result;
      authManager._login(user);
    }
  }

  private void GetErrorMessage(AuthError errorCode)
  {
    switch (errorCode)
    {
      case AuthError.MissingPassword:
        passwordErrorText.text = "Missing password.";
        passwordErrorText.enabled = true;
        break;
      case AuthError.WrongPassword:
        passwordErrorText.text = "Incorrect password.";
        passwordErrorText.enabled = true;
        break;
      case AuthError.InvalidEmail:
        emailErrorText.text = "Invalid email.";
        emailErrorText.enabled = true;
        break;
      case AuthError.MissingEmail:
        emailErrorText.text = "Missing email.";
        emailErrorText.enabled = true;
        break;
      case AuthError.UserNotFound:
        emailErrorText.text = "Account not found.";
        emailErrorText.enabled = true;
        break;
      default:
        emailErrorText.text = "Unknown error occurred.";
        emailErrorText.enabled = true;
        break;
    }
  }
}
