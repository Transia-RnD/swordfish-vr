using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using GalleryCSharp.Models;
using AppWallet = GalleryCSharp.Models.Wallet;
using Newtonsoft.Json;

public class WalletService : MonoBehaviour
{
    public FirebaseFirestore defaultStore;

    public string errorPointer;

    public void CreateWallet(AppWallet wallet, string xummToken)
    {
        defaultStore = FirebaseFirestore.DefaultInstance;
        // GambitAnalytics.creatingWallet()
        // guard let playerClone = AppState.sharedInstance.selfPlayer else {
        //     createDelegate?.createWalletFailure(error: ErrorCodes.getError("BADC1")) //BADC1
        //     GambitAnalytics.errorHappened()
        //     return
        // }
        bool isLoggedIn = GameState.Instance.isLoggedIn;
        Player playerClone = GameState.Instance.selfPlayer;
        // if (playerClone)
        // {
        //     // createDelegate?.createWalletFailure(error: ErrorCodes.getError("BADC1")) //BADC1
        //     // SwordfishAnalytics.errorHappened()

        //     // Trigger/Handle Error
        //     // Analytic

        //     return;
        // }
        string signorAddress = wallet.Address;
        
        DocumentReference newPlayerRef = defaultStore.Collection("Players").Document(playerClone.Id);
        DocumentReference newPlayerRefRef = defaultStore.Collection("PlayerRefs").Document(playerClone.Id);
        DocumentReference clonePlayerRef = newPlayerRef.Collection("LegacyPlayers").Document();
        DocumentReference newWalletRef = newPlayerRef.Collection("Wallets").Document(signorAddress);
        
        Int32 updatedTime = Convert.ToInt32(Utils.UnixTimeNow());

        defaultStore.RunTransactionAsync(transaction =>
        {
            return transaction.GetSnapshotAsync(newPlayerRef).ContinueWith((snapshotTask) =>
            {
                try {
                    DocumentSnapshot playerSnapshot = snapshotTask.Result;
                    if (!playerSnapshot.Exists) {
                        errorPointer = "Player Snapshot Doesn't Exists";
                        // errorPointer?.pointee = fetchError;
                        // return nil;
                        return;
                    }
                    Player oldPlayer = PlayerService.GetPlayerFromSnapshot(
                        snapshot: playerSnapshot,
                        playerId: playerSnapshot.Id
                    );
                    Dictionary<string, object> playerData = playerSnapshot.ToDictionary();

                    // TODO: MERGE TRUE
                    transaction.Set(
                        clonePlayerRef,
                        PlayerService.ParsePlayer(
                            oldPlayer,
                            newPlayerRef
                        )
                    );
                    // DocumentReference oldDocRef = playerData["oldPlayerRef"];
                    // Dictionary<string, object> newPlayerDict = new Dictionary<string, object>()
                    // {
                    //     { "newPlayerRef", clonePlayerRef}
                    // };
                    // if (oldDocRef) {
                    //     transaction.Update(
                    //         oldDocRef,
                    //         newPlayerDict
                    //     );
                    // }
                    
                    playerClone.DefaultSignor = signorAddress;
                    playerClone.UpdatedTime = updatedTime;
                    // playerClone.wallets[$"{signorAddress}"] = true;
                    // playerClone.device?.xummToken = xummToken;
                    transaction.Set(
                        newPlayerRef,
                        PlayerService.ParsePlayer(playerClone, null)
                    );
                    
                    // TODO: MERGE TRUE
                    transaction.Set(
                        newPlayerRefRef,
                        PlayerService.ParsePlayerRef(PlayerService.CloneRef(playerClone), null)
                    );
                    
                    wallet.Id = newWalletRef.Id;
                    wallet.Active = true;
                    wallet.CreatedTime = updatedTime;
                    transaction.Set(
                        newWalletRef,
                        WalletService.ParseWallet(wallet, null)
                    );
                    // return true;
                } catch (Exception e) {
                    Debug.Log(e);
                }
            });
        }).ContinueWith((transactionResultTask) =>
        {
            // if (transactionResultTask.Result)
            // {
            //     Debug.Log("Wallet Created");
            // }
            // else
            // {
            //     Debug.Log(errorPointer);
            // }
            // if let error = error {
            //     self.createDelegate?.createWalletFailure(error: error) //error
            //     GambitAnalytics.errorHappened()
            //     return
            // }
            // SwaggerClientAPI.customHeaders = ["Authorization": "Bearer \(AppState.sharedInstance.userIDToken!)"]
            // TestnetAPI.testnetMint(body: TestnetMintRequest(
            //     playerId: playerClone.playerID,
            //     isType: "faucet"
            // )) { data, error in }
            // GambitAnalytics.creatingWalletSuccess()
            // self.createDelegate?.createWalletSuccess()
            Debug.Log("Wallet Created");
            // Debug.Log(errorPointer);
        });
    }

    public static AppWallet GetWalletFromSnapshot(DocumentSnapshot snapshot, string walletId)
    {   
        Dictionary<string, object> snapData = snapshot.ToDictionary();
        string serialized = JsonConvert.SerializeObject(snapData);
        return JsonConvert.DeserializeObject<Wallet>(serialized);
    }
    public static Dictionary<string, object> ParseWallet(AppWallet wallet, DocumentReference oldWalletRef)
    {
        // wallet.oldWalletRef = oldWalletRef;
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(wallet.ToJson());
    }
}