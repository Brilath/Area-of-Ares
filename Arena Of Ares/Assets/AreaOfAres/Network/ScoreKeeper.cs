using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreKeeper : MonoBehaviourPun
{
    [SerializeField] private Dictionary<int, int> _scoreBoard;
    public Dictionary<int, int> ScoreBoard { get { return _scoreBoard; } private set { _scoreBoard = value; } }
    private static ScoreKeeper _instance;
    public static ScoreKeeper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreKeeper>();
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<ScoreKeeper>();
                }
            }
            return _instance;
        }
    }
    private bool initialized;

    private void Awake()
    {
        ScoreBoard = new Dictionary<int, int>();
        if (!PhotonNetwork.IsMasterClient) { Destroy(this.gameObject); }
        if (_instance != null) { Destroy(this.gameObject); }
        DontDestroyOnLoad(this);
        // FruitBasket.UpdateBasket += HandlePlayerScore;
    }
    private void OnDestroy()
    {
        // FruitBasket.UpdateBasket -= HandlePlayerScore;
    }
    private void OnEnable()
    {
        // FruitBasket.UpdateBasket += HandlePlayerScore;
    }
    private void OnDisable()
    {
        // FruitBasket.UpdateBasket -= HandlePlayerScore;
    }
    private void Start()
    {
        if (!initialized && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("InitializeScoreKeeper", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void InitializeScoreKeeper()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            ScoreBoard.Add(player.ActorNumber, 0);
        }
        initialized = true;
        Debug.Log("Score Keeper Initialized");
    }

    private void HandlePlayerScore(int playerId, int count)
    {
        photonView.RPC("UpdatePlayerScore", RpcTarget.AllBuffered, playerId, count);
        // UpdatePlayerScore(playerId, count);
    }

    [PunRPC]
    private void UpdatePlayerScore(int playerId, int count)
    {
        if (ScoreBoard.ContainsKey(playerId))
        {
            ScoreBoard[playerId] = count;
            Debug.Log($"Updated Score Keeper for {playerId} to {count}");
        }
    }

    public void UpdateRoundScore(int playerId, int count)
    {
        if (ScoreBoard.ContainsKey(playerId))
        {
            ScoreBoard[playerId] += count;
            Debug.Log($"Updated Score Keeper for {playerId} to {count}");
        }
    }

    public int GetStoredScore(int playerId)
    {
        int score = 0;
        if (ScoreBoard.ContainsKey(playerId))
        {
            score = ScoreBoard[playerId];
        }
        return score;
    }
}
