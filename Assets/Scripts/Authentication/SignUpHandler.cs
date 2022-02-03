using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpHandler : MonoBehaviour
{

  public AuthManager authManager;
  public InputField emailTextBox;
  public InputField passwordTextBox;
  public Button signupButton;
  public Text emailErrorText;
  public Text passwordErrorText;
  public FirebaseAuth auth;

  // Start is called before the first frame update
  void Start()
  {
    emailTextBox.text = "playerone@swordfish.io";
    passwordTextBox.text = "123456";
    auth = FirebaseAuth.DefaultInstance;
    signupButton.onClick.AddListener(() => RegisterButton());
    // backButton.onClick.AddListener(() => SceneManager.LoadScene("SignInScene"));
  }
  public void Update()
  {
      if (Input.GetKeyUp(KeyCode.L))
      {
          RegisterButton();
      }
      if (Input.GetKeyUp(KeyCode.P))
      {
          SceneManager.LoadScene("SignInScene");
      }
  }

  private async void RegisterButton()
  {
    // passwordErrorText.enabled = false;
    StartCoroutine(Register(
        emailTextBox.text,
        passwordTextBox.text
    ));
  }

  // Create a user with the email and password.
  // public Task CreateUserWithEmailAsync() {
  //   string email = emailTextBox.text;
  //   string password = passwordTextBox.text;

  //   Debug.Log(String.Format("Attempting to create user {0}...", email));
  //   DisableUI();

  //   return auth.CreateUserWithEmailAndPasswordAsync(email, password)
  //     .ContinueWithOnMainThread((task) => {
  //       EnableUI();
  //       LogTaskCompletion(task, "User Creation");
  //       return task;
  //     }).Unwrap();
  // }

  // // Log the result of the specified task, returning true if the task
  // // completed successfully, false otherwise.
  // protected bool LogTaskCompletion(Task task, string operation) {
  //   bool complete = false;
  //   if (task.IsCanceled) {
  //     Debug.Log(operation + " canceled.");
  //   } else if (task.IsFaulted) {
  //     Debug.Log(operation + " encounted an error.");
  //     foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
  //       string authErrorCode = "";
  //       Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
  //       if (firebaseEx != null) {
  //         authErrorCode = String.Format("AuthError.{0}: ",
  //           ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
  //         GetErrorMessage((Firebase.Auth.AuthError)firebaseEx.ErrorCode);
  //       }
  //       Debug.Log(authErrorCode + exception.ToString());
  //     }
  //   } else if (task.IsCompleted) {
  //     Debug.Log(operation + " completed");
  //     complete = true;
  //   }
  //   return complete;
  // }

  private IEnumerator Register(string _email, string password)
  {
      var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, password);
      yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
      if (RegisterTask.Exception != null)
      {
          Debug.Log(message: $"Failed to register task with {RegisterTask.Exception}");
          FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
          AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
          string message = "Login failed!";
          switch(errorCode)
          {
              case AuthError.UserNotFound:
                  message = "Account does not exist";
                  break;
          }
          Debug.Log(message);
          // warningLoginText.text = message;
      } else {
          FirebaseUser user = RegisterTask.Result;
          authManager._register(user);
      }
  }

    void DisableUI()
  {
    emailTextBox.DeactivateInputField();
    passwordTextBox.DeactivateInputField();
    // confirmPasswordTextBox.DeactivateInputField();
    // backButton.interactable = false;
    signupButton.interactable = false;
    // emailErrorText.enabled = false;
    // passwordErrorText.enabled = false;
  }

  void EnableUI()
  {
    emailTextBox.ActivateInputField();
    passwordTextBox.ActivateInputField();
    // confirmPasswordTextBox.ActivateInputField();
    // backButton.interactable = true;
    signupButton.interactable = true;
  }

  // private void GetErrorMessage(AuthError errorCode)
  // {
  //   switch (errorCode)
  //   {
  //     case AuthError.MissingPassword:
  //       passwordErrorText.text = "Missing password.";
  //       passwordErrorText.enabled = true;
  //       break;
  //     case AuthError.WeakPassword:
  //       passwordErrorText.text = "Too weak of a password.";
  //       passwordErrorText.enabled = true;
  //       break;
  //     case AuthError.InvalidEmail:
  //       emailErrorText.text = "Invalid email.";
  //       emailErrorText.enabled = true;
  //       break;
  //     case AuthError.MissingEmail:
  //       emailErrorText.text = "Missing email.";
  //       emailErrorText.enabled = true;
  //       break;
  //     case AuthError.UserNotFound:
  //       emailErrorText.text = "Account not found.";
  //       emailErrorText.enabled = true;
  //       break;
  //     case AuthError.EmailAlreadyInUse:
  //       emailErrorText.text = "Email already in use.";
  //       emailErrorText.enabled = true;
  //       break;
  //     default:
  //       emailErrorText.text = "Unknown error occurred.";
  //       emailErrorText.enabled = true;
  //       break;
  //   }
  // }
}
