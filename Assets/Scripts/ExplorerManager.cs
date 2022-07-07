using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ripple.Binary.Codec.Types;
using Xrpl.Client.Model;
using Xrpl.Client.Json.Converters;
using Xrpl.Client.Model.Ledger;
using Xrpl.Client.Requests.Ledger;
using Xrpl.Client.Model.Account;
using Xrpl.Client.Model.Account;
using Xrpl.Client.Requests.Account;
using Xrpl.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Converters;
using IO.Swagger.Model;

public class LedgerEntity : MonoBehaviour
{
    [JsonProperty("account_hash")]
    public string AccountHash { get; set; }

    [JsonProperty("accounts")]
    public dynamic[] Accounts { get; set; }
}

public class ExplorerManager : MonoBehaviour
{
    
    [SerializeField] public GameObject ledgerPrefab;
    [SerializeField] public GameObject commonTxMenu;
    [SerializeField] public GameObject accountTxMenu;
    [SerializeField] public GameObject paymentTxMenu;
    [SerializeField] public GameObject nftTxMenu;

    public float speed = 7.0f;

    private List<Ledger> ledgerList = new List<Ledger>();
    private List<GameObject> ledgerGOList = new List<GameObject>();

    private static IRippleClient client;
    private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    // private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    private List<string> transactions = new List<string>();

    // int section = 0;
    // int row = 0;
    // int item = 0;
    int x_matrix = 3;
    int x_cur = 1;
    int y_matrix = 3;
    int y_cur = 1;
    int z_cur = 1;

    [Tooltip("Optional GUI Text element to output debug information.")]
    public Text DebugText;


    // Start is called before the first frame update
    private async void Awake()
    {
        try
        {
            // if (!GameState.Instance.isLoggedIn) { return; }
            client = new RippleClient(serverUrl);
            client.Connect();
            LogText("XRPL CONNECTED");
            InvokeRepeating("AddLedger", 2.0f, 4.0f);
            // Player selfPlayer = GameState.Instance.selfPlayer;
            // LogText(String.Format("PLAYER ADDRESS {0}", "r223rsyz1cfqPbjmiX6oYu1hFgNwCkWZH"));
            // AccountInfo accountInfo = await client.AccountInfo("r223rsyz1cfqPbjmiX6oYu1hFgNwCkWZH");
            // decimal currencyTotal = (decimal)accountInfo.AccountData.Balance.ValueAsXrp;
            // LogText(String.Format("ACCOUNT CURRENCY {0}", currencyTotal));
            // await AddLedger();
        }
        catch (Exception e)
        {
            LogText(String.Format("{0} Awake caught", e));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async Task AddLedger()
    {   
        try
        {
            LogText("FETCHING LEDGER");
            var request = new LedgerRequest {
                LedgerIndex = new LedgerIndex(LedgerIndexType.Validated),
                Transactions = true,
                Binary = true
            };
            LogText("LEDGER REQUEST");
            BaseLedgerInfo closedLedger = await client.ClosedLedger();
            LogText("RECEIVED LEDGER");
            LogText(string.Format("Closed Ledger: {0}", closedLedger.LedgerHash));
            request.LedgerHash = closedLedger.LedgerHash;
            Ledger ledger = await client.Ledger(request);
            LedgerBinaryEntity entity = (LedgerBinaryEntity)ledger.LedgerEntity;
            LogText(string.Format("TXs: {0}", entity.Transactions));
            if (!ledgerList.Contains(ledger))
            {
                ledgerList.Add(ledger);
                SpawnLedgerBoxes(ledger);
            }
        }
        catch (Exception e)
        {
            LogText(String.Format("{0} AddLedger caught", e));
        }
    }

    void ResetLedger()
    {
        for (int i = 0; i < ledgerGOList.Count; ++i) {  // X Axis
            Destroy (ledgerGOList[i]);
        }
        // LogText("INCOMING TXs: " + ledger.LedgerEntity.Transactions.Count);
        // for (int i = 0; i < ledger.LedgerEntity.Transactions.Count; ++i) {
        //     transactions.Add(ledger.LedgerEntity.Transactions[i].TransactionHash);
        // }
        // if (transactions.Count > 0)
        // {
        //     ResetLedger();
        //     SpawnLedgerBoxes();
        // }
    }

    Vector3 NextLedgerVector()
    {
        // Calc Y
        if (y_cur == y_matrix)
        {
            y_cur = 1;
            // Calc X
            if (x_cur == x_matrix)
            {
                x_cur = 1;
                z_cur += 1;
            }
            else
            {
                x_cur += 1;
            }
        }
        else 
        {
            y_cur += 1;
        }
        // LogText(string.Format("X Axis: {0} Y Axis: {1} Z Axis: {2}", x_cur, y_cur, z_cur));
        return new Vector3(x_cur, y_cur, z_cur);
    }

    GameObject GetTxMenu(string transactionType)
    {
        return commonTxMenu;
        // switch (transactionType)
        // {
        //     case "NFTokenMint":
        //         return nftTxMenu;
        //     case "NFTokenBurn":
        //         return nftTxMenu;
        //     case "NFTokenOfferCreate":
        //         return nftTxMenu;
        //     case "AccountSet":
        //         return accountTxMenu;
        //     case "EscrowCancel":
        //         return paymentTxMenu;
        //     case "EscrowCreate":
        //         return paymentTxMenu;
        //     case "EscrowFinish":
        //         return paymentTxMenu;
        //     case "OfferCancel":
        //         return paymentTxMenu;
        //     case "OfferCreate":
        //         return paymentTxMenu;
        //     case "Payment":
        //         return paymentTxMenu;
        //     case "PaymentChannelClaim":
        //         return paymentTxMenu;
        //     case "PaymentChannelCreate":
        //         return paymentTxMenu;
        //     case "PaymentChannelFund":
        //         return paymentTxMenu;
        //     case "SetRegularKey":
        //         return accountTxMenu;
        //     case "SignerListSet":
        //         return accountTxMenu;
        //     case "TrustSet":
        //         return accountTxMenu;
        //     case "EnableAmendment":
        //         return accountTxMenu;
        //     case "SetFee":
        //         return accountTxMenu;
        //     case "AccountDelete":
        //         return accountTxMenu;
        //     default:
        //         return accountTxMenu;
        // }
    }

    void SpawnLedgerBoxes(Ledger ledger)
    {
        try
        {
            LedgerBinaryEntity entity = (LedgerBinaryEntity)ledger.LedgerEntity;
            for (int i = 0; i < entity.Transactions.Count; ++i) {  // X Axis
                string hash = entity.Transactions[i];
                GameObject ledgerBoxGO = Instantiate(ledgerPrefab, new Vector3(2, 4, 5), Quaternion.identity);
                LedgerBox ledgerBox = ledgerBoxGO.GetComponent<LedgerBox> ();
                ledgerBox.endPos = NextLedgerVector();
                ledgerBox.hash = hash;
                ledgerGOList.Add(ledgerBoxGO);
            }
        }
        catch (Exception e)
        {
            LogText(String.Format("{0} SpawnLedgerBoxes caught", e));
        }
    }

    void LogText(string message) {

        // Output to worldspace to help with debugging.
        if (DebugText) {
            DebugText.text += "\n" + message;
        }

        Debug.Log(message);
    }
}
