using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Ipfs.Http;
using GalleryCSharp.Models;

public class GalleryManager : MonoBehaviour
{
    private GalleryFrameService service;
    public IpfsClient ipfs;

    // North Cupboard
    // North Round Display
    // NorthEast Display
    // NorthEast Cupboard
    // NorthWest Display
    // NorthWest Cupboard
    // NorthWest Frame
    [SerializeField] public GameObject nwframe001;
    [SerializeField] public GameObject nwframe002;
    // NorthEast Frame
    [SerializeField] public GameObject neframe001;
    [SerializeField] public GameObject neframe002;
    // SouthWest Frame
    [SerializeField] public GameObject swframe001;
    [SerializeField] public GameObject swframe002;
    // SouthEast Frame
    [SerializeField] public GameObject seframe001;
    [SerializeField] public GameObject seframe002;

    [SerializeField] public GameObject nwframeBox001;

    // void ResetNFTImage(Material m, string cid)
    // {
    //     // Material m = nwframe001.GetComponent<MeshRenderer>().material;
    //     // m.color = new Color(1,1,1,.5f);
    //     // string imageCid = "QmVvUy7ZkPusUbALMPiEV1vuhaMZM7JBEjZ5TyfNPx6PA8";
    //     // string url = "https://ipfs.io/ipfs/" + cid;
    //     StartCoroutine(downloadImage("https://ipfs.io/ipfs/" + cid, m));
    // }

    async void Start()
    {
        Debug.Log("STARING GALLERY MANAGER");
        service = new GalleryFrameService();
        List<Frame> frames = await service.GetGalleryFrames("MleOWbsG4tdArtyq34Ft");
        List<GameObject> objects = new List<GameObject>(); 
        objects.Add(nwframe001);
        objects.Add(nwframe002);
        objects.Add(neframe001);
        objects.Add(neframe002);
        for (int i = 0; i < frames.Count; i++)
        {
            string image = frames[i].Image;
            Debug.Log(image);
            Material m = objects[i].GetComponent<MeshRenderer>().material;
            StartCoroutine(downloadImage(image, m));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator downloadImage(string uri, Material m) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            m.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
