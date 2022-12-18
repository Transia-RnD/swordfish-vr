using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using GalleryCSharp.Models;
using Newtonsoft.Json;
using Ipfs.Http;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Xrpl.Wallet;

public class GalleryFrameService
{
    public FirebaseFirestore defaultStore;

    public async Task<List<Frame>> GetGalleryFrames(string galleryId)
    {
        defaultStore = FirebaseFirestore.DefaultInstance;
        DocumentReference galleryRef = defaultStore.Collection("Gallerys").Document(galleryId);
        Query query = galleryRef.Collection("Locations");
        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
        List<Frame> frameList = new List<Frame>();
        foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents) {
            Frame frame = GetFrameFromSnapshot(documentSnapshot, documentSnapshot.Id);
            frameList.Add(frame);
        };
        return frameList;
        // query.GetSnapshotAsync().ContinueWithOnMainThread(task => {
        //     QuerySnapshot querySnapshot = task.Result;
        //     List<Frame> frameList = new List<Frame>();
        //     foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents) {
        //         Frame frame = GetFrameFromSnapshot(documentSnapshot, documentSnapshot.Id);
        //         frameList.Add(frame);
        //         Debug.Log(frame.Image);
        //     };
        // });
    }

    public static Frame GetFrameFromSnapshot(DocumentSnapshot snapshot, string frameId)
    {
        try
        {
            Dictionary<string, object> snapData = snapshot.ToDictionary();
            string serialized = JsonConvert.SerializeObject(snapData);
            return JsonConvert.DeserializeObject<Frame>(serialized);
        } catch (Exception e) {
            Debug.Log(e);
            throw e;
        }
    }
    public static Dictionary<string, object> ParseFrame(Frame frame, DocumentReference oldFrameRef)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(frame.ToJson());
    }
}