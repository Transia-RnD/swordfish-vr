using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using GalleryCSharp.Models;
using Newtonsoft.Json;
using Xrpl.Wallet;

public class PlayerService : MonoBehaviour
{
    public FirebaseFirestore defaultStore = FirebaseFirestore.DefaultInstance;
    
    public static Player GetPlayerFromSnapshot(DocumentSnapshot snapshot, string playerId)
    {   
        Dictionary<string, object> snapData = snapshot.ToDictionary();
        string serialized = JsonConvert.SerializeObject(snapData);
        return JsonConvert.DeserializeObject<Player>(serialized);
    }
    public static Dictionary<string, object> ParsePlayer(Player player, DocumentReference oldPlayerRef)
    {
        // player.oldPlayerRef = oldPlayerRef;
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(player.ToJson());
    }

    public static PlayerRef CloneRef(Player player)
    {
        return new PlayerRef(
            id: player.Id,
            active: true,
            deleted: false,
            createdTime: Convert.ToInt32(Utils.UnixTimeNow()),
            avatar: "",
            userName: "",
            signorAddress: player.DefaultSignor
        );
    }
    public static PlayerRef GetPlayerRefFromSnapshot(DocumentSnapshot snapshot, string playerRefId)
    {   
        Dictionary<string, object> snapData = snapshot.ToDictionary();
        string serialized = JsonConvert.SerializeObject(snapData);
        return JsonConvert.DeserializeObject<PlayerRef>(serialized);
    }
    public static Dictionary<string, object> ParsePlayerRef(PlayerRef playerRef, DocumentReference oldPlayerRefRef)
    {
        // playerRef.oldPlayerRefRef = oldPlayerRefRef;
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(playerRef.ToJson());
    }
}

