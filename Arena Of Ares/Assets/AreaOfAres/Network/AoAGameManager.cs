using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using Photon.Realtime;
using System.Linq;

public class AoAGameManager : MonoBehaviourPun
{
    [Header("Spawning Players")]
    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private Transform[] _startingPositions;
    [SerializeField] private Sprite[] _playerSprites;
    [Header("Player Ranking")]
    [SerializeField] private GameObject _playerRankingScreen;
    [SerializeField] private GameObject _playerRankings;
    [SerializeField] private GameObject _playerRankPrefab;
    [Header("General")]
    [SerializeField] private int nextLevel;
    [SerializeField] private FruitController _fruitController;
    [SerializeField] private TextMeshProUGUI _gameClockText;
    [SerializeField] private float _gameTimeLimit;
    [SerializeField] private float _gameTimeLeft;
    [SerializeField] private bool _gameEnding;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.IsMasterClient)
        {
            _fruitController.StartSpawning();
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            _playerRankingScreen.SetActive(false);

            _gameTimeLimit = NetworkCustomSettings.GAME_TIME;
            _gameTimeLeft = _gameTimeLimit;
            _gameEnding = false;

            int playerPosition = 0;
            object playerNum;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_NUMBER, out playerNum))
            {
                playerPosition = (int)playerNum - 1;
            }
            Vector3 startingPosition = _startingPositions[playerPosition].position;

            object playerSelection;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_SELECTION_NUMBER, out playerSelection))
            {
                PhotonNetwork.Instantiate(_playerPrefabs[(int)playerSelection].name, startingPosition, Quaternion.identity);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            GameController.Instance.LoadNextLevel(0);
        }
    }
    private void Update()
    {
        _gameTimeLeft -= Time.deltaTime;
        _gameTimeLeft = Mathf.Clamp(_gameTimeLeft, 0, _gameTimeLimit);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            photonView.RPC("UpdateClock", RpcTarget.AllBuffered);
        }

        if (_gameTimeLeft <= 0 && !_gameEnding && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _gameEnding = true;
            StartCoroutine("LoadNextLevel");
        }
    }

    [PunRPC]
    private void UpdateClock()
    {
        TimeSpan ts = TimeSpan.FromSeconds(_gameTimeLeft);
        _gameClockText.text = string.Format($"{ts.Minutes}:{ts.Seconds.ToString("D2")}");
    }

    private IEnumerator LoadNextLevel()
    {
        _fruitController.EndSpawning();
        photonView.RPC("RankPlayers", RpcTarget.AllBuffered);

        yield return new WaitForSeconds(5);
        if (PhotonNetwork.IsMasterClient)
        {
            GameController.Instance.LoadNextLevel(nextLevel);
        }
    }

    [PunRPC]
    private void RankPlayers()
    {
        List<AoAPlayer> aoaPlayers = new List<AoAPlayer>();
        aoaPlayers = GameController.Instance.UpdatePlayers();
        int rank = 1;

        _playerRankingScreen.SetActive(true);

        Debug.Log("Sorting Players by fruit collected");

        aoaPlayers = aoaPlayers.OrderByDescending(p => p.FruitCount).ToList();

        foreach (AoAPlayer player in aoaPlayers)
        {
            GameObject rankingGO = PhotonNetwork.Instantiate(_playerRankPrefab.name, transform.position, Quaternion.identity);
            rankingGO.transform.SetParent(_playerRankings.transform);
            rankingGO.transform.localScale = Vector3.one;
            PlayerRank pr = rankingGO.GetComponent<PlayerRank>();
            pr.Initialize(rank, player.Name, player.FruitCount, player.Model);
            rank++;
        }
    }
}