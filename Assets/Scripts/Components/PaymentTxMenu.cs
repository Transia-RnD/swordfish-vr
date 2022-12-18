using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ipfs.Http;
using GalleryCSharp.Models;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Xrpl.Models.Ledger;

public class PaymentTxMenu : MonoBehaviour
{
    public HashOrTransaction transaction;
    // public PaymentTransactionResponse transaction;
    public GameObject AccountText;
    public GameObject TxTypeText;
    public GameObject DestText;
    public GameObject DestTagText;
    public GameObject CurrText;
    public GameObject AmountText;

    void SetValue(GameObject obj, string name, string value)
    {
        Text text = obj.GetComponent<Text>();
        text.text = string.Format(
            "{0}: {1}",
            name,
            value
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        // PaymentTransactionResponse transaction = (PaymentTransactionResponse)ctransaction.Transaction;
        SetValue(
            AccountText,
            "Account",
            transaction.Transaction.Account.ToString()
        );
        SetValue(
            TxTypeText,
            "TransactionType",
            transaction.Transaction.TransactionType.ToString()
        );
        // SetValue(
        //     DestText,
        //     "Destination",
        //     transaction.Transaction.Destination.ToString()
        // );
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
        
    }
}
