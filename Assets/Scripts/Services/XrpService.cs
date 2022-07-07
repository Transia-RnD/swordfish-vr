using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swordfish;
using IO.Swagger.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xrpl.Client.Requests.Transaction;
using Xrpl.Client.Model.Transaction;
using Xrpl.Client.Requests.Ledger;
using Xrpl.Client.Model.Ledger;
using Xrpl.Client;
using Xrpl.Wallet;
using Ripple.Keypairs;
using System.Threading.Tasks;

public class XrpService : MonoBehaviour
{
    private static IRippleClient client;
    private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    public void Awake()
    {
        client = new RippleClient(serverUrl);
        client.Connect();
    }

    public Wallet CreateXrpWallet(string secret)
    {
        Seed seed = Seed.FromPassPhrase("taco main silly string happened town dollar toon").SetEd25519();
        string signorAddress = seed.KeyPair().Id();

        Wallet wallet = new Wallet(
            walletId: "null",
            active: true,
            deleted: false,
            createdTime: 0,
            address: signorAddress,
            name: "Gambit_Xrp_Wallet",
            isHd: false,
            isPrivate: false
        );
        string encryptedSeed = AESGCM.SimpleEncryptWithPassword(
            seed.ToString(),
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
    
    public SignedTx XrpSignTransaction(string destination, string amount)
    {
        string secret = "sEd7rBGm5kxzauRTAV2hbsNz7N45X91";
        Dictionary<string, object> txJson = new Dictionary<string, object>() {
            { "Account", "Account" },
            { "Amount",  amount },
            { "Destination", destination },
            { "Fee", "10" },
            { "Flags", "2147483648" }, 
            { "Sequence", "1" },
            { "TransactionType", "Payment" },
        };
        string unsignedTxJson = JsonConvert.SerializeObject(txJson);
        return TxSigner.SignJson(JObject.Parse(unsignedTxJson), secret);
    }

    public async Task<Submit> XrpSubmitTransaction(RippleClient client, SignedTx signedTx)
    {
        SubmitBlobRequest request = new SubmitBlobRequest();
        request.TransactionBlob = signedTx.TxBlob;
        return await client.SubmitTransactionBlob(request);
    }

    public string GetNFTFromTokenId(string tokenId)
    {
        return "QmeJe2A4FydhwiMcHr8P1LiyZcgJryHPNWsf9bsTQ281nk";
    }
}