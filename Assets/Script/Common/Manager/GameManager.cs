using System.Collections;
using UnityEngine;
using Unity.Netcode;

public enum ColorType { Gray, Blue, Red, Purple }

public class GameManager : NetworkSingletonBehavior<GameManager>
{
    private int[] _answer;
    private int _currentPointer;
    private bool[] _isSoloInteracts;
    private ColorType[] _colors;
    private Coroutine[] _ropeTimers;
    private Coroutine _gameTimer;
    
    private ElevatorManager _elevatorManager;
    /// <summary>
    /// Game�� ������ �� ȣ��
    /// </summary>
    public void StartGame()
    {
        SetPlayerTransform();
        SpawnManager.Instance.InitializeClientRpc();
        GenerateAnswer();
        GenerateRope();
        GeneratenNext();
        _gameTimer = StartCoroutine(GameTimer());

        //elevator �׽�Ʈ
        _elevatorManager = gameObject.GetComponent<ElevatorManager>();
        _elevatorManager.GenerateElevatorClientRpc();
        _elevatorManager.StartElevator();
    }

    public void EndGame()
    {
        if (_gameTimer != null)
        {
            StopCoroutine(_gameTimer);
            _gameTimer = null;
        }

        SpawnManager.Instance.DestroyNextClientRpc();
        SpawnManager.Instance.DestroyAllRope();
        StartGame();
    }

    /// <summary>
    /// Rope�� Interact ��Ȳ�� ��ȯ�Ѵ�.
    /// </summary>
    /// <returns>true: ȥ�� ��ȣ�ۿ�, false: ��ȣ�ۿ��� ���ų� ���ÿ� ��ȣ�ۿ�</returns>
    [ServerRpc(RequireOwnership = false)]
    public void OnPlayerInteractRopeServerRpc(int objectId)
    {
        _isSoloInteracts[objectId] = !_isSoloInteracts[objectId];

        if (_isSoloInteracts[objectId])
        {
            _ropeTimers[objectId] = StartCoroutine(RopeTimer(objectId));
        }
        else
        {
            StopCoroutine(_ropeTimers[objectId]);
            _ropeTimers[objectId] = null;
        }
    }

    /// <summary>
    /// Rope�� �ı��� �� ȣ���Ѵ�.
    /// </summary>
    /// <param name="objectId"></param>
    [ServerRpc(RequireOwnership = false)]
    public void OnDestoryRopeServerRpc(int objectId)
    {
        if (_answer[_currentPointer] == objectId)
        {
            _currentPointer++;

            // ������ �� ó��
            if (_currentPointer == 7)
            {
                EndGame();
            }
            else
            {
                SpawnManager.Instance.SetNextClientRpc(_colors[_answer[_currentPointer]]);
            }
        }
        else
        {
            EndGame();
        }
    }

    /// <summary>
    /// Player�� �ʱ� ��ġ�� ����
    /// </summary>
    private void SetPlayerTransform()
    {
        ClientManager.Instance.SetPlayerTransformClientRpc(SpawnManager.Instance.GetPlayerSpawnPosition(IsServer), SpawnManager.Instance.GetPlayerSpawnRotation(IsServer), IsServer);
        ClientManager.Instance.SetPlayerTransformClientRpc(SpawnManager.Instance.GetPlayerSpawnPosition(!IsServer), SpawnManager.Instance.GetPlayerSpawnRotation(!IsServer), !IsServer);

    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    private void GenerateAnswer()
    {
        _answer = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
        _currentPointer = 0;    
        for (int i = 0; i < 98; i++)
        {
            int j = Random.Range(0, 7);
            int temp = _answer[i % 7];
            _answer[i % 7] = _answer[j];
            _answer[j] = temp;
        }
    }

    /// <summary>
    /// Rope�� ���� ���ϰ� Object�� �����Ѵ�.
    /// </summary>
    private void GenerateRope()
    {
        _colors = new ColorType[7];
        _isSoloInteracts = new bool[7];
        _ropeTimers = new Coroutine[7];

        for (int i = 0; i < 7; i++)
        {
            _colors[i] = (ColorType)Random.Range(0, 4);
            SpawnManager.Instance.CreateRopeClientRpc(_colors[i], i);
        }
    }

    private void GeneratenNext()
    {
        SpawnManager.Instance.CreateNextClientRpc();
        SpawnManager.Instance.SetNextClientRpc(_colors[_answer[_currentPointer]]);
    }

    /// <summary>
    /// 5�� �� Rope�� �ı��Ѵ�.
    /// </summary>

    IEnumerator RopeTimer(int objectId)
    {
        yield return new WaitForSeconds(5f);

        ClientManager.Instance.ClearPlayerHandClientRpc(objectId);
        SpawnManager.Instance.DestroyRopeClientRpc(objectId);
        OnDestoryRopeServerRpc(objectId);
    }

    IEnumerator GameTimer()
    {
        for (int i = 60; i > 0; i--)
        {
            yield return new WaitForSeconds(1f);
            ClientManager.Instance.SetPlayerRemainTimeClientRpc(i);
        }
        EndGame();
    }
}
