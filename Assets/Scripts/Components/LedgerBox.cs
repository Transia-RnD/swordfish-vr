using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Xrpl.Client.Model.Ledger;
using Xrpl.Client;
using Ripple.Binary.Codec.Types;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Transaction.TransactionTypes;
using Xrpl.Client.Requests.Transaction;
using Xrpl.Client.Responses.Transaction.Interfaces;
using Xrpl.Client.Responses.Transaction.TransactionTypes;
using Xrpl.Client.Extensions;
using System.Threading.Tasks;
using Ipfs.Http;
using IO.Swagger.Model;

public class LedgerBox : MonoBehaviour
{

    public Material PaymentMaterial;
    public Material TrustMaterial;
    public Material OfferMaterial;
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
    public float speed = 8.0f;
    public string hash;
    public ITransactionResponseCommon transaction;
    // Start is called before the first frame update

    private static IRippleClient client;
    private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    // private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    async void Start()
    {
        client = new RippleClient(serverUrl);
        client.Connect();
        render = GetComponent<MeshRenderer>();
        initialMaterial = render.sharedMaterial;
        render.sharedMaterial = PaymentMaterial;
        // await GetTransaction();
    }
    public async Task GetTransaction()
    {
        Debug.Log(string.Format("GET TRANSACTION: {0}", hash));
        ITransactionResponseCommon itransaction = await client.Transaction(hash);
        transaction = itransaction;
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, step);
    }

    public void UpdateMaterial() {
        // if (transaction == null) { return; }
        Debug.Log(transaction.TransactionType.ToString());
        switch (transaction.TransactionType.ToString())
        {
            case "NFTokenMint":
                render.sharedMaterial = NFTMaterial;
                // Debug.Log(transaction.Transaction);
                break;
            case "NFTokenBurn":
                render.sharedMaterial = NFTMaterial;
                // Debug.Log(transaction.Transaction);
                break;
            // case "NFTokenOfferCreate":
            //     render.sharedMaterial = NFTMaterial;
            //     // Debug.Log(transaction.Transaction);
            //     break;
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
                render.sharedMaterial = OfferMaterial;
                break;
            case "OfferCreate":
                render.sharedMaterial = OfferMaterial;
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

        UpdateSwarm();
    }

    // No longer ohlding down activate
    public void SetInactive(PointerEventData eventData) {
        active = false;

        UpdateSwarm();
    }

    // Hovering over our object
    public void SetHovering(PointerEventData eventData) {
        hovering = true;

        UpdateSwarm();
    }

    // No longer hovering over our object
    public void ResetHovering(PointerEventData eventData) {
        hovering = false;

        UpdateSwarm();
    }

    public void UpdateSwarm() {
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
