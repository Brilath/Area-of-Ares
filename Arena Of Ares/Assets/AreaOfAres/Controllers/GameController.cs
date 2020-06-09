using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool rejoin; // reset it to false elsewhere
    [SerializeField] private Sprite[] _playerSprites;
    private List<AoAPlayer> _players;
    public List<AoAPlayer> Players { get { return _players; } private set { _players = value; } }
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<GameController>();
                }
            }
            return _instance;
        }
    }
    public int Round { get; set; }

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) { Destroy(this.gameObject); }
        if (_instance != null) { Destroy(this.gameObject); }
        DontDestroyOnLoad(this);
        _players = new List<AoAPlayer>();
        Round = 0;
    }

    void Start()
    {
        GameObject[] playerGOs;
        Sprite playerImage;
        playerGOs = GameObject.FindGameObjectsWithTag("Player");

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object playerSelection;
            if (player.CustomProperties.TryGetValue(NetworkCustomSettings.PLAYER_SELECTION_NUMBER, out playerSelection))
            {
                playerImage = _playerSprites[(int)playerSelection];
                int fruitAmount = 0;
                for (int i = 0; i < playerGOs.Length; i++)
                {
                    GameObject go = playerGOs[i];
                    if (go.GetComponent<PhotonView>().OwnerActorNr == player.ActorNumber)
                    {
                        fruitAmount = go.GetComponent<FruitBasket>().GetFruit();
                    }
                }
                Players.Add(new AoAPlayer(player, playerImage, fruitAmount));
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        { Destroy(this.gameObject); }
    }

    public List<AoAPlayer> UpdatePlayers()
    {
        GameObject[] playerGOs;
        playerGOs = GameObject.FindGameObjectsWithTag("Player");

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int fruitAmount = 0;
            for (int i = 0; i < playerGOs.Length; i++)
            {
                GameObject go = playerGOs[i];
                if (go.GetComponent<PhotonView>().OwnerActorNr == player.ActorNumber)
                {
                    fruitAmount = go.GetComponent<FruitBasket>().GetFruit();
                    Players.Find(p => p.Player == player).ModifyCount(fruitAmount);
                }
            }
        }

        return Players;
    }

    public void LoadNextLevel(int level)
    {
        Round++;
        StopAllCoroutines();
        PhotonNetwork.LoadLevel(level);
    }


    #region Disconnect    
    // TO DO finish reconnection code
    void OnConnectionFail(DisconnectCause cause)
    {
        if (PhotonNetwork.Server == ServerConnection.GameServer)
        {
            switch (cause)
            {
                // add other disconnect causes that could happen while joined
                case DisconnectCause.DisconnectByClientLogic:
                    rejoin = true;
                    break;
                default:
                    rejoin = false;
                    break;
            }
        }
    }

    void OnDisconnectedFromPhoton()
    {
        if (rejoin && !PhotonNetwork.ReconnectAndRejoin())
        {
            Debug.LogError("Error trying to reconnect and rejoin");
        }
    }
    #endregion
}