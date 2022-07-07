using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Xrpl.Client;
using Xrpl.Client.Model.Ledger;
using Xrpl.Client.Responses.Transaction.Interfaces;
using Ipfs.Http;
using IO.Swagger.Model;
using System.Threading.Tasks;
using UnityEngine.Networking;

// using Xrpl.Client.Responses.Transaction.Interfaces;
// using Xrpl.Client.Responses.Transaction.TransactionTypes;

public class CommonTxMenu : MonoBehaviour
{
    public ITransactionResponseCommon transaction;
    public GameObject AccountText;
    public GameObject TxTypeText;
    public GameObject FeeText;

    // void SetValue(GameObject obj, string name, string value)
    // {
    //     Text text = obj.GetComponent<Text>();
    //     text.text = string.Format(
    //         "{0}: {1}",
    //         name,
    //         value
    //     );
    // }

    // Start is called before the first frame update
    void Start()
    {
        // SetValue(
        //     AccountText,
        //     "Account",
        //     transaction.Transaction.Account.ToString()
        // );
        // SetValue(
        //     TxTypeText,
        //     "TransactionType",
        //     transaction.Transaction.TransactionType.ToString()
        // );
        // SetValue(
        //     FeeText,
        //     "Fee",
        //     transaction.Transaction.Fee.ToString()
        // );
    }

    // Update is called once per frame
    void Update()
    {

    }

    // public async Task GetTransaction()
    // {
        // TransactionResponseCommon rtransaction = await client.Transaction(transaction.Transaction.TransactionHash);
        // ITransactionResponseCommon response = await client.Transaction("97DB76DC957E9CAFDB01435E7227AC4B5D455467BCFE1876F0C4700C3B3BCCF8");
        // NFTokenMintTransactionResponse rtransaction = (NFTokenMintTransactionResponse)transaction.Transaction;
        // NFTokenMintTransactionResponse rtransaction = (NFTokenMintTransactionResponse)response;
        
    // }
}
