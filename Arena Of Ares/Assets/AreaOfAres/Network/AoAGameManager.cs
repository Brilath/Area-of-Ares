using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using Photon.Realtime;
using System.Linq;

public class AoAGameManager : MonoBehaviourPunCallbacks
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
    [SerializeField] private GameController _gameController;

    private void Awake()
    {
        _gameController = GetComponent<GameController>();
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
            _gameController.LoadNextLevel(0);
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

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Lets End this round...");
            ScoreKeeper scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();
            GameObject goScoreKeeper;
            if (scoreKeeper == null)
            {
                goScoreKeeper = PhotonNetwork.Instantiate("ScoreKeeper", transform.position, Quaternion.identity);
                scoreKeeper = goScoreKeeper.GetComponent<ScoreKeeper>();
            }
            Debug.Log($"Finding Score Keeper {scoreKeeper.gameObject.name}");

            List<AoAPlayer> aoaPlayers = new List<AoAPlayer>();
            aoaPlayers = _gameController.UpdatePlayers();
            foreach (AoAPlayer player in aoaPlayers)
            {
                scoreKeeper.UpdateRoundScore(player.Player.ActorNumber, player.FruitCount);
            }

            photonView.RPC("RankPlayersRPC", RpcTarget.AllBuffered, scoreKeeper.ScoreBoard);
            yield return new WaitForSeconds(NetworkCustomSettings.SCORE_SCREEN_TIME);
            // GameController.Instance.LoadNextLevel(nextLevel);
            _gameController.LoadNextLevel(nextLevel);
        }
    }

    [PunRPC]
    private void RankPlayersRPC(Dictionary<int, int> playerScores)
    {
        List<AoAPlayer> aoaPlayers = new List<AoAPlayer>();
        // aoaPlayers = GameController.Instance.UpdatePlayers();
        aoaPlayers = _gameController.UpdatePlayers();
        int rank = 1;

        _playerRankingScreen.SetActive(true);

        Debug.Log("Sorting Players by fruit collected");

        foreach (KeyValuePair<int, int> score in playerScores)
        {
            aoaPlayers.Find(p => p.Player.ActorNumber == score.Key)?.SetCount(score.Value);
        }

        aoaPlayers = aoaPlayers.OrderByDescending(p => p.FruitCount).ToList();

        foreach (AoAPlayer player in aoaPlayers)
        {
            GameObject rankingGO = PhotonNetwork.Instantiate(_playerRankPrefab.name, transform.position, Quaternion.identity);
            rankingGO.transform.SetParent(_playerRankings.transform);
            rankingGO.transform.localScale = Vector3.one;
            PlayerRank pr = rankingGO.GetComponent<PlayerRank>();
            //scoreKeeper.UpdateRoundScore(player.Player.ActorNumber, player.FruitCount);
            //pr.Initialize(rank, player.Name, scoreKeeper.GetStoredScore(player.Player.ActorNumber), player.Model);
            //pr.Initialize(rank, player.Name, playerScores[player.Player.ActorNumber], player.Model);
            pr.Initialize(rank, player.Name, player.FruitCount, player.Model);
            rank++;
        }
    }

    public void LoadMainMenu()
    {
        StopAllCoroutines();
        PhotonNetwork.LoadLevel(NetworkCustomSettings.MAIN_MENU_SCENE);
    }
    // Call back if the master client leaves the room
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber && !_gameEnding)
        {
            GameObject scoreKeeper = Instantiate(new GameObject(), transform.position, Quaternion.identity);
            scoreKeeper.AddComponent<ScoreKeeper>();
        }
    }
}