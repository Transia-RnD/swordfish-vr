using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Ipfs.Http;
using IO.Swagger.Model;

public class GalleryManager : MonoBehaviour
{
    public IpfsClient ipfs;

    // North Cupboard
    // North Round Display
    // NorthEast Display
    // NorthEast Cupboard
    // NorthWest Display
    // NorthWest Cupboard
    // NorthWest Frame
    [SerializeField] public GameObject nwframe001;
    [SerializeField] public GameObject nwframeBox001;
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

    void ResetNFTImage(Material m, string cid)
    {
        // Material m = nwframe001.GetComponent<MeshRenderer>().material;
        // m.color = new Color(1,1,1,.5f);
        // string imageCid = "QmVvUy7ZkPusUbALMPiEV1vuhaMZM7JBEjZ5TyfNPx6PA8";
        // string url = "https://ipfs.io/ipfs/" + cid;
        StartCoroutine(downloadImage("https://ipfs.io/ipfs/" + cid, m));
    }
    async void Start()
    {
        ipfs = new IpfsClient("https://ipfs.io/ipfs");
        IPFSGallery gallery = await GalleryService.GetIPFSGallery(
            ipfs,
            "QmYRpYnBXVDSvJ19LvfhL9Rgv92h9LAQGeKPjT936dtsim"
        );
        List<string> tokenIds = gallery.Data.Message.TokenIds;
        tokenIds.ForEach(delegate(string tokenId)
        {
            Debug.Log(tokenId);
        });
        // GameObject[,] objects;
        // objects = new GameObject[nwframe001, nwframe002];
        // for (int i = 0; i < objects.Length; i++)
        // {

        // }
        // m.color = new Color(1,1,1,.5f);
        // string imageCid = "QmVvUy7ZkPusUbALMPiEV1vuhaMZM7JBEjZ5TyfNPx6PA8";
        // string url = "https://ipfs.io/ipfs/" + imageCid;
        // StartCoroutine(downloadImage(url, m));
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
