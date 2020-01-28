using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AreaOfAres.Network;
using System;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

namespace AreaOfAres.UI.Controls
{
    public class NetworkMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _playerPanel;
        [SerializeField] private GameObject _characterPanel;

        [Header("Room")]
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private Transform _playersList;
        [SerializeField] private Room _currentRoom;
        [SerializeField] private float _refreshRate;
        [SerializeField] private GameObject _startButton;

        private void Awake()
        {

        }
        private void OnEnable()
        {
            PhotonLobby.OnLobbyConnected += HandleLobbyConnected;
            PhotonLobby.OnLobbyDisconnected += HandleLobbyDisconnected;

            //PhotonRoom.OnNewJoinRoom
        }
        private void OnDisable()
        {
            PhotonLobby.OnLobbyConnected -= HandleLobbyConnected;
            PhotonLobby.OnLobbyDisconnected -= HandleLobbyDisconnected;
        }

        private void HandleLobbyDisconnected()
        {
            _playButton.SetActive(false);
        }

        private void HandleLobbyConnected()
        {
            _playButton.SetActive(true);
        }

        // [PunRPC]
        // public void UpdateRoomPlayers(List<Player> players)
        // {
        //     for (int i = 0; i < _playersList.transform.childCount; i++)
        //     {
        //         _playersList.GetChild(i).gameObject.SetActive(false);
        //     }
        //     Debug.Log($"Current Players");
        //     int index = 0;

        //     foreach (Player player in players)
        //     {
        //         Debug.Log($"Player: {player.NickName}");
        //         var playerGo = _playersList.GetChild(index).gameObject;
        //         playerGo.SetActive(true);
        //         playerGo.GetComponentInChildren<TextMeshProUGUI>().text = player.NickName;
        //         index++;
        //     }
        // }
    }
}