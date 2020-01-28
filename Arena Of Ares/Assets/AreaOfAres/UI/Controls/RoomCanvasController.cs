﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AreaOfAres.Network;
using System;
using AreaOfAres.UI.Controls;

public class RoomCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPanels;
    [SerializeField] private PlayerPanel _playerPanel;
    [SerializeField] private Dictionary<RoomPlayer, PlayerPanel> _panels;

    private void Awake()
    {
        _panels = new Dictionary<RoomPlayer, PlayerPanel>();
    }
    private void OnEnable()
    {
        PhotonRoom.OnRoomConnected += HandleOnRoomConnected;
        PhotonRoom.OnRoomDisconnected += HandleOnRoomDisconnected;
    }

    private void OnDisable()
    {
        PhotonRoom.OnRoomConnected -= HandleOnRoomConnected;
        PhotonRoom.OnRoomDisconnected -= HandleOnRoomDisconnected;
    }

    private void HandleOnRoomConnected(RoomPlayer player)
    {
        var panel = Instantiate(_playerPanel, _playerPanels.transform);
        panel.Initialize(player);
        _panels.Add(player, panel);
    }
    private void HandleOnRoomDisconnected(RoomPlayer player)
    {
        for (int i = 0; i > _panels.Count; i++)
        {
            if (_panels.ContainsKey(player))
            {
                //_panels.Values

            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
