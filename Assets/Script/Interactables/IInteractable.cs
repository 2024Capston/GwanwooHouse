using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾�� ��ȣ�ۿ��� �� �ִ� ��ü�� ��Ÿ���� �������̽�
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// �÷��̾�� ��ȣ�ۿ��� �����Ѵ�.
    /// </summary>
    /// <param name="player">��� �÷��̾�</param>
    /// <returns></returns>
    public bool StartInteraction(PlayerController player);

    /// <summary>
    /// �÷��̾�� ��ȣ�ۿ��� �ߴ��Ѵ�.
    /// </summary>
    /// <param name="player">��� �÷��̾�</param>
    /// <returns></returns>
    public bool StopInteraction(PlayerController player);

    /// <summary>
    /// �÷��̾ ��ü�� ��ȣ�ۿ��� �� �ִ��� ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="player">��� �÷��̾�</param>
    /// <returns>��ȣ�ۿ� ���� ����</returns>
    public bool IsInteractable(PlayerController player);

}