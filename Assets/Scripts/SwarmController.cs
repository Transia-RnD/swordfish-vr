using System.Collections;
using System.Collections.Generic;
using System;
using Plugins.SwarmUi;
using UnityEditor;
using UnityEngine;
using Ripple.Core.Types;
using RippleDotNet.Model;
using RippleDotNet.Model.Account;
using RippleDotNet.Requests.Account;
using RippleDotNet;


// [CustomEditor(typeof(SwarmController))]
// public class SwarmUiEditor : UnityEditor.Editor {
//     public override void OnInspectorGUI() {
//         DrawDefaultInspector();

//         SwarmController accountExplorer = (SwarmController) target;
//         if (GUILayout.Button("Test animation")) {
//             accountExplorer.ToggleVisibility();
//         }
//     }
// }

public class SwarmController : MonoBehaviour
{

    private static IRippleClient client;
    private static string serverUrl = "wss://xls20-sandbox.rippletest.net:51233";
    public string rootAccount = "rYTGjwGcbGRgxrdL3jBy62jnWtdV8UUmh";
    public int layerCount = 5;
    private List<string> highlightList = new List<string>();
    [SerializeField] public GameObject swarmPrefab;

    private List<GameObject> swarmList = new List<GameObject>();
    private List<string> nftList = new List<string>();

    public event Action OnShow;
    public event Action OnHide;

    private bool _isVisible;

    public void Show() {
        _isVisible = true;
        OnShow?.Invoke();
    }

    public void Hide() {
        _isVisible = false;
        OnHide?.Invoke();
    }

    public void ToggleVisibility() {
        if (_isVisible) {
            Hide();
        } else {
            Show();
        }
    }

    private async void Awake()
    {
        client = new RippleClient(serverUrl);
        client.Connect();
        GetAccount();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {   
            GetAccount();
        }
    }

    // COLUMN 1 y + 30
    // COLUMN 2 x + 20 : y + 30 : z + 20

    int RotateX(int row)
    {
        if (row == 1)
        {
            return -19;
        }
        if (row == 2)
        {
            return 0;
        }
        if (row == 3)
        {
            return 19;
        }
        return 0;
    }

    int RotateY(int column)
    {
        if (column == 1)
        {
            return -52;
        }
        if (column == 2)
        {
            return -31;
        }
        if (column == 3)
        {
            return -10;
        }
        if (column == 4)
        {
            return 10;
        }
        if (column == 5)
        {
            return 31;
        }
        if (column == 6)
        {
            return 52;
        }
        return 0;
    }

    void Generate()
    {
        int xOrig = 234;
        int yOrig = -270;
        int zOrig = 50;

        int x_matrix = 3;
        int y_matrix = 6;
        int count = 0;
        for (int i = 0; i < x_matrix; ++i) {  // X Axis
            // Debug.Log("X AXIS: " + i);
            for (int j = 0; j < y_matrix; ++j) {  // Y Axis
                count += 1;
                if (count > nftList.Count) {
                    continue;
                }
                int xTmp = xOrig + j * 25;
                int yTmp = yOrig + i * 30;
                int zTmp = zOrig + j * 20;
                // int xR = xRot + j * 25;
                // int yR = yRot + i * 30;
                // int zR = zRot + i * 20;
                // Debug.Log(string.Format("X Axis: {0} Y Axis: {1} Z Axis: {2}", xTmp, yTmp, zTmp));
                GameObject go = Instantiate(swarmPrefab, new Vector3 (xTmp, yTmp, zTmp), Quaternion.identity);
                go.transform.Rotate(
                    RotateX(i+1), 
                    RotateY(j+1), 
                    0
                );
                go.transform.SetParent(this.transform, false);
                swarmList.Add(go);
            }
        }
        for (int i = 0; i < swarmList.Count; ++i) {
            UiAnimator animator = swarmList[i].GetComponent<UiAnimator>();
            animator.Load(this);
        }
    }
    async void GetAccount()
    {
        AccountNFTs accountNFTs = await client.AccountNFTs(rootAccount);
        Debug.Log(accountNFTs.NFTs);
        for (int i = 0; i < accountNFTs.NFTs.Count; ++i) {
            nftList.Add(accountNFTs.NFTs[i].URIAsString);
        }
        Generate();
        // 1. Get Account Transactions
        // 2. Get Account NFT Offers
        // 3. Get Account NFTs
        // 4. Get Account Details
        // 5. Get Account Pending...
    }
}