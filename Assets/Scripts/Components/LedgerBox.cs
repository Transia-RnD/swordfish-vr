using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RippleDotNet.Model.Ledger;
using RippleDotNet;
using Ripple.Core.Types;
using RippleDotNet.Model;
using RippleDotNet.Model.Transaction.TransactionTypes;
using RippleDotNet.Requests.Transaction;
using RippleDotNet.Responses.Transaction.Interfaces;
using RippleDotNet.Responses.Transaction.TransactionTypes;
using RippleDotNet.Extensions;
using System.Threading.Tasks;
using Ipfs.Http;
using IO.Swagger.Model;

public class LedgerBox : MonoBehaviour
{

    public GameObject debugMenu;
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
    public HashOrTransaction transaction;
    // Start is called before the first frame update

    void Start()
    {
        render = GetComponent<MeshRenderer>();
        initialMaterial = render.sharedMaterial;
        render.sharedMaterial = PaymentMaterial;
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, step);
    }

    public void UpdateMaterial() {
        if (transaction == null) { return; }
        Debug.Log(transaction.Transaction.TransactionType.ToString());
        switch (transaction.Transaction.TransactionType.ToString())
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
