using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private Image _selectedCharacterImage;
    [SerializeField] private Sprite[] _selectableCharacters;
    private int currentSelection;

    public void Initialize(int playerID, int selection)
    {
        Debug.Log($"Player Section Initialize for player: {playerID}");
        // Check if player is local player
        if (PhotonNetwork.LocalPlayer.ActorNumber == playerID)
        {
            Debug.Log($"Player Section Initialize for current player");
            ActivateSelection(selection);
        }
        else
        {
            object playerSelection;
            if (PhotonNetwork.CurrentRoom.GetPlayer(playerID).CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_SELECTION_NUMBER, out playerSelection))
            {
                int sel = (int)playerSelection;
                UpdatePlayerSelection(sel);
            }
        }
    }

    private void ActivateSelection(int selection)
    {
        UpdatePlayerSelection(selection);
        // Set Player Selection Custom Property
        ExitGames.Client.Photon.Hashtable playerSelectionProperty = new ExitGames.Client.Photon.Hashtable(){
            {
                NetworkCustomSettings.PLAYER_SELECTION_NUMBER, currentSelection}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperty);
    }

    public void UpdatePlayerSelection(int selection)
    {
        _selectedCharacterImage.sprite = _selectableCharacters[selection];
    }

    public void NextSelection()
    {
        currentSelection++;
        if (currentSelection >= _selectableCharacters.Length)
        {
            currentSelection = 0;
        }
        ActivateSelection(currentSelection);
    }
    public void PreviousSelection()
    {
        currentSelection--;
        if (currentSelection < 0)
        {
            currentSelection = _selectableCharacters.Length - 1;
        }
        ActivateSelection(currentSelection);
    }
}
