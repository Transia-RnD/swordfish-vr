using UnityEngine;
using UnityEngine.UI;

public class XRPLSetup : MonoBehaviour
{
  public XRPLManager xrplManager;
  
  void Awake()
  {
    xrplManager.InitializeChain();
  }
}