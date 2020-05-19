using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class AoAGameManager : MonoBehaviourPun
{
    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private Transform[] _startingPositions;


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            int playerPosition = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 startingPosition = _startingPositions[playerPosition - 1].position;

            object playerSelection;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_SELECTION_NUMBER, out playerSelection))
            {
                GameObject playerGO = PhotonNetwork.Instantiate(_playerPrefabs[(int)playerSelection].name, startingPosition, Quaternion.identity);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void LoadMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}