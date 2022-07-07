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

public class GalleryService : MonoBehaviour
{
    public FirebaseFirestore defaultStore;
    // public IpfsClient ipfs;

    // public void Awake()
    // {
    //     // ipfs = new IpfsClient("https://ipfs.io/ipfs");
    //     // GetIPFSGallery("QmYRpYnBXVDSvJ19LvfhL9Rgv92h9LAQGeKPjT936dtsim");
    // }

    public static async Task<IPFSGallery> GetIPFSGallery(IpfsClient ipfs, string cid)
    {
        string text = await ipfs.FileSystem.ReadAllTextAsync(cid);
        return GetGalleryFromIPFS(
            text,
            cid
        );
        // string[] tokenIds = ipfsGallery.Data.Message.TokenIds;
        // for (int i = 0; i < tokenIds.Length; i++)
        // {
        //     Debug.Log(tokenIds[i]);
        // }
        // const string tokenCid = "QmeJe2A4FydhwiMcHr8P1LiyZcgJryHPNWsf9bsTQ281nk";
        // string token_meta = await ipfs.FileSystem.ReadAllTextAsync(tokenCid);
        // Debug.Log(token_meta);
        // const string imageCid = "QmVvUy7ZkPusUbALMPiEV1vuhaMZM7JBEjZ5TyfNPx6PA8";
        // string image_data = await ipfs.FileSystem.ReadFileAsync(imageCid);
        // Debug.Log(image_data);
    }

    // public async void GetGallery(string galleryId)
    // {
    //     defaultStore = FirebaseFirestore.DefaultInstance;
    //     DocumentReference galleryRef = defaultStore.Collection("Galleries").Document(galleryId);
    //     galleryRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    //     {
    //     DocumentSnapshot snapshot = task.Result;
    //     if (snapshot.Exists) {
    //         Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
    //         Dictionary<string, object> city = snapshot.ToDictionary();
    //         foreach (KeyValuePair<string, object> pair in city) {
    //         Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
    //         }
    //     } else {
    //         Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
    //     }
    //     });
    // }

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

    public static IPFSGallery GetGalleryFromIPFS(string serialized, string galleryCid)
    {   
        return JsonConvert.DeserializeObject<IPFSGallery>(serialized);
    }

    public static Gallery GetGalleryFromSnapshot(DocumentSnapshot snapshot, string galleryId)
    {   
        Dictionary<string, object> snapData = snapshot.ToDictionary();
        string serialized = JsonConvert.SerializeObject(snapData);
        return JsonConvert.DeserializeObject<Gallery>(serialized);
    }
    public static Dictionary<string, object> ParseWallet(Gallery gallery, DocumentReference oldGalleryRef)
    {
        // gallery.oldGalleryRef = oldGalleryRef;
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(gallery.ToJson());
    }
}