using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using Unity.VisualScripting;

public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner _runner;

    public NetworkPrefabRef _playerPrefab;

    public int maxCount = 2;

    [SerializeField] private LobbyUI UI;

    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private bool _mouseButton0;

    private void Update()
    {
        _mouseButton0 = _mouseButton0 | Input.GetMouseButton(0);
    }

    public async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = UI.RoomName, // Change this to randomly generate
            PlayerCount = maxCount,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        Debug.Log($"Hosting a game");
    }


    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"Connected {runner.IsRunning}");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        if (_mouseButton0)
            data.buttons |= NetworkInputData.MOUSEBUTTON1;

        _mouseButton0 = false;

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
        if (runner.IsServer)
        {
            UI.LobbyPanel.SetActive(false);
            UI.InputPanel.SetActive(false);
            UI.RoomPanel.SetActive(true);
            UI.NumberOfPlayers = string.Format("{0}/{1}", runner.ActivePlayers.ToList().Count.ToString(), maxCount.ToString());
        }
        else
        {
            UI.LobbyPanel.SetActive(false);
            UI.InputPanel.SetActive(false);
            UI.RoomPanel.SetActive(true);
            UI.NumberOfPlayers = string.Format("{0}/{1}", runner.ActivePlayers.ToList().Count.ToString(), maxCount.ToString());
        }
        
        UI.StartGameButton.gameObject.SetActive(!runner.IsClient);
        UI.roomCode.gameObject.SetActive(!runner.IsClient);
        UI.EnterGameButton.gameObject.SetActive(runner.IsClient);
        // UI.waitingText.gameObject.SetActive(runner.IsClient); 
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            _spawnedCharacters.Remove(player);
        }
        UI.NumberOfPlayers = string.Format("{0}/{1}", runner.ActivePlayers.ToList().Count.ToString(), maxCount.ToString());
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
