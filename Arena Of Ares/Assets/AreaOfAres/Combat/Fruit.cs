using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Fruit : MonoBehaviourPun
{
    [SerializeField] public int Amount;

    public static event Action<Fruit, int> OnCollected = delegate { };
    public static event Action<int, int> OnDropped = delegate { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collection fruit {photonView.InstantiationId} for player");

        if (other.gameObject.tag == "Player")
        {
            FruitBasket fruitBasket = other.gameObject.GetComponent<FruitBasket>();
            if (fruitBasket != null)
            {
                fruitBasket.Modify(Amount);
            }

            if (this.gameObject != null)
            {
                int playerId = other.gameObject.GetComponent<PlayerSetup>().PlayerNumber;
                Debug.Log($"Collection fruit {photonView.InstantiationId} for player id {playerId}");
                OnCollected(this, playerId);
                if (!photonView.IsMine && PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    photonView.RPC("DestroyFruit", RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    void DestroyFruit()
    {
        Destroy(this.gameObject);
    }

    public static void DropFruit(int playerID, int amount)
    {
        OnDropped(playerID, amount);
    }
}
