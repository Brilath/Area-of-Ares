using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class AoAGameManager : MonoBehaviourPun
{
    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private Transform[] _startingPositions;
    [SerializeField] private TextMeshProUGUI _gameClockText;
    [SerializeField] private GameObject _gameEndScreen;

    [SerializeField] private float _gameTimeLimit;
    [SerializeField] private float _gameTimeLeft;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            int playerPosition = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 startingPosition = _startingPositions[playerPosition - 1].position;
            _gameTimeLeft = _gameTimeLimit;

            object playerSelection;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_SELECTION_NUMBER, out playerSelection))
            {
                GameObject playerGO = PhotonNetwork.Instantiate(_playerPrefabs[(int)playerSelection].name, startingPosition, Quaternion.identity);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void Update()
    {
        _gameTimeLeft -= Time.deltaTime;
        _gameTimeLeft = Mathf.Clamp(_gameTimeLeft, 0, _gameTimeLimit);
        UpdateClock();

        if (_gameTimeLeft <= 0)
        {
            StartCoroutine("EndGame");
        }
    }

    private void UpdateClock()
    {
        TimeSpan ts = TimeSpan.FromSeconds(_gameTimeLeft);
        _gameClockText.text = string.Format($"{ts.Minutes}:{ts.Seconds.ToString("D2")}");
    }

    private IEnumerator EndGame()
    {
        _gameEndScreen.SetActive(true);
        yield return new WaitForSeconds(10);
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}