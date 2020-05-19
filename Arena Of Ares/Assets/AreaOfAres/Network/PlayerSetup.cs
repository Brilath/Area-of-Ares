using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image playerNumberImage;
    [SerializeField] private TextMeshProUGUI playerNumberText;
    [SerializeField] private Color[] _playerColors;
    [SerializeField] private int playerNumber;
    [SerializeField] private Color playerColor;


    // Start is called before the first frame update
    private void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<AnimationController>().enabled = true;
            transform.GetComponent<PlayerSoundController>().enabled = true;
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<AnimationController>().enabled = false;
            transform.GetComponent<PlayerSoundController>().enabled = false;
        }

        playerNumber = photonView.OwnerActorNr;
        playerColor = _playerColors[playerNumber - 1];
        photonView.RPC("SetupPlayerIcon", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SetupPlayerIcon()
    {
        playerNumberText.text = playerNumber.ToString();
        playerNumberImage.color = playerColor;
    }
}
