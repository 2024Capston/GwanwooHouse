using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerManager : NetworkSingletonBehavior<ServerManager>
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            GameManager.Instance.StartGame();
            ClientManager.Instance.SetPlayerColorUIClientRpc();
        }
    }

}
