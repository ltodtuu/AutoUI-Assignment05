using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameStateManager : NetworkBehaviour
{
    [SerializeField]
    Transform cube;

    [SerializeField]
    GameObject _textField;

    [SerializeField]
    Canvas _canvas;

    public NetworkVariable<GameStateContainer> currentGameState = new NetworkVariable<GameStateContainer>(
        new GameStateContainer
        {
            gameState = GameState.NotStarted,
            cubesSpawnedForP1 = false,
            cubesSpawnedForP2 = false,
        });

    public struct GameStateContainer : INetworkSerializable
    {
        public GameState gameState;
        public bool cubesSpawnedForP1;
        public bool cubesSpawnedForP2;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref gameState);
            serializer.SerializeValue(ref cubesSpawnedForP1);
            serializer.SerializeValue(ref cubesSpawnedForP2);
        }
    }

    public enum GameState
    {
        NotStarted,
        NotReady,
        Started,
    }

    private List<ulong> playerIDs;

    // Update is called once per frame
    void Update()
    {
        switch (this.currentGameState.Value.gameState)
        {
            case GameState.NotStarted:
                _textField.GetComponent<TextMeshProUGUI>().text = "Press F to start the Game...";
                _canvas.gameObject.SetActive(false);
                break;
            case GameState.NotReady:
                _textField.GetComponent<TextMeshProUGUI>().text = "Wainting for blocks to spawn...";
                _canvas.gameObject.SetActive(false);
                break;
            case GameState.Started:
                _textField.GetComponent<TextMeshProUGUI>().text = "Build:";
                _canvas.gameObject.SetActive(true);
                break;
        }
        if (!IsServer) return;
        if (playerIDs is null || playerIDs.Count != 2)
        {
            this.playerIDs = characterScript.ActivePlayers.ToList();
        }
        if (Input.GetKey(KeyCode.F) && this.currentGameState.Value.gameState is GameState.NotStarted)
        {
            this.currentGameState.Value = new GameStateContainer()
            {
                gameState = GameState.NotReady,
                cubesSpawnedForP2 = this.currentGameState.Value.cubesSpawnedForP2,
                cubesSpawnedForP1 = this.currentGameState.Value.cubesSpawnedForP1,
            };
            _textField.GetComponent<TextMeshProUGUI>().text = "Waiting for Cubes to Spawn...";
        }
        if (this.currentGameState.Value.gameState is GameState.NotReady && playerIDs.Count == 2)
        {
            if (!this.currentGameState.Value.cubesSpawnedForP1)
            {
                for (int i = 0; i < 10; i++)
                {
                    var spawnedObject = Instantiate(cube);
                    spawnedObject.transform.position = new Vector3(-8 + i * 0.4f, 3.5f, -6f);

                    spawnedObject.GetComponentInChildren<ClientNetworkTransform>().NetworkObject.Spawn(true);
                    spawnedObject.GetComponentInChildren<ClientNetworkTransform>().NetworkObject.ChangeOwnership(playerIDs[0]);
                }
                this.currentGameState.Value = new GameStateContainer()
                {
                    gameState = this.currentGameState.Value.gameState,
                    cubesSpawnedForP2 = this.currentGameState.Value.cubesSpawnedForP2,
                    cubesSpawnedForP1 = true,
                };
            }
            else if (!this.currentGameState.Value.cubesSpawnedForP2)
            {
                for (int i = 0; i < 10; i++)
                {
                    var spawnedObject = Instantiate(cube);
                    spawnedObject.transform.position = new Vector3(-8 + i * 0.4f, 3.5f, -4f);
                    spawnedObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                    spawnedObject.GetComponentInChildren<ClientNetworkTransform>().NetworkObject.Spawn(true);
                    spawnedObject.GetComponentInChildren<ClientNetworkTransform>().NetworkObject.ChangeOwnership(playerIDs[1]);
                    this.currentGameState.Value = new GameStateContainer()
                    {
                        gameState = this.currentGameState.Value.gameState,
                        cubesSpawnedForP1 = this.currentGameState.Value.cubesSpawnedForP2,
                        cubesSpawnedForP2 = true,
                    };
                }
            }
            if (this.currentGameState.Value.cubesSpawnedForP1 && this.currentGameState.Value.cubesSpawnedForP2)
            {
                this.currentGameState.Value = new GameStateContainer()
                {
                    gameState = GameState.Started,
                    cubesSpawnedForP2 = this.currentGameState.Value.cubesSpawnedForP2,
                    cubesSpawnedForP1 = this.currentGameState.Value.cubesSpawnedForP1,
                };
                
            }
        }
    }
}
