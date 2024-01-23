using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;

public class LobbyUI : LobbyManager
{
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private TextMeshProUGUI numberOfPlayers;
    [SerializeField] private Button CreateBtn;
    [SerializeField] private Button JoinBtn;

    public Button StartGameButton;
    public GameObject LobbyPanel;
    public GameObject InputPanel;
    public GameObject RoomPanel;

    public string NumberOfPlayers
    {
        get { return numberOfPlayers.text; }
        set { numberOfPlayers.text = value; }
    }
    public string RoomName => "Test";

    private void Awake()
    {
        CreateBtn.onClick.AddListener(() => StartGame(GameMode.Host));
        JoinBtn.onClick.AddListener(() => Debug.Log("Test")); // Change this later to make a new Input Room
    }

    public void CreatePlayers()
    {
        foreach(var player in _runner.ActivePlayers)
        {
            //// Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % _runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = _runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }

        RoomPanel.SetActive(false);
    }
}
