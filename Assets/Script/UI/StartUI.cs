using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField]
    private Button _startHostButton;
    [SerializeField]
    private Button _startClientButton;

    private void Start()
    {
        _startHostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false);
        }); 
        _startClientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false);
        });
    }


}
