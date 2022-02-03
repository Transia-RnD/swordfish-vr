using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RippleDotNet.Model.Ledger;

public class MenuScript : MonoBehaviour
{
    public HashOrTransaction transaction;
    public GameObject DebugText;

    // Start is called before the first frame update
    void Start()
    {
        Text debugText = DebugText.GetComponent<Text>();
        // Debug.Log(transaction.TransactionHash);
        ShowDefault();
    }

    void ShowPayment()
    {
        Text debugText = DebugText.GetComponent<Text>();
        debugText.text = string.Format(
            "Account: {1}",
            transaction.Transaction.Account
        );
    }

    void ShowDefault()
    {
        Text debugText = DebugText.GetComponent<Text>();
        debugText.text = string.Format(
            "TransactionType: {0} \nAccount: {1}", 
            transaction.Transaction.TransactionType,
            transaction.Transaction.Account
        );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
