using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : NetworkSingletonBehavior<SpawnManager>
{
    [SerializeField]
    private Transform _redSpawnPoint;

    [SerializeField]
    private Transform _blueSpawnPoint;

    [SerializeField]
    private Transform[] _ropeSpawnPoint = new Transform[7];

    [SerializeField]
    private Transform _nextSpawnPoint;

    [SerializeField]
    private Material[] _materials = new Material[3];

    private GameObject _rope;
    private GameObject _next;
    private Renderer _nextMaterial;

    private List<GameObject> _ropes;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _rope = Resources.Load<GameObject>("Prefabs/Object/Rope");
        _next = Resources.Load<GameObject>("Prefabs/Object/Next");
    }

    [ClientRpc]
    public void InitializeClientRpc()
    {
        _ropes = new List<GameObject>();
    }

    /// <summary>
    /// RedPlayer, BluePlayer�� Spawn ��ġ�� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="isHost"></param>
    /// <returns></returns>
    public Vector3 GetPlayerSpawnPosition(bool isHost)
    {
        if (isHost)
        {
            return _redSpawnPoint.position;
        }
        return _blueSpawnPoint.position;
    }

    public Quaternion GetPlayerSpawnRotation(bool isHost)
    {
        if (isHost)
        {
            return _redSpawnPoint.rotation;
        }
        return _blueSpawnPoint.rotation;
    }

    [ClientRpc]
    public void DestroyRopeClientRpc(int index)
    {
        GameObject rope = _ropes[index];
        if (rope != null)
        {
            Destroy(rope);
            rope = null;
        }
    }

    public void DestroyAllRope()
    {
        for (int i = 0; i < 7; i++)
        {
            DestroyRopeClientRpc(i);
        }
    }

    /// <summary>
    /// rope Object�� �����ϰ� Ŭ���̾�Ʈ���� ���̴� ���� ĥ�ϰ� �Ѵ�.
    /// </summary>
    [ClientRpc]
    public void CreateRopeClientRpc(ColorType color, int index)
    {
        Debug.Log(color);
        int mask = IsServer ? 1 : 2;
        int colorIndex = (int)color & mask;
        GameObject rope = Instantiate(_rope, _ropeSpawnPoint[index].transform.position, _ropeSpawnPoint[index].transform.rotation);
        rope.GetComponent<Renderer>().material = _materials[colorIndex];
        rope.GetComponent<RopeController>().ObjectIndex = index;
        _ropes.Add(rope);
    }

    [ClientRpc]
    public void CreateNextClientRpc()
    {
        GameObject next = Instantiate(_next, _nextSpawnPoint.transform.position, _nextSpawnPoint.transform.rotation);
        _nextMaterial = next.GetComponent<Renderer>();  
    }

    [ClientRpc]
    public void SetNextClientRpc(ColorType color)
    {
        int mask = IsServer ? 1 : 2;
        int colorIndex = (int)color & mask;
        _nextMaterial.material = _materials[colorIndex];
    }

    [ClientRpc]
    public void DestroyNextClientRpc()
    {
        Destroy(_nextMaterial.gameObject);
        _nextMaterial = null;
    }
}
