using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RopeController : MonoBehaviour, IInteractable
{
    public Outline ObjectOutline { get; private set; }
    public int ObjectIndex { get; set; }

    private void Awake()
    {
        ObjectOutline = GetComponent<Outline>();
        ObjectOutline.enabled = false;
    }

    public bool StartInteraction(PlayerController player)
    {
        GameManager.Instance.OnPlayerInteractRopeServerRpc(ObjectIndex);
        return true;
    }

    public bool StopInteraction(PlayerController player)
    {
        GameManager.Instance.OnPlayerInteractRopeServerRpc(ObjectIndex);
        return true;
    }

    public bool IsInteractable(PlayerController player)
    {
        return true;
    }

    public void ShowHighlight()
    {
        ObjectOutline.enabled = true;
    }

    public void HideHighlight()
    {
        ObjectOutline.enabled = false;
    }

}
