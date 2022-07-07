using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Xrpl.Client;
using Xrpl.Client.Model.Ledger;
using Ipfs.Http;
using IO.Swagger.Model;
using System.Threading.Tasks;
using UnityEngine.Networking;

// using Xrpl.Client.Responses.Transaction.Interfaces;
// using Xrpl.Client.Responses.Transaction.TransactionTypes;

public class MenuScript : MonoBehaviour
{
    public HashOrTransaction transaction;
    public GameObject DebugText;

    // private static IRippleClient client;
    // private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    // private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    // public IpfsClient ipfs;

    // public Material nftImage;
    public RawImage nftImage;

    // Start is called before the first frame update
    async void Start()
    {
        // Text debugText = DebugText.GetComponent<Text>();
        // Debug.Log(transaction.TransactionHash);
        // ShowDefault();

        // client = new RippleClient(serverUrl);
        // client.Connect();
        // await GetTransaction();
    }

    void ShowPayment()
    {
        Text debugText = DebugText.GetComponent<Text>();
        debugText.text = string.Format(
            "Account: {1}",
            transaction.Transaction.Account
        );
    }

    void ShowDefault()
    {
        Text debugText = DebugText.GetComponent<Text>();
        debugText.text = string.Format(
            "TransactionType: {0} \nAccount: {1}", 
            transaction.Transaction.TransactionType,
            transaction.Transaction.Account
        );
    }

    // Update is called once per frame
    void Update()
    {

    }

    // public async Task GetTransaction()
    // {
        // TransactionResponseCommon rtransaction = await client.Transaction(transaction.Transaction.TransactionHash);
        // ITransactionResponseCommon response = await client.Transaction("97DB76DC957E9CAFDB01435E7227AC4B5D455467BCFE1876F0C4700C3B3BCCF8");
        // NFTokenMintTransactionResponse rtransaction = (NFTokenMintTransactionResponse)transaction.Transaction;
        // NFTokenMintTransactionResponse rtransaction = (NFTokenMintTransactionResponse)response;
        
    // }
}
