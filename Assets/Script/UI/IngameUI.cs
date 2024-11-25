using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _colorData;

    [SerializeField]
    private TMP_Text _remainTime;

    public void SetColorData(bool isServer)
    {
        if (isServer)
        {
            _colorData.text = "Your Color: <color=\"blue\">Blue</color>";
        }
        else
        {
            _colorData.text = "Your Color: <color=\"red\">Red</color>";
        }
    }

    public void SetRemainTime(int time)
    {
        _remainTime.text = $"Time Remain: {time}s";
    }
}
