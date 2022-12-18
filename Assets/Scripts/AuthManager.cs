using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using GalleryCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Converters;
using Xrpl.Client.Json.Converters;

public class AotTypeEnforcer : ScriptableObject
{
    public void Awake()
    {
        AotHelper.EnsureType<StringEnumConverter>();
        AotHelper.EnsureType<CurrencyConverter>();
        // AotHelper.EnsureType<GenericStringConverter<T>>();
        AotHelper.EnsureType<LedgerBinaryConverter>();
        AotHelper.EnsureType<LedgerIndexConverter>();
        AotHelper.EnsureType<LOConverter>();
        AotHelper.EnsureType<MetaBinaryConverter>();
        AotHelper.EnsureType<RippleDateTimeConverter>();
        AotHelper.EnsureType<StringOrArrayConverter>();
        AotHelper.EnsureType<TransactionConverter>();
        AotHelper.EnsureType<TransactionOrHashConverter>();
    }
}

public class AuthManager : ScriptableObject
{
    public Text DebugText;

    [Header("Firebase")]
    public Firebase.FirebaseApp app;
    public FirebaseAuth auth;
    public FirebaseFirestore db;
    public FirebaseUser user;

    private JsonSerializerSettings serializerSettings = new JsonSerializerSettings
    {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
    };

    public void InitializeFirebase()
    {
        // LogText("SETUP FIREBASE AUTH");
        app = Firebase.FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        // auth.StateChanged += AuthStateChanged;
        // auth.IdTokenChanged += IdTokenChanged;
        db = FirebaseFirestore.DefaultInstance;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) 
    {
        // LogText("AUTH STATE CHANGED");
        if (auth.CurrentUser != user) {
            bool signedIn = auth.CurrentUser != null;
            if (!signedIn && user != null) {
                LogText("SIGNED OUT " + user.UserId);
                SceneManager.LoadScene("SignInScene");
                return;
            }
            user = auth.CurrentUser;
            if (signedIn) {
                LogText("SIGNED IN " + user.UserId);
                _login(user);
                return;
            }
        }
        else
        {
            // LogText("NOT LOGGED IN");
            SceneManager.LoadScene("SignInScene");
        }
    }
    public async void _login(FirebaseUser user)
    {
        // LogText("LOGIN STARTED");
        // string token = await user.TokenAsync(true);
        // GameState.Instance.userIdToken = token;

        string accountId = user.UserId;
        
        DocumentReference userRef = db.Collection("MasterUserList").Document(accountId);
        DocumentSnapshot userSnapshot = await userRef.GetSnapshotAsync();
        if (!userSnapshot.Exists) {
            LogText(String.Format("Document {0} does not exist!", userSnapshot.Id));
            auth.SignOut();
            SceneManager.LoadScene("SignInScene");
        } else {
            LogText("VALID MASTER USER");
            Dictionary<string, object> userDict = userSnapshot.ToDictionary();
            DocumentReference playerRef = db.Collection("Players").Document(accountId);
            DocumentSnapshot playerSnapshot = await playerRef.GetSnapshotAsync();
            if (!playerSnapshot.Exists) {
                LogText(String.Format("Document {0} does not exist!", playerSnapshot.Id));
                auth.SignOut();
                SceneManager.LoadScene("SignInScene");
            } else {
                try
                {
                    LogText("VALID PLAYER");
                    Dictionary<string, object> playerDict = playerSnapshot.ToDictionary();
                    string serialized = JsonConvert.SerializeObject(playerDict);
                    Player selfPlayer = JsonConvert.DeserializeObject<Player>(serialized);
                    // Player.PermissionLevelEnum permissionLevel = selfPlayer.permissionLevel;
                    GameState.Instance.accountId = accountId;
                    GameState.Instance.selfPlayer = selfPlayer;
                    // GameState.Instance.permissionLevel = permissionLevel;
                    GameState.Instance.userId = user.UserId;
                    LoginSuccessful();
                }
                catch (Exception e)
                {
                    LogText(String.Format("{0} Exception caught", e));
                }
            }
        }
    }

    public async void _register(FirebaseUser user)
    {

        Int32 createdTime = Convert.ToInt32(Utils.UnixTimeNow());
        
        // string token = await user.TokenAsync(true);
        string userId = user.UserId;
                
        DocumentReference newMUserRef = db.Collection("MasterUserList").Document(userId);
        await newMUserRef.SetAsync(new Dictionary<string, object>(){
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
            id: newPlayerRef.Id,
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
            id: newPlayerRef.Id,
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
        await newPlayerRef.SetAsync(playerParsed);

        Dictionary<string, object> playerRefParsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(playerRef.ToJson());
        await newPlayerRefRef.SetAsync(playerRefParsed);
        
        RegisterSuccessful();
    }

    public void LoginSuccessful()
    {
        LogText("LOGGED IN");
        GameState.Instance.isLoggedIn = true;
        SceneManager.LoadScene("MetaXrplorer");
    }

    public void RegisterSuccessful()
    {
        LogText("REGISTERED");
        GameState.Instance.isLoggedIn = true;
        SceneManager.LoadScene("MetaXrplorer");
    }

    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        LogText("Getting userIDToken");
        // Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        // if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        // {
        // senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
        //     task => LogText(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        // }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
    void LogText(string message) 
    {

        // Output to worldspace to help with debugging.
        if (DebugText) {
            DebugText.text += "\n" + message;
        }
        Debug.Log(message);
    }
}