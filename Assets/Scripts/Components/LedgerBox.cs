using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RippleDotNet.Model.Ledger;

public class LedgerBox : MonoBehaviour
{

    public GameObject debugMenu;
    public Material PaymentMaterial;
    public Material TrustMaterial;
    public Material OfferMaterial;
    public Material EscrowMaterial;
    public Material AccountMaterial;
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
    public HashOrTransaction transaction;
    // Start is called before the first frame update

    private static IRippleClient client;
    private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    // private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    void Start()
    {
        render = GetComponent<MeshRenderer>();
        initialMaterial = render.sharedMaterial;
        render.sharedMaterial = PaymentMaterial;
        UpdateMaterial();

        client = new RippleClient(serverUrl);
        client.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            // PRIMARY TRIGGER
            Debug.Log("Z");
            active = true;
            // UpdateSwarm();
        }
        float step = speed * Time.deltaTime;
        // transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, step);

    }

    public GetTransaction()
    {

        // TransactionResponseCommon rtransaction = await rippleClient.Transaction(transaction.Transaction.TransactionHash);
        TransactionResponseCommon rtransaction = await rippleClient.Transaction("");
        // LogText(string.Format("Closed Ledger: {0}", closedLedger.LedgerHash));
    }

    public void UpdateMaterial() {
        switch (transaction.Transaction.TransactionType.ToString())
        {
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
        // active = false;

        UpdateSwarm();
    }

    public void UpdateSwarm() {
        MenuScript menuScript = debugMenu.GetComponent<MenuScript>() as MenuScript;
        menuScript.transaction = transaction;
        if (active) {
            debugMenu.SetActive(true);
        }
        else if (hovering) {
            debugMenu.SetActive(true);
        }
        else {
            debugMenu.SetActive(false);
        }
    }
}
