using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using AreaOfAres.UI;

namespace AreaOfAres.Network
{
    public class ConnectController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView _photonView;

        [Header("Join Game")]
        [SerializeField] private TextMeshProUGUI _ignInput;
        [SerializeField] private TextMeshProUGUI _roomInput;
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private GameObject _playButton;
        [SerializeField] private float MIN_CHARS;
        [SerializeField] private MainMenuController menuController;

        [Header("Room")]
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private Transform _playersList;
        [SerializeField] private Room _currentRoom;
        [SerializeField] private float _refreshRate;
        [SerializeField] private GameObject _startButton;
        RoomOptions ro = new RoomOptions();

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();

            menuController = GetComponent<MainMenuController>();
            MIN_CHARS = 1;
            ro.MaxPlayers = 4;
            _photonView = GetComponent<PhotonView>();
            _refreshRate = 10f;
        }

        private void Update()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {

            }
        }

        // Called when a Photon Player got connected. We need to then load a bigger scene.
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
            _photonView.RPC("UpdateRoomPlayers", RpcTarget.AllBuffered);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        // Called when a Photon Player got disconnected. We need to load a smaller scene.
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects
            _photonView.RPC("UpdateRoomPlayers", RpcTarget.AllBuffered);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public override void OnConnectedToMaster()
        {
            if (!PhotonNetwork.InLobby)
            {
                Debug.Log("Connected to Master!!");
                PhotonNetwork.JoinLobby(TypedLobby.Default);
            }
            Debug.Log("Already connected to Master!!");
        }
        public override void OnJoinedLobby()
        {
            Debug.Log("Connected to Lobby");
            _playButton.SetActive(true);
        }
        public override void OnLeftLobby()
        {
            _playButton.SetActive(false);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log($"Connected to room {PhotonNetwork.CurrentRoom.Name}");
            _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            _currentRoom = PhotonNetwork.CurrentRoom;
            _photonView.RPC("UpdateRoomPlayers", RpcTarget.AllBuffered);
            if (PhotonNetwork.IsMasterClient)
            {
                _startButton.SetActive(true);
            }
            else
            {
                _startButton.SetActive(false);
            }
            menuController.ShowRoomCanvas();
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Reason for disconnect {cause.ToString()}");
        }

        public override void OnLeftRoom()
        {

        }

        public void DisconnectRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        [PunRPC]
        public void UpdateRoomPlayers()
        {
            for (int i = 0; i < _playersList.transform.childCount; i++)
            {
                _playersList.GetChild(i).gameObject.SetActive(false);
            }
            Debug.Log($"Current Players");
            int index = 0;

            foreach (KeyValuePair<int, Player> player in _currentRoom.Players)
            {
                Debug.Log($"Player: {player.Value.NickName}");
                var playerGo = _playersList.GetChild(index).gameObject;
                playerGo.SetActive(true);
                playerGo.GetComponentInChildren<TextMeshProUGUI>().text = player.Value.NickName;
                index++;
            }
        }

        public void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }

        public void CreateRoom()
        {
            if (_ignInput.text.Length > MIN_CHARS &&
                _roomInput.text.Length > MIN_CHARS)
            {
                PhotonNetwork.NickName = _ignInput.text;
                _errorText.text = "";

                PhotonNetwork.JoinOrCreateRoom(_roomInput.text, ro, TypedLobby.Default);
            }
            else
            {
                _errorText.text = "Enter IGN / Room Name";

            }
        }
        public void JoinRoom()
        {
            if (_ignInput.text.Length > MIN_CHARS)
            {
                PhotonNetwork.NickName = _ignInput.text;
                _errorText.text = "";

                if (_roomInput.text.Length > MIN_CHARS)
                {
                    PhotonNetwork.JoinOrCreateRoom(_roomInput.text, ro, TypedLobby.Default);
                }
                else
                {
                    if (PhotonNetwork.CountOfRooms > 0)
                    {
                        PhotonNetwork.JoinRandomRoom();
                    }
                    else
                    {
                        _errorText.text = "No rooms available please create one";
                    }
                }
            }
            else
            {
                _errorText.text = "Enter IGN";
            }
        }
    }
}