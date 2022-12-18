using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xrpl.Client;

public class XRPLManager : ScriptableObject
{
    
    public IXrplClient client;
    private static string serverUrl = "wss://s.altnet.rippletest.net:51233";

    // Start is called before the first frame update
    public void InitializeChain()
    {
        client = new XrplClient(serverUrl);
        client.Connect();
    }
}
