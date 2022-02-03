using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine.UI;
using IO.Swagger.Model;
using Swordfish;
using Newtonsoft.Json;


public class AuthManager1 : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public Firebase.FirebaseApp app;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public FirebaseFirestore db;
    
    [Header("Login")]
    public GameObject authDialog;
    public InputField emailLoginField;
    public InputField passwordLoginField;

    [Tooltip("Optional GUI Text element to output debug information.")]
    public Text DebugText;
    
    private void Awake()
    {
        emailLoginField.text = "playertwo@swordfish.io";
        passwordLoginField.text = "123456";
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                InitializeFirebase();
            } else {
                LogText(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}",
                    dependencyStatus
                ));
            }
        });
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        LogText("Setting up firebase auth");
        // if (auth.CurrentUser != user) {
        //     bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
        //     if (!signedIn && user != null) {
        //         LogText("Signed out " + user.UserId);
        //     }
        //     user = auth.CurrentUser;
        //     if (signedIn) {
        //         LogText("Signed in " + user.UserId);
        //     }
        // }
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) 
    {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                LogText("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                LogText("Signed in " + user.UserId);
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            LoginButton();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            RegisterButton();
        }
    }

    public async void LoginButton()
    {
        LogText("Logging In");
        StartCoroutine(Login(
            emailLoginField.text,
            passwordLoginField.text
        ));
    }

    public async void RegisterButton()
    {
        LogText("Registering User");
        StartCoroutine(Register(
            emailLoginField.text,
            passwordLoginField.text
        ));
    }

    private IEnumerator Login(string _email, string password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        if (LoginTask.Exception != null)
        {
            LogText(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "Login failed!";
            switch(errorCode)
            {
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            LogText(message);
            // warningLoginText.text = message;
        } else {
            user = LoginTask.Result;
            _login(user);
        }
    }

    private async void _login(FirebaseUser user)
    {
        string token = await user.TokenAsync(true);
        GameState.Instance.userIdToken = token;

        string accountId = user.UserId;
        
        DocumentReference userRef = db.Collection("MasterUserList").Document(accountId);
        DocumentSnapshot userSnapshot = await userRef.GetSnapshotAsync();
        if (!userSnapshot.Exists) {
            Debug.Log(String.Format("Document {0} does not exist!", userSnapshot.Id));
        } else {
            Debug.Log(String.Format("Document data for {0} document:", userSnapshot.Id));
            Dictionary<string, object> userDict = userSnapshot.ToDictionary();
            DocumentReference playerRef = db.Collection("Players").Document(accountId);
            DocumentSnapshot playerSnapshot = await playerRef.GetSnapshotAsync();
            if (!playerSnapshot.Exists) {
                Debug.Log(String.Format("Document {0} does not exist!", playerSnapshot.Id));
            } else {
                Debug.Log(String.Format("Document data for {0} document:", playerSnapshot.Id));
                Dictionary<string, object> playerDict = playerSnapshot.ToDictionary();
                string js = JsonConvert.SerializeObject(playerDict);
                Player selfPlayer = JsonConvert.DeserializeObject(js) as Player;
                // Player.PermissionLevelEnum permissionLevel = selfPlayer.permissionLevel;
                GameState.Instance.accountId = accountId;
                GameState.Instance.selfPlayer = selfPlayer;
                // GameState.Instance.permissionLevel = permissionLevel;
                GameState.Instance.userId = user.UserId;
                LoginSuccessful();
            }
        }
    }

    private IEnumerator Register(string _email, string password)
    {
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, password);
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
        if (RegisterTask.Exception != null)
        {
            LogText(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "Login failed!";
            switch(errorCode)
            {
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            LogText(message);
            // warningLoginText.text = message;
        } else {
            user = RegisterTask.Result;

            // Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            
            // string token = await user.TokenAsync(true);
            string userId = user.UserId;
                    
            DocumentReference newMUserRef = db.Collection("MasterUserList").Document(userId);
            newMUserRef.SetAsync(new Dictionary<string, object>(){
                { "accountId", userId },
            });
            
            DocumentReference newPlayerRef = db.Collection("Players").Document(userId);
            DocumentReference newPlayerRefRef = db.Collection("PlayerRefs").Document(userId);
            
            // var device = AppDevice()
            // device.active = true
            // device.deviceID = userID
            // device.deviceType = "ios"
            // device.permissions = ["read": true, "write": true]
            // device.location = CLLocationCoordinate2D(latitude: 0.0, longitude: 0.0)
            
            Player newPlayer = new Player(
                playerId: newPlayerRef.Id,
                permissionLevel: Player.PermissionLevelEnum.User,
                active: true,
                deleted: false,
                createdTime: DateTime.UtcNow.Millisecond,
                email: _email,
                firstName: "firstName",
                lastName: "lastName",
                isLoggedIn: false
            );
            newPlayer.UpdatedTime = DateTime.UtcNow.Millisecond;
            newPlayer.Terms = true;
            newPlayer.TermTime = DateTime.UtcNow.Millisecond;
            newPlayer.Privacy = true;
            newPlayer.PrivacyTime = DateTime.UtcNow.Millisecond;
            newPlayer.IsOnline = true;
            newPlayer.LastLoggedIn = DateTime.UtcNow.Millisecond;
            // newPlayer.device = device
            
            PlayerRef playerRef = new PlayerRef(
                playerId: newPlayerRef.Id,
                active: true,
                deleted: false,
                createdTime: DateTime.UtcNow.Millisecond
                // userType: Player.PermissionLevelEnum.User
            );
            
            // GameState.userIdToken = token;
            GameState.Instance.userId = userId;
            GameState.Instance.accountId = userId;
            GameState.Instance.selfPlayer = newPlayer;

            Dictionary<string, object> playerParsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(newPlayer.ToJson());
            newPlayerRef.SetAsync(playerParsed);

            Dictionary<string, object> playerRefParsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(playerRef.ToJson());
            newPlayerRefRef.SetAsync(playerRefParsed);
            LogText("User Registered");
        }
    }

    public void LogText(string message) {

        // Output to worldspace to help with debugging.
        if (DebugText) {
            DebugText.text += "\n" + message;
        }

        Debug.Log(message);
    }

    public void LoginSuccessful()
    {
        GameState.Instance.isLoggedIn = true;
        LogText("Logged In");
        authDialog.SetActive(false);
    }
}
