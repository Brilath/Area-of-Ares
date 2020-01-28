using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace AreaOfAres.Network
{
    public class PhotonRoom : MonoBehaviourPunCallbacks
    {
        public PhotonRoom instance;

        [SerializeField] private PhotonView _photonView;


        public static event Action<RoomPlayer> OnRoomConnected = delegate { };
        public static event Action<RoomPlayer> OnRoomDisconnected = delegate { };
        public static event Action OnNewJoinRoom = delegate { };

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

            _photonView = GetComponent<PhotonView>();
        }
        // Called when a Photon Player got connected. We need to then load a bigger scene.
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
            //_photonView.RPC("UpdateRoomPlayers", RpcTarget.AllBuffered);            

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

    }
}