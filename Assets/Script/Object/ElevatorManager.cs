using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ElevatorManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject _elevator;
    private bool isUp;

    [ClientRpc]
    public void GenerateElevatorClientRpc()
    {
        _elevator = Instantiate(Resources.Load<GameObject>("Prefabs/Object/Elevator"));
        _elevator.transform.position = new Vector3(21, -1, -13);
    }
    public void StartElevator()
    {
        isUp = true;
        StartCoroutine(CoStartElevator());
    }
    private IEnumerator CoStartElevator()
    {
        int cnt = 0;
        while (true)
        {
            yield return null;
            if (isUp)
            {
                SetElevatorPositionClientRpc(Vector3.up * 0.05f);
                cnt++;
            }
            else
            {
                SetElevatorPositionClientRpc(Vector3.down * 0.05f);
                cnt++;
            }
            if (cnt == 200)
            {
                isUp = !isUp;
                cnt = 0;
            } 
        }
    }

    [ClientRpc]
    private void SetElevatorPositionClientRpc(Vector3 deltaPosition)
    {
        _elevator.transform.position += deltaPosition;
    }
}
