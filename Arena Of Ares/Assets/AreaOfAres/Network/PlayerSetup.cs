using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image playerNumberImage;
    [SerializeField] private TextMeshProUGUI playerNumberText;
    [SerializeField] private Transform playerDashImages;
    [SerializeField] private Color[] _playerColors;
    [SerializeField] private int playerNumber;
    public int PlayerNumber { get { return playerNumber; } set { playerNumber = value; } }
    [SerializeField] private Color playerColor;


    // Start is called before the first frame update
    private void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<AnimationController>().enabled = true;
            transform.GetComponent<PlayerSoundController>().enabled = true;
            transform.GetComponent<PlayerUIController>().Intitalize(transform.GetComponent<MovementController>());
            playerColor = _playerColors[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            SetupDashIcons(playerDashImages, playerColor);
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<AnimationController>().enabled = false;
            transform.GetComponent<PlayerSoundController>().enabled = false;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == photonView.OwnerActorNr)
            {
                object playerNum;
                if (player.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_NUMBER, out playerNum))
                {
                    PlayerNumber = (int)playerNum;
                    playerColor = _playerColors[PlayerNumber - 1];
                    photonView.RPC("SetupPlayerIcon", RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    private void SetupPlayerIcon()
    {
        playerNumberText.text = PlayerNumber.ToString();
        playerNumberImage.color = playerColor;
    }

    private void SetupDashIcons(Transform transform, Color color)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<Image>().color = color;
        }
    }
}
