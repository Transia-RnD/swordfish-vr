using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using GalleryCSharp.Models;
using Newtonsoft.Json;
using Ipfs.Http;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Xrpl.Wallet;

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
}