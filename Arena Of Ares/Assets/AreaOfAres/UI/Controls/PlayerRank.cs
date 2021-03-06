﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRank : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerRankText;
    [SerializeField] TextMeshProUGUI _playerNameText;
    [SerializeField] Image _playerModel;
    [SerializeField] TextMeshProUGUI _playerScore;

    public void Initialize(int rank, string name, int score, Sprite image)
    {
        _playerRankText.text = rank.ToString();
        _playerNameText.text = name;
        _playerScore.text = score.ToString();
        _playerModel.sprite = image;
    }

}
