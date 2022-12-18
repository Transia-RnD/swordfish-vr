using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ipfs.Http;
using GalleryCSharp.Models;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Xrpl.Models.Transaction;
using Xrpl.Models.Subscriptions;
using Xrpl.Models.Methods;
using Newtonsoft.Json;

public class CommonTxMenu : MonoBehaviour
{
    public Dictionary<string, dynamic> data;
    public GameObject TransactionType;
    public GameObject Result;
    public GameObject Hash;
    public GameObject DateText;
    public GameObject IndexText;
    public GameObject AccountText;
    public GameObject SequenceText;
    public GameObject FeeText;

    void SetValue(GameObject obj, string name)
    {
        Text text = obj.GetComponent<Text>();
        text.text = name;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateMenu()
    {
        Debug.Log("START MENU");
        if (data != null)
        {
            string txString = JsonConvert.SerializeObject(data);
            TransactionStream tx = JsonConvert.DeserializeObject<TransactionStream>(txString);
            Debug.Log(tx.Transaction.Date);
            Debug.Log(tx.Transaction.LedgerIndex);
            Debug.Log(tx.Transaction.Account);
            Debug.Log(tx.Transaction.Sequence);
            Debug.Log(tx.Transaction.Fee.Value);
            SetValue(
                TransactionType,
                tx.Transaction.TransactionType.ToString()
            );
            SetValue(
                Hash,
                tx.Transaction.Hash.ToString()
            );
            SetValue(
                DateText,
                tx.Transaction.Date.ToString()
            );
            SetValue(
                IndexText,
                tx.Transaction.LedgerIndex.ToString()
            );
            SetValue(
                AccountText,
                tx.Transaction.Account.ToString()
            );
            SetValue(
                SequenceText,
                tx.Transaction.Sequence.ToString()
            );
            SetValue(
                FeeText,
                tx.Transaction.Fee.Value.ToString()
            );
        }
    }
}
