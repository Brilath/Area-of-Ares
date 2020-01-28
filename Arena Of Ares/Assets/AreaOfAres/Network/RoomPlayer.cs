using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace AreaOfAres.Network
{
    public class RoomPlayer
    {
        public Player PhotonPlayer { get; set; }
        public bool Ready { get; set; }
    }
}