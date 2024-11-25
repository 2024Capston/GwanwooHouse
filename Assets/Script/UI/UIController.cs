using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [field: SerializeField]
    public StartUI StartUI { get; set; }

    [field: SerializeField]
    public IngameUI IngameUI { get; set; }

    private void OnEnable()
    {
        ClientManager.Instance.UIController = this;
    }
}
