using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BombLoader : NetworkSingletonBehavior<BombLoader>, ILoader
{
    private readonly string MANAGER_PATH = "Prefabs/Manager/";

    private GameObject _gameManager;
    private GameObject _serverManager;
    private GameObject _clientManager;
    private GameObject _spawnManager;

    protected override void Init()
    {
        base.Init();

        _isDestroyOnLoad = true;

        Load();
    }

    public void Load()
    {
        _gameManager = Instantiate(Resources.Load<GameObject>(MANAGER_PATH + "GameManager"));    
        _serverManager = Instantiate(Resources.Load<GameObject>(MANAGER_PATH + "ServerManager"));
        _clientManager = Instantiate(Resources.Load<GameObject>(MANAGER_PATH + "ClientManager"));
        _spawnManager = Instantiate(Resources.Load<GameObject>(MANAGER_PATH + "SpawnManager"));
    }

    public void Destroy()
    {
        Destroy(_gameManager);
        _gameManager = null;    
        Destroy(_serverManager);
        _serverManager = null;
        Destroy(_clientManager);
        _clientManager = null;
        Destroy(_spawnManager);
        _spawnManager = null;
    }
}
