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
        #region Class Variables
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private bool _isConnectedToPhoton;

        [Header("Join Game")]
        [SerializeField] private TextMeshProUGUI _ignInput;
        [SerializeField] private TextMeshProUGUI _roomInput;
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private GameObject _playButton;
        [SerializeField] private MainMenuController _menuController;

        [Header("Room")]
        [SerializeField] private string _gameMode;
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private TextMeshProUGUI _playerCountText;
        [SerializeField] private Transform _playersList;
        [SerializeField] private float _refreshRate;
        [SerializeField] private GameObject _startButton;
        private Dictionary<int, GameObject> _playerList;
        private RoomOptions _ro = new RoomOptions();


        [Header("Room Panels")]
        [SerializeField] private GameObject _roomPlayerContainer;
        [SerializeField] private GameObject _roomPlayerPanel;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            SetupPhotonConnection();

            _menuController = GetComponent<MainMenuController>();
            _ro.MaxPlayers = 4;
            _photonView = GetComponent<PhotonView>();
            _refreshRate = 10f;
            _gameMode = NetworkCustomSettings.SURVIVAL_MODE;
            _playerList = new Dictionary<int, GameObject>();
            _isConnectedToPhoton = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        private void Update()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        #endregion

        #region UI Methods
        public void OnCreateRoomButtonClicked()
        {
            if (IsValidPlayerName())
            {
                PhotonNetwork.NickName = _ignInput.text;
                _errorText.text = "";
                CreateCustomRandomRoom();
            }
            else
            {
                _errorText.text = "Enter Valid IGN";
            }
        }
        public void OnJoinRoomButtonClicked()
        {
            if (IsValidPlayerName())
            {
                PhotonNetwork.NickName = _ignInput.text;
                _errorText.text = "";

                if (IsValidRoomName())
                {
                    Debug.Log($"You want to join room {_roomInput.text}");
                    PhotonNetwork.JoinRoom(_roomInput.text);
                }
                else
                {
                    ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
                    {{NetworkCustomSettings.GAME_MODE, _gameMode}};
                    PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
                }
            }
            else
            {
                _errorText.text = "Enter Valid IGN";
            }
        }
        #endregion
        #region Photon Call Backs
        // Connected to the internet
        public override void OnConnected()
        {
            Debug.Log("Player connected to internet");
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log($"Player has is connected to Photon master server");
            _playButton.SetActive(true);
            //PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        public override void OnJoinedLobby()
        {
            Debug.Log($"Player has is connected to Photon lobby");
        }
        public override void OnCreatedRoom()
        {
            Debug.Log($"{PhotonNetwork.CurrentRoom.Name} is created.");
        }
        // Photon Call back when current player joins a room
        public override void OnJoinedRoom()
        {
            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} connected to room {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"Currently {PhotonNetwork.CurrentRoom.PlayerCount} connected");
            int playerCounter = 1;

            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(NetworkCustomSettings.GAME_MODE))
            {
                _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
                _playerCountText.text = $"Player Count {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";

                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    GameObject playerListGO = Instantiate(_roomPlayerPanel);
                    playerListGO.transform.SetParent(_roomPlayerContainer.transform);
                    playerListGO.transform.localScale = Vector3.one;
                    playerListGO.GetComponent<PlayerRoomPanel>().Initialize(player.ActorNumber, player.NickName);

                    _playerList.Add(player.ActorNumber, playerListGO);

                    object playerSelection;
                    if (player.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_SELECTION_NUMBER, out playerSelection))
                    {
                        int selection = (int)playerSelection;
                        playerListGO.GetComponent<PlayerSelection>().Initialize(player.ActorNumber, selection);
                    }
                    else
                    {
                        playerListGO.GetComponent<PlayerSelection>().Initialize(player.ActorNumber, 0);
                    }

                    ExitGames.Client.Photon.Hashtable playerNumberProperty = new ExitGames.Client.Photon.Hashtable()
                    {{NetworkCustomSettings.PLAYER_NUMBER, playerCounter}};
                    player.SetCustomProperties(playerNumberProperty);
                    playerCounter++;
                }
            }

            _startButton.SetActive(false);
            _menuController.ShowRoomCanvas();
        }
        // Called when a Photon Player gets connected
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"{newPlayer.NickName} connected to room {PhotonNetwork.CurrentRoom.Name}");

            _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            _playerCountText.text = $"Player Count {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";

            GameObject playerListGO = Instantiate(_roomPlayerPanel);
            playerListGO.transform.SetParent(_roomPlayerContainer.transform);
            playerListGO.transform.localScale = Vector3.one;
            playerListGO.GetComponent<PlayerRoomPanel>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

            _playerList.Add(newPlayer.ActorNumber, playerListGO);

            _startButton.SetActive(CheckPlayersReady());
        }
        // Call back if another player leaves the room
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"{otherPlayer.NickName} has left the room");
            _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            _playerCountText.text = $"Player Count {PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";

            if (_playerList.ContainsKey(otherPlayer.ActorNumber))
            {
                Destroy(_playerList[otherPlayer.ActorNumber].gameObject);
                _playerList.Remove(otherPlayer.ActorNumber);
            }
            //************************************************************\\
            //    Loop through each player and reassign player number     \\
            //************************************************************\\
            int playerCounter = 1;

            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(NetworkCustomSettings.GAME_MODE))
            {
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    ExitGames.Client.Photon.Hashtable playerNumberProperty = new ExitGames.Client.Photon.Hashtable()
                    {{NetworkCustomSettings.PLAYER_NUMBER, playerCounter}};
                    player.SetCustomProperties(playerNumberProperty);

                    ExitGames.Client.Photon.Hashtable actorNumberProperty = new ExitGames.Client.Photon.Hashtable()
                    {{NetworkCustomSettings.ACTOR_NUMBER, player.ActorNumber}};
                    player.SetCustomProperties(actorNumberProperty);

                    playerCounter++;
                }
                _startButton.SetActive(CheckPlayersReady());
            }
        }
        // Call back if the current player leaves the room
        public override void OnLeftRoom()
        {
            Debug.Log("Player has left the room");

            foreach (GameObject go in _playerList.Values)
            {
                Destroy(go);
            }
            _playerList.Clear();
            _roomNameText.text = string.Empty;

            object isPlayerLockedIn;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_LOCKED_IN, out isPlayerLockedIn))
            {
                ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable()
                    {
                        {NetworkCustomSettings.PLAYER_LOCKED_IN, false}
                    };
                PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
            }
            object playerNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_NUMBER, out playerNumber))
            {
                ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable()
                    {
                        {NetworkCustomSettings.PLAYER_NUMBER, 0}
                    };
                PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Player was disconnected");
            Debug.Log($"Reason for disconnect {cause.ToString()}");
            _menuController.HideAllCanvases();
        }
        // Call back if joining a room fails
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"OnJoinRandomFailed {message}");
            CreateCustomRandomRoom();
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"OnJoinRoomFailed {message}");
            CreateCustomRandomRoom();
        }
        // Call back if the master client leaves the room
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                _startButton.SetActive(CheckPlayersReady());
            }
        }
        // If a player custom properties change this is called
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            Debug.Log($"{targetPlayer.NickName} has updated their player properties");

            GameObject playerListGO;
            if (_playerList.TryGetValue(targetPlayer.ActorNumber, out playerListGO))
            {
                object isPlayerLockedIn;
                if (changedProps.TryGetValue(NetworkCustomSettings.PLAYER_LOCKED_IN, out isPlayerLockedIn))
                {
                    if ((bool)isPlayerLockedIn)
                    {
                        playerListGO.GetComponent<PlayerRoomPanel>().SetPlayerLockedIn();
                    }
                }
                object playerSelection;
                if (changedProps.TryGetValue(NetworkCustomSettings.PLAYER_SELECTION_NUMBER, out playerSelection))
                {
                    int selection = (int)playerSelection;
                    Debug.Log($"{targetPlayer.NickName} changed to character {selection}");
                    playerListGO.GetComponent<PlayerSelection>().UpdatePlayerSelection(selection);
                }
            }
            _startButton.SetActive(CheckPlayersReady());
        }
        #endregion

        #region Public Methods
        public void DisconnectRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            int[] levels = NetworkCustomSettings.CurrentLevels();
            int randomIndex = Random.Range(0, levels.Length);
            PhotonNetwork.LoadLevel(levels[randomIndex]);
        }
        #endregion

        #region Private Methods
        private static void SetupPhotonConnection()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.IsMessageQueueRunning = true;

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
        private void CreateCustomRandomRoom()
        {
            if (string.IsNullOrEmpty(_gameMode)) return;

            string roomName = _roomInput.text;
            if (!IsValidRoomName())
            {
                roomName = $"Room{Random.Range(1000, 9999)}";
            }

            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 4;

            string[] roomProperties = { NetworkCustomSettings.GAME_MODE };

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable()
            { {NetworkCustomSettings.GAME_MODE, _gameMode} };

            ro.CustomRoomPropertiesForLobby = roomProperties;
            ro.CustomRoomProperties = customRoomProperties;
            PhotonNetwork.CreateRoom(roomName, ro);
        }
        private bool CheckPlayersReady()
        {
            // If client isn't the master client return false
            if (!PhotonNetwork.IsMasterClient)
                return false;

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                object isPlayerLockedIn;
                if (player.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_LOCKED_IN, out isPlayerLockedIn))
                {
                    if (!(bool)isPlayerLockedIn)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsValidPlayerName()
        {
            bool isValid = false;
            Debug.Log("CheckingIGN");

            if (_ignInput.text.Length > NetworkCustomSettings.PLAYER_NAME_MIN &&
                _ignInput.text.Length - 1 <= NetworkCustomSettings.PLAYER_NAME_MAX)
            {
                isValid = true;
            }

            return isValid;
        }
        private bool IsValidRoomName()
        {
            bool isValid = false;

            if (_roomInput.text.Length > NetworkCustomSettings.ROOM_LENGTH_MIN &&
                _roomInput.text.Length <= NetworkCustomSettings.ROOM_LENGTH_MAX)
            {
                isValid = true;
            }

            return isValid;
        }
        #endregion
    }
}