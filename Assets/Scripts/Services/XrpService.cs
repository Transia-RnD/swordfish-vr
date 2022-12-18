using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swordfish;
using GalleryCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using AppWallet = GalleryCSharp.Models.Wallet;
using Xrpl.Client;
using Xrpl.Wallet;

public class XrpService : MonoBehaviour
{
    private static IXrplClient client;
    private static string serverUrl = "wss://hooks-testnet-v2.xrpl-labs.com";

    public void Awake()
    {
        client = new XrplClient(serverUrl);
        client.Connect();
    }

    public AppWallet CreateXrpWallet(string secret)
    {
        XrplWallet xwallet = XrplWallet.Generate();
        string signorAddress = xwallet.ClassicAddress;

        AppWallet wallet = new AppWallet(
            id: "null",
            active: true,
            deleted: false,
            createdTime: 0,
            address: signorAddress,
            name: "MetaXrplorer_Xrp_Wallet",
            isHd: false,
            isPrivate: false
        );
        string encryptedSeed = AESGCM.SimpleEncryptWithPassword(
            xwallet.Seed.ToString(),
            secret
        );
        string encryptedSecret = AESGCM.SimpleEncryptWithPassword(
            secret.ToString(),
            "123456654321"
        );
        PlayerPrefs.SetString("seed", encryptedSeed);
        PlayerPrefs.SetString("secret", encryptedSecret);
        return wallet;
    }

    // public async Task<Ledger> FetchLedger(RippleClient client)
    // {
    //     LedgerRequest request = new LedgerRequest();
    //     return await client.Ledger();
    // }
    
    // public SignedTx XrpSignTransaction(string destination, string amount)
    // {
    //     string secret = "sEd7rBGm5kxzauRTAV2hbsNz7N45X91";
    //     Dictionary<string, object> txJson = new Dictionary<string, object>() {
    //         { "Account", "Account" },
    //         { "Amount",  amount },
    //         { "Destination", destination },
    //         { "Fee", "10" },
    //         { "Flags", "2147483648" }, 
    //         { "Sequence", "1" },
    //         { "TransactionType", "Payment" },
    //     };
    //     string unsignedTxJson = JsonConvert.SerializeObject(txJson);
    //     return wallet.Sign(unsignedTxJson);
    // }

    // public async Task<Submit> XrpSubmitTransaction(RippleClient client, SignedTx signedTx)
    // {
    //     SubmitBlobRequest request = new SubmitBlobRequest();
    //     request.TransactionBlob = signedTx.TxBlob;
    //     return await client.SubmitTransactionBlob(request);
    // }

    public string GetNFTFromTokenId(string tokenId)
    {
        return "QmeJe2A4FydhwiMcHr8P1LiyZcgJryHPNWsf9bsTQ281nk";
    }
}