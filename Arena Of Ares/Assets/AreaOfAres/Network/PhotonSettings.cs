using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace AreaOfAres.Network
{
    [CreateAssetMenu(menuName = "Game Settings/Photon", fileName = "Photon Settings")]
    public class PhotonSettings : ScriptableObject
    {
        [SerializeField] private byte _maxPlayers;
        [SerializeField] private RoomOptions _roomOptions;
        [SerializeField] private TypedLobby _typedLobby;
        [SerializeField] private int _minRoomLength;
        [SerializeField] private string _gameVersion;

        public byte MaxPlayers { get { return _maxPlayers; } private set { _maxPlayers = value; } }
        public RoomOptions Options { get { return _roomOptions; } private set { _roomOptions = value; } }
        public TypedLobby Lobby { get { return _typedLobby; } private set { _typedLobby = value; } }
        public string GameVersion { get { return _gameVersion; } private set { _gameVersion = value; } }
        public int MinimumRoomLength { get { return _minRoomLength; } private set { _minRoomLength = value; } }

        public PhotonSettings()
        {
            MaxPlayers = 4;
            Options = new RoomOptions();
            Options.MaxPlayers = MaxPlayers;
            Lobby = TypedLobby.Default;
            GameVersion = "1.0";
            MinimumRoomLength = 1;
        }
    }
}