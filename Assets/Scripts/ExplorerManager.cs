using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Converters;
using GalleryCSharp.Models;
using Xrpl.Client;
using Xrpl.Models.Methods;
using Xrpl.Models.Ledger;
using Xrpl.Models.Common;
// using System.Net.WebSockets;
using NativeWebSocket;
using Xrpl.Models.Transaction;

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

    private List<LOLedger> ledgerList = new List<LOLedger>();
    private List<GameObject> ledgerGOList = new List<GameObject>();

    private static IXrplClient client;
    private static WebSocket websocket;
    private static string serverUrl = "ws://35.232.209.204:6006";

    private List<Dictionary<string, dynamic>> transactions = new List<Dictionary<string, dynamic>>();

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

    bool resetting = false;


    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket(serverUrl);

        websocket.OnOpen += () =>
        {
            LogText("Connection open!");
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            serializerSettings.FloatParseHandling = FloatParseHandling.Double;
            serializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;

            var ledgerClosedReq = new SubscribeRequest()
            {
                Id = Guid.NewGuid(),
                Streams = new List<string>(new[]
                    {
                        "ledger",
                    })
            };

            var request = new SubscribeRequest()
            {
                Id = Guid.NewGuid(),
                Streams = new List<string>(new[]
                    {
                        "transactions",
                    })
            };
            string ledgerClosedString = JsonConvert.SerializeObject(ledgerClosedReq, serializerSettings);
            SendWebSocketMessage(ledgerClosedString);
            string jsonString = JsonConvert.SerializeObject(request, serializerSettings);
            SendWebSocketMessage(jsonString);
        };

        websocket.OnError += (e) =>
        {
            LogText("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            LogText("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            // LogText($"MESSAGE: {message}");
            Dictionary<string, dynamic> data;
            try
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message);
                // LogText(data["type"]);
            }
            catch (Exception error)
            {
                LogText(error.Message);
                return;
            }
            if (data["type"] == "response")
            {
                // UpdateMaterial(data);
                return;
            }
            if (data["type"] == "transaction")
            {
                // transactions.Add(data);
                SpawnLedgerBox(data);
                return;
            }
            if (data["type"] == "ledgerClosed")
            {
                ResetLedgerBoxes();
                return;
            }
        };

        await websocket.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
        #endif
    }

    private async void OnApplicationQuit()
    {
        LogText("DISCONNECTING...");
        await websocket.Close();
    }

    async void SendWebSocketMessage(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
        // Sending plain text
        await websocket.SendText(message);
        }
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

    void ResetLedgerBoxes()
    {
        resetting = true;
        LogText($"RESETTING LEDGER: {ledgerGOList.Count}");
        for (int i = 0; i < ledgerGOList.Count; ++i) {
            GameObject obj = ledgerGOList[i];
            LedgerBox ledgerBox = obj.GetComponent<LedgerBox> ();
            // ledgerGOList.Remove(obj);
            // obj.SetActive(false);
            // Destroy(ledgerBox.gameObject);
            Destroy(obj.gameObject);
        }
        transactions = new List<Dictionary<string, dynamic>>();
        ledgerGOList = new List<GameObject>();
        x_matrix = 3;
        x_cur = 1;
        y_matrix = 3;
        y_cur = 1;
        z_cur = 1;
        resetting = false;
    }

    void SpawnLedgerBox(Dictionary<string, dynamic> data)
    {
        try
        {
            GameObject ledgerBoxGO = Instantiate(ledgerPrefab, new Vector3(2, 4, 5), Quaternion.identity);
            LedgerBox ledgerBox = ledgerBoxGO.GetComponent<LedgerBox> ();
            ledgerBox.endPos = NextLedgerVector();
            ledgerBox.data = data;
            ledgerGOList.Add(ledgerBoxGO);
        }
        catch (Exception e)
        {
            LogText(String.Format("{0} SpawnLedgerBoxes caught", e));
        }
    }

    // void SpawnLedgerBoxes()
    // {
    //     try
    //     {
    //         for (int i = 0; i < transactions.Count; ++i) {  // X Axis

    //             GameObject ledgerBoxGO = Instantiate(ledgerPrefab, new Vector3(2, 4, 5), Quaternion.identity);
    //             LedgerBox ledgerBox = ledgerBoxGO.GetComponent<LedgerBox> ();
    //             ledgerBox.endPos = NextLedgerVector();
    //             ledgerBox.data = data;
    //             ledgerGOList.Add(ledgerBoxGO);
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         LogText(String.Format("{0} SpawnLedgerBoxes caught", e));
    //     }
    // }

    void LogText(string message) {

        // Output to worldspace to help with debugging.
        if (DebugText) {
            DebugText.text += "\n" + message;
        }

        Debug.Log(message);
    }
}
