using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Swordfish;
using GalleryCSharp.Models;
using Xrpl.Client;
using Xrpl.Models.Methods;

public class PlayerStats : CharacterStats
{
    PlayerUI playerUI;
    private static IXrplClient client;
    private Player selfPlayer;
    
    private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    // Start is called before the first frame update
    private async void Awake()
    {
        if (!GameState.Instance.isLoggedIn) { return; }
        
        playerUI = GetComponent<PlayerUI> ();

        selfPlayer = GameState.Instance.selfPlayer;

        maxHealth = 100;
        currHealth = maxHealth;

        maxStamina = 100;
        currStamina = maxStamina;

        client = new XrplClient(serverUrl);
        client.Connect();
        await GetAccountInfo();

        string seed = PlayerPrefs.GetString("seed");
        string address = PlayerPrefs.GetString("address");
        Debug.Log(seed);
        Debug.Log(address);
    }

    public async Task GetAccountInfo()
    {
        AccountInfoRequest request = new AccountInfoRequest(selfPlayer.DefaultSignor);
        AccountInfo accountInfo = await client.AccountInfo(request);
        currencyTotal = (decimal)accountInfo.AccountData.Balance.ValueAsXrp;
        SetStats ();
        client.Disconnect();
    }

    public override void Die()
    {
        Debug.Log("You Died!");
        // client.Disconnect();
    }

    void SetStats()
    {
        Debug.Log("Setting Player Statistics");
        playerUI.currencyAmount.text = $"{System.Math.Round(currencyTotal, 2)} XRP";
    }

    void UpdatePlayerBanner(string message)
    {
        playerUI.bannerNotifictation.text = message;
    }

    // void OnDestroy()
    // {
    //     client.Disconnect();
    // }
}
