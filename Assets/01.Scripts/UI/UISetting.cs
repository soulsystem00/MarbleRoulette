using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    [SerializeField] private TMP_InputField inputPlayerCount;
    [SerializeField] private Button buttonShuffle;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private TMP_Dropdown dropdownMapSelect;
    [SerializeField] private TMP_Dropdown dropdownWinner;
    public override void Init()
    {
        inputPlayerCount.onValueChanged.AddListener(OnPlayerCountChanged);
        buttonShuffle.onClick.AddListener(OnShuffleButtonClick);
        buttonPlay.onClick.AddListener(OnPlayButtonClick);
        dropdownMapSelect.onValueChanged.AddListener(OnMapSelectChanged);
        dropdownWinner.onValueChanged.AddListener(OnWinnerSelectChanged);
    }

    public override void Open()
    {
        base.Open();

        inputPlayerCount.text = string.Empty;
        dropdownMapSelect.value = 0;
        dropdownWinner.value = 0;

        OnPlayerCountChanged(string.Empty);
        OnMapSelectChanged(0);
        OnWinnerSelectChanged(0);
    }

    public override void Close()
    {
        base.Close();
    }

    private void OnPlayerCountChanged(string value)
    {
        string data = value.Replace("\n", "").Replace("\r", "");
        GameManager.Instance.SpawnPlayers(data);
    }

    private void OnPlayButtonClick()
    {
        GameManager.Instance.Ready();
    }

    private void OnShuffleButtonClick()
    {
        GameManager.Instance.ShufflePlayers();
    }

    private void OnMapSelectChanged(int value)
    {
        GameManager.Instance.MapSetting((MapType)value);
    }

    private void OnWinnerSelectChanged(int value)
    {
        GameManager.Instance.SetWinType((WinType)value);
    }
}
