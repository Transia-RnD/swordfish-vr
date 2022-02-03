using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject playerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        playerPrefab = PhotonNetwork.Instantiate(
            "Network Player",
            transform.position,
            transform.rotation
        );
        Debug.Log("Spawned Network Player");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(playerPrefab);
    }
}
