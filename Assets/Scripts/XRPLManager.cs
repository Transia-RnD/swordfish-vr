using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ripple.Binary.Codec.Types;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Ledger;
using Xrpl.Client.Requests.Ledger;
using Xrpl.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class XRPLManager : ScriptableObject
{
    
    public IRippleClient client;
    private static string serverUrl = "wss://s.altnet.rippletest.net:51233";
    // private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";

    // Start is called before the first frame update
    public void InitializeChain()
    {
        client = new RippleClient(serverUrl);
        client.Connect();
    }
}
