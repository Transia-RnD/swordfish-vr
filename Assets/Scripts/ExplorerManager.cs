using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ripple.Core.Types;
using RippleDotNet.Model;
using RippleDotNet.Model.Ledger;
using RippleDotNet.Requests.Ledger;
using RippleDotNet;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    [SerializeField] public GameObject debugMenu;

    public float speed = 7.0f;

    private List<Ledger> ledgerList = new List<Ledger>();
    private List<GameObject> ledgerGOList = new List<GameObject>();

    private static IRippleClient client;
    // private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

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
        client = new RippleClient(serverUrl);
        client.Connect();
        InvokeRepeating("AddLedger", 2.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {   
            // AddLedger();

        }
    }

    public async Task AddLedger()
    {
        var request = new LedgerRequest {
            LedgerIndex = new LedgerIndex(LedgerIndexType.Validated),
            Transactions = true,
            Expand = true
        };
        BaseLedgerInfo closedLedger = await client.ClosedLedger();
        LogText(string.Format("Closed Ledger: {0}", closedLedger.LedgerHash));
        request.LedgerHash = closedLedger.LedgerHash;
        Ledger ledger = await client.Ledger(request);
        LogText(string.Format("TXs: {0}", ledger.LedgerEntity.Transactions.Count));
        if (!ledgerList.Contains(ledger))
        {
            ledgerList.Add(ledger);
            SpawnLedgerBoxes(ledger);
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

    void SpawnLedgerBoxes(Ledger ledger)
    {
        for (int i = 0; i < ledger.LedgerEntity.Transactions.Count; ++i) {  // X Axis
            // LogText(string.Format("X Axis: {0} Y Axis: {1} Z Axis: {2}", xTmp, yTmp, zTmp));
            GameObject ledgerBoxGO = Instantiate(ledgerPrefab, new Vector3(2, 4, 5), Quaternion.identity);
            LedgerBox ledgerBox = ledgerBoxGO.GetComponent<LedgerBox> ();
            ledgerBox.endPos = NextLedgerVector();
            ledgerBox.debugMenu = debugMenu;
            ledgerBox.transaction = ledger.LedgerEntity.Transactions[i];
            ledgerGOList.Add(ledgerBoxGO);
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
