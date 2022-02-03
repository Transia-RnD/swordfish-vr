using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Swordfish;
using IO.Swagger.Model;

public class WalletUI : MonoBehaviour
{

    private XrpService xrpService;
    private WalletService walletService;

    [Header("WalletUI")]
    public Button uiButton;
    public Text titleText;
    public Text descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        walletService = new WalletService();
        xrpService = new XrpService();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            CreateWallet();
        }
    }

    public void CreateWallet()
    {
        Wallet wallet = xrpService.CreateXrpWallet("123456654321");
        walletService.CreateWallet(wallet, "123456654321");
    }
}
