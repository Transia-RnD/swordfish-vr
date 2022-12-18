using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Ipfs.Http;
using GalleryCSharp.Models;
using Xrpl.Client;
using Xrpl.Models.Transaction;
using Xrpl.Models.Subscriptions;
using Xrpl.Models.Methods;
using Newtonsoft.Json;

public class LedgerBox : MonoBehaviour
{

    public Material PaymentMaterial;
    public Material TrustMaterial;
    public Material OfferCreateMaterial;
    public Material OfferCancelMaterial;
    public Material EscrowMaterial;
    public Material AccountMaterial;
    public Material NFTMaterial;
    public Material MiscMaterial;

    Material initialMaterial;

    MeshRenderer render;

    // Currently activating the object?
    bool active = false;

    // Currently hovering over the object?
    bool hovering = false;

    public string txId = "";
    public Vector3 endPos = new Vector3(0, 0, 0);
    public float speed = 100.0f;
    public string hash;
    public Dictionary<string, dynamic> data;
    // Start is called before the first frame update

    private static IXrplClient client;
    private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    // private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    async void Start()
    {
        render = GetComponent<MeshRenderer>();
        initialMaterial = render.sharedMaterial;
        render.sharedMaterial = PaymentMaterial;
        if (data != null) 
        {
            UpdateMaterial(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, step);
    }

    public void UpdateMaterial(Dictionary<string, dynamic> data) {
        // if (transaction == null) { return; }
        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.NullValueHandling = NullValueHandling.Ignore;
        serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        serializerSettings.FloatParseHandling = FloatParseHandling.Double;
        serializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
        // Debug.Log((string)data["transaction"]["TransactionType"]);
        switch ((string)data["transaction"]["TransactionType"])
        {
            case "NFTokenMint":
                render.sharedMaterial = NFTMaterial;
                // var deserialized = JsonConvert.DeserializeObject((string)data["transaction"], typeof(NFTokenMint), serializerSettings);
                break;
            case "NFTokenBurn":
                render.sharedMaterial = NFTMaterial;
                // Debug.Log(transaction.Transaction);
                break;
            case "NFTokenOfferCreate":
                render.sharedMaterial = NFTMaterial;
                // Debug.Log(transaction.Transaction);
                break;
            case "AccountSet":
                render.sharedMaterial = AccountMaterial;
                break;
            case "EscrowCancel":
                render.sharedMaterial = EscrowMaterial;
                break;
            case "EscrowCreate":
                render.sharedMaterial = EscrowMaterial;
                break;
            case "EscrowFinish":
                render.sharedMaterial = EscrowMaterial;
                break;
            case "OfferCancel":
                render.sharedMaterial = OfferCancelMaterial;
                break;
            case "OfferCreate":
                render.sharedMaterial = OfferCreateMaterial;
                break;
            case "Payment":
                render.sharedMaterial = PaymentMaterial;
                break;
            case "PaymentChannelClaim":
                render.sharedMaterial = PaymentMaterial;
                break;
            case "PaymentChannelCreate":
                render.sharedMaterial = PaymentMaterial;
                break;
            case "PaymentChannelFund":
                render.sharedMaterial = PaymentMaterial;
                break;
            case "SetRegularKey":
                render.sharedMaterial = TrustMaterial;
                break;
            case "SignerListSet":
                render.sharedMaterial = TrustMaterial;
                break;
            case "TrustSet":
                render.sharedMaterial = TrustMaterial;
                break;
            case "EnableAmendment":
                render.sharedMaterial = MiscMaterial;
                break;
            case "SetFee":
                render.sharedMaterial = MiscMaterial;
                break;
            case "AccountDelete":
                render.sharedMaterial = AccountMaterial;
                break;
        }
    }

    // Holding down activate
    public void SetActive(PointerEventData eventData) {
        active = true;
        Debug.Log("LEDGER BOX: SET ACTIVE");
        UpdateSwarm();
    }

    // No longer ohlding down activate
    public void SetInactive(PointerEventData eventData) {
        active = false;
        Debug.Log("LEDGER BOX: SET INACTIVE");
        UpdateSwarm();
    }

    // Hovering over our object
    public void SetHovering(PointerEventData eventData) {
        hovering = true;

        Debug.Log("LEDGER BOX: SET HOVERING");
        UpdateSwarm();
    }

    // No longer hovering over our object
    public void ResetHovering(PointerEventData eventData) {
        hovering = false;
        Debug.Log("LEDGER BOX: RESET HOVERING");
        UpdateSwarm();
    }

    public void UpdateSwarm() {
        Debug.Log("LEDGER BOX: UPDATE SWARM");
        if (active) {
            // Debug.Log("ACTIVE");
        }
        else if (hovering) {
            // Debug.Log("HOVERING");
        }
        else {
            // Debug.Log("NONE");
        }
    }
}
