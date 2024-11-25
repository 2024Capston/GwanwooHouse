using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientManager : NetworkSingletonBehavior<ClientManager>
{
    [field: SerializeField]
    public PlayerController MyPlayer { get; private set; }

    [field: SerializeField]
    public PlayerController PartnerPlayer { get; private set; }

    public UIController UIController { get; set; }

    /// <summary>
    /// Player를 설정한다.
    /// </summary>
    public void SetPlayer(PlayerController player, bool isMine)
    {
        if (isMine)
        {
            MyPlayer = player;
        }
        else
        {
            PartnerPlayer = player; 
        }
    }

    [ClientRpc] 
    public void SetPlayerTransformClientRpc(Vector3 position, Quaternion rotation, bool isServer)
    {
        if (IsServer == isServer)
        {
            MyPlayer.transform.position = position;
            MyPlayer.transform.rotation = rotation;
            MyPlayer.InteractableInHand = MyPlayer.InteractableOnPointer = null;
        }
        else
        {
            PartnerPlayer.transform.position = position;
            PartnerPlayer.transform.rotation = rotation;    
        }
    }

    [ClientRpc]
    public void ClearPlayerHandClientRpc(int objectId)
    {
        RopeController ropeController = MyPlayer.InteractableInHand as RopeController;
        if (ropeController != null && ropeController.ObjectIndex == objectId)
        {
            MyPlayer.InteractableInHand = null;
            MyPlayer.InteractableOnPointer = null;
        }
    }

    [ClientRpc]
    public void SetPlayerColorUIClientRpc()
    {
        UIController.IngameUI.gameObject.SetActive(true);
        UIController.IngameUI.SetColorData(IsServer);
    }

    [ClientRpc]
    public void SetPlayerRemainTimeClientRpc(int time)
    {
        UIController.IngameUI.SetRemainTime(time);
    }
}
