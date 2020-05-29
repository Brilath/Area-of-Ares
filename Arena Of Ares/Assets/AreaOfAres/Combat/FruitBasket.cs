﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FruitBasket : MonoBehaviourPun
{
    public bool Modifiable { get { return _modifiable; } private set { _modifiable = value; } }
    [SerializeField] private bool _modifiable;
    [SerializeField] private int _maxFruit;
    [SerializeField] private int _currentFruit;
    [SerializeField] private float _invincibilityLength;
    [SerializeField] private float _invincibilityCounter;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private PlayerSoundController _playerSoundController;

    public int FruitCount { get { return _currentFruit; } set { _currentFruit = value; } }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        _playerSoundController = GetComponent<PlayerSoundController>();
    }

    private void Start()
    {
        FruitCount = 0;
        Modifiable = true;
        _invincibilityCounter = 0;
    }

    private void Update()
    {
        if (_invincibilityCounter > 0)
        {
            photonView.RPC("SetRenderAlpha", RpcTarget.AllBuffered, 0.5f);
            _invincibilityCounter -= Time.deltaTime;

            if (_invincibilityCounter <= 0)
            {
                photonView.RPC("SetRenderAlpha", RpcTarget.AllBuffered, 1.0f);
                Modifiable = true;
            }
        }
    }

    public void Modify(int amount)
    {
        if (Modifiable)
        {
            if (amount > 0)
            {
                _playerSoundController.PlayHealSound();
            }
            else if (amount < 0)
            {
                SetModifiable(false);
                _playerSoundController.PlayDamageSound();
                if (FruitCount > 0)
                {
                    int playerId = gameObject.GetComponent<PlayerSetup>().PlayerNumber;
                    Fruit.DropFruit(playerId, amount);
                }
            }
            FruitCount += amount;
            FruitCount = Mathf.Clamp(FruitCount, 0, _maxFruit);
        }
    }

    public int GetFruit()
    {
        return FruitCount;
    }
    [PunRPC]
    private void SetRenderAlpha(float alpha)
    {
        Color invincibilityColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        spriteRenderer.color = invincibilityColor;
    }
    private void SetModifiable(bool status)
    {
        Modifiable = status;
        if (!Modifiable)
        {
            _invincibilityCounter = _invincibilityLength;
        }
    }
}
