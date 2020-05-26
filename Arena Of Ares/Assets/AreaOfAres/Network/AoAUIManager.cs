using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AoAUIManager : MonoBehaviour
{
    [SerializeField] private DisplayPlayer[] _playerUIs;
    [SerializeField] private Color[] _playerColors;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                object playerNum;
                int playerID = 0;
                if (player.Value.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_NUMBER, out playerNum))
                {
                    playerID = (int)playerNum;
                }

                string playerName = player.Value.NickName;
                Color playerColor = _playerColors[playerID - 1];
                DisplayPlayer playerUI = _playerUIs[playerID - 1];

                playerUI.Intitalize(playerID, playerName, playerColor);
                playerUI.gameObject.SetActive(true);
            }
        }
    }
}
