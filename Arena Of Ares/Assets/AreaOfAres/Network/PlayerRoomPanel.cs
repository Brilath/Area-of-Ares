using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerRoomPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    [SerializeField] private GameObject _lockInGameObject;
    [SerializeField] private Button _lockInButton;
    [SerializeField] private TextMeshProUGUI _lockInButtonText;
    [SerializeField] private Image _lockedInImage;

    [Header("Player")]
    [SerializeField] private bool isPlayerLockedIn;

    public void Initialize(int playerID, string playerName)
    {
        _playerName.text = playerName;

        // Check if player is local player
        if (PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            object isPlayerLockedIn;
            if (PhotonNetwork.CurrentRoom.GetPlayer(playerID).CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_LOCKED_IN, out isPlayerLockedIn))
            {
                if ((bool)isPlayerLockedIn)
                {
                    SetPlayerLockedIn();
                }
                else
                {
                    SetPlayerNotLockedIn();
                }
            }
            else
            {
                SetPlayerNotLockedIn();
            }
        }
        else
        {
            ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable()
            {
                {NetworkCustomSettings.PLAYER_LOCKED_IN, isPlayerLockedIn}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

            _lockInButton.onClick.AddListener(() =>
                {
                    SetPlayerLockedIn();
                    isPlayerLockedIn = true;
                    ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable()
                    {
                        {NetworkCustomSettings.PLAYER_LOCKED_IN, isPlayerLockedIn}
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
                }
            );
        }
    }

    public void SetPlayerLockedIn()
    {
        _lockInGameObject.SetActive(false);
        _leftArrow.gameObject.SetActive(false);
        _rightArrow.gameObject.SetActive(false);
        _lockedInImage.gameObject.SetActive(true);
    }

    public void SetPlayerNotLockedIn()
    {
        _lockInGameObject.SetActive(false);
        _leftArrow.gameObject.SetActive(false);
        _rightArrow.gameObject.SetActive(false);
        _lockedInImage.gameObject.SetActive(false);
    }
}
