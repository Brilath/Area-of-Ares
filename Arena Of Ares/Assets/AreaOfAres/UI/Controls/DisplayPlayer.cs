﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;

public class DisplayPlayer : MonoBehaviourPun
{
    [SerializeField] private int playerID;
    [SerializeField] private int fruitCount;
    [SerializeField] private Image playerNumberImage;
    [SerializeField] private TextMeshProUGUI playerNumberText;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI fruitCountText;

    public void Intitalize(int id, string name, Color color)
    {
        playerID = id;
        playerNumberText.text = playerID.ToString();
        fruitCount = 0;
        UpdateFruitCount(id, fruitCount);
        playerName.text = name;
        playerNumberImage.color = color;

        FruitBasket.UpdateBasket += HandleUpdateBasket;
    }

    private void HandleUpdateBasket(int id, int amount)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateFruitCount", RpcTarget.AllBuffered, id, amount);
        }
    }

    [PunRPC]
    public void UpdateFruitCount(int id, int amount)
    {
        Debug.Log($"Updating player {id} UI by {amount}");
        if (playerID == id)
        {
            fruitCount = amount;
            fruitCount = Mathf.Clamp(fruitCount, 0, 99);
            fruitCountText.text = fruitCount.ToString("D2");
        }
    }

    private void OnDestroy()
    {
        FruitBasket.UpdateBasket -= HandleUpdateBasket;
    }
}
