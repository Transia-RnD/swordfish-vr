using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using IO.Swagger.Model;
using Newtonsoft.Json;

[CreateAssetMenu]
public class AuthManager : ScriptableObject
{
    [Header("Firebase")]
    public Firebase.FirebaseApp app;
    public FirebaseAuth auth;
    public FirebaseFirestore db;
    public FirebaseUser user;

    public void InitializeFirebase()
    {
        Debug.Log("Setting up firebase auth");
        app = Firebase.FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        // auth.StateChanged += AuthStateChanged;
        // auth.IdTokenChanged += IdTokenChanged;
        db = FirebaseFirestore.DefaultInstance;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) 
    {
        Debug.Log("Auth State Changed");
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
                SceneManager.LoadScene("SignInScene");
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
                _login(user);
            }
        }
        else
        {
            SceneManager.LoadScene("SignInScene");
        }
    }
    public async void _login(FirebaseUser user)
    {
        // string token = await user.TokenAsync(true);
        // GameState.Instance.userIdToken = token;

        string accountId = user.UserId;
        
        DocumentReference userRef = db.Collection("MasterUserList").Document(accountId);
        DocumentSnapshot userSnapshot = await userRef.GetSnapshotAsync();
        if (!userSnapshot.Exists) {
            Debug.Log(String.Format("Document {0} does not exist!", userSnapshot.Id));
        } else {
            Dictionary<string, object> userDict = userSnapshot.ToDictionary();
            DocumentReference playerRef = db.Collection("Players").Document(accountId);
            DocumentSnapshot playerSnapshot = await playerRef.GetSnapshotAsync();
            if (!playerSnapshot.Exists) {
                Debug.Log(String.Format("Document {0} does not exist!", playerSnapshot.Id));
            } else {
                Dictionary<string, object> playerDict = playerSnapshot.ToDictionary();
                string js = JsonConvert.SerializeObject(playerDict);
                Player selfPlayer = JsonConvert.DeserializeObject<Player>(js);
                // Player.PermissionLevelEnum permissionLevel = selfPlayer.permissionLevel;
                GameState.Instance.accountId = accountId;
                GameState.Instance.selfPlayer = selfPlayer;
                // GameState.Instance.permissionLevel = permissionLevel;
                GameState.Instance.userId = user.UserId;
                LoginSuccessful();
            }
        }
    }

    public async void _register(FirebaseUser user)
    {

        Int32 createdTime = Convert.ToInt32(Utils.UnixTimeNow());
        
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
            createdTime: createdTime,
            email: user.Email,
            firstName: "firstName",
            lastName: "lastName",
            isLoggedIn: false
        );
        newPlayer.UpdatedTime = createdTime;
        newPlayer.Terms = true;
        newPlayer.TermTime = createdTime;
        newPlayer.Privacy = true;
        newPlayer.PrivacyTime = createdTime;
        newPlayer.IsOnline = true;
        newPlayer.LastLoggedIn = createdTime;
        // newPlayer.device = device
        
        PlayerRef playerRef = new PlayerRef(
            playerId: newPlayerRef.Id,
            active: true,
            deleted: false,
            createdTime: createdTime
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
        
        RegisterSuccessful();
    }

    public void LoginSuccessful()
    {
        GameState.Instance.isLoggedIn = true;
        Debug.Log("Logged In");
        SceneManager.LoadScene("Meta(x)rplorer");
    }

    public void RegisterSuccessful()
    {
        GameState.Instance.isLoggedIn = true;
        Debug.Log("Registered");
        SceneManager.LoadScene("Meta(x)rplorer");
    }

    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Debug.Log("Getting userIDToken");
        // Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        // if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        // {
        // senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
        //     task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        // }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}