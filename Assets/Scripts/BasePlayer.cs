using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasePlayer : MonoBehaviour
{

    public GameObject menuHolder;
    private GameObject txMenu;
    private CommonTxMenu menuScript;

    // Currently activating the object?
    bool active = false;

    // Currently hovering over the object?
    bool hovering = false;
    
    void Start()
    {
        // GameObject.Instantiate(spawnMenu, new Vector3(1,1,0),Quaternion.identity);
        // menuScript = spawnMenu.GetComponent<CommonTxMenu>() as CommonTxMenu;
        // Debug.Log(menuScript);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetActive(PointerEventData eventData) {
        Debug.Log(eventData.pointerEnter.transform.name);
        // if (eventData.pointerEnter.transform.parent.tag == "Slider")
        //     activeSlider = GameObject.Find(eventData.pointerEnter.transform.parent.name).GetComponent<Slider>();
        // else if (eventData.pointerEnter.transform.parent.parent.tag == "Slider")
        //     activeSlider = GameObject.Find(eventData.pointerEnter.transform.parent.parent.name).GetComponent<Slider>();
        // }
        active = true;

        UpdateSwarm(eventData);
    }

    // No longer ohlding down activate
    public void SetInactive(PointerEventData eventData) {
        Debug.Log(eventData.pointerEnter.transform.name);
        active = false;

        UpdateSwarm(eventData);
    }

    // Hovering over our object
    public void SetHovering(PointerEventData eventData) {
        hovering = true;

        UpdateSwarm(eventData);
    }

    // No longer hovering over our object
    public void ResetHovering(PointerEventData eventData) {
        Debug.Log(eventData.pointerEnter.transform.name);
        hovering = false;

        UpdateSwarm(eventData);
    }

    public void UpdateSwarm(PointerEventData eventData) {
        
        if (active) 
        {
            Debug.Log("ACTIVE");
            GameObject menuHolder = GameObject.Find("MenuHolder");
            txMenu = menuHolder.transform.GetChild(0).gameObject;
            menuScript = txMenu.GetComponent<CommonTxMenu>() as CommonTxMenu;
            LedgerBox ledgerBox = GameObject.Find(eventData.pointerEnter.transform.name).GetComponent<LedgerBox>();
            menuScript.data = ledgerBox.data;
            menuScript.UpdateMenu();
            txMenu.SetActive(true);
        } 
        else if (hovering) 
        {
            Debug.Log("HOVERING");
            GameObject menuHolder = GameObject.Find("MenuHolder");
            txMenu = menuHolder.transform.GetChild(0).gameObject;
            menuScript = txMenu.GetComponent<CommonTxMenu>() as CommonTxMenu;
            LedgerBox ledgerBox = GameObject.Find(eventData.pointerEnter.transform.name).GetComponent<LedgerBox>();
            menuScript.data = ledgerBox.data;
            menuScript.UpdateMenu();
            txMenu.SetActive(true);
        } 
        else 
        {
            txMenu.SetActive(false);
        }
    }
}
