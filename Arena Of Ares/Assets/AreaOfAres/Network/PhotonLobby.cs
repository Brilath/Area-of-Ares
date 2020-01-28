using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace AreaOfAres.Network
{
    public class PhotonLobby : MonoBehaviourPunCallbacks
    {
        public static PhotonLobby instance;

        public static event Action OnLobbyConnected = delegate { };
        public static event Action OnLobbyDisconnected = delegate { };

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
            DontDestroyOnLoad(gameObject);

            PhotonNetwork.AutomaticallySyncScene = true;
            ConnectToLobby();
        }

        private void ConnectToLobby()
        {
            PhotonNetwork.ConnectUsingSettings();
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
            OnLobbyConnected?.Invoke();
        }

        public override void OnLeftLobby()
        {
            Debug.Log($"Client has disconnected from lobby");
            Debug.Log($"Reconnectiong to Master...");
            ConnectToLobby();
            OnLobbyDisconnected?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Reason for disconnect {cause.ToString()}");
            Debug.Log($"Reconnectiong to Master...");
            ConnectToLobby();
        }

    }
}