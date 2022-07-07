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

public class NFTMenu : MonoBehaviour
{
    public HashOrTransaction transaction;
    public GameObject DebugText;

    private static IRippleClient client;
    // private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    public IpfsClient ipfs;

    // public Material nftImage;
    public RawImage nftImage;

    // Start is called before the first frame update
    async void Start()
    {
        // Text debugText = DebugText.GetComponent<Text>();
        // Debug.Log(transaction.TransactionHash);
        // ShowDefault();

        client = new RippleClient(serverUrl);
        client.Connect();
        await GetTransaction();
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

    public async Task GetTransaction()
    {
        // TransactionResponseCommon rtransaction = await client.Transaction(transaction.Transaction.TransactionHash);
        // ITransactionResponseCommon response = await client.Transaction("97DB76DC957E9CAFDB01435E7227AC4B5D455467BCFE1876F0C4700C3B3BCCF8");
        // NFTokenMintTransactionResponse rtransaction = (NFTokenMintTransactionResponse)transaction.Transaction;
        // NFTokenMintTransactionResponse rtransaction = (NFTokenMintTransactionResponse)response;
        // Debug.Log(string.Format("TX: {0}", rtransaction.URI.FromHexString()));
        // string uri = "ipfs://QmPQxWHyapjTATmGbLXCeHpY6i8p8DuiDrbLR3HJ1rXkRs";
        // // Debug.Log(uri);
        // ipfs = new IpfsClient("https://ipfs.io/ipfs");
        // IPFSTokenMetaData metadata = await MetaService.GetIPFSMeta(
        //     ipfs,
        //     "QmeJe2A4FydhwiMcHr8P1LiyZcgJryHPNWsf9bsTQ281nk"
        // );
        // Debug.Log(metadata.Image);
        // string imageUri = MetaService.ConvertIPFSURL(metadata.Image);
        // Debug.Log(imageUri);
        string imageUri = "https://nftdistro.mypinata.cloud/ipfs/QmVvUy7ZkPusUbALMPiEV1vuhaMZM7JBEjZ5TyfNPx6PA8";
        StartCoroutine(downloadImage(imageUri, nftImage));
    }

    public static IEnumerator downloadImage(string uri, RawImage frame)
    {
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);

        Debug.Log(www);
        DownloadHandler handle = www.downloadHandler;

        //Send Request and wait
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Error while Receiving: " + www.error);
        }
        else
        {
            Debug.Log("Success");
            frame.texture = DownloadHandlerTexture.GetContent(www);
        }
    }
}
