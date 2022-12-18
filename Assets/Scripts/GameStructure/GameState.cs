using UnityEngine;
using GalleryCSharp.Models;

public class GameState : Singleton<GameState>
{
    protected GameState() { }

    public bool isLoggedIn = false;
    public bool didTimeout = false;
    public string userIdToken;
    public string userId;
    public string accountId;
    // public MasterUser selfAccount;
    public Player selfPlayer;
    public Player.PermissionLevelEnum permissionLevel;

    public void ClearState() {
        isLoggedIn = false;
        didTimeout = false;
        userIdToken = "";
        userId = "";;
        accountId = "";;
    }
}