using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrpl.Wallet;
using Firebase;
using Firebase.Firestore;
using IO.Swagger.Model;
using Newtonsoft.Json;
using Ipfs.Http;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class MetaService : MonoBehaviour
{
    public FirebaseFirestore defaultStore;

    public string ConvertIPFSURL(string uri)
    {
        // string pattern = "ipfs://";
        // if (uri.Contains(pattern)) {
        //     string[] words = uri.Split(pattern);
        //     Debug.Log(words[-1]);
        //     return "https://ipfs.io/ipfs/" + words[-1];
        // }
        // string _pattern = "ipfs/";
        // string[] _words = uri.Split(_pattern);
        // return "https://ipfs.io/ipfs/" + _words[-1];
        return "https://ipfs.io/ipfs/" + "";
    }

    public static async Task<IPFSTokenMetaData> GetIPFSMeta(IpfsClient ipfs, string cid)
    {
        string text = await ipfs.FileSystem.ReadAllTextAsync(cid);
        return GetMetaFromIPFS(
            text,
            cid
        );
    }

    public static IEnumerator downloadImage(string uri, Material m)
    {
        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);

        DownloadHandler handle = www.downloadHandler;

        //Send Request and wait
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Error while Receiving: " + www.error);
        }
        else
        {
            Debug.Log("Success");
            //Load Image
            // Texture2D texture2d = DownloadHandlerTexture.GetContent(www);
            m.mainTexture = DownloadHandlerTexture.GetContent(www);
        }
    }

    public static IPFSTokenMetaData GetMetaFromIPFS(string serialized, string cid)
    {   
        return JsonConvert.DeserializeObject<IPFSTokenMetaData>(serialized);
    }
}