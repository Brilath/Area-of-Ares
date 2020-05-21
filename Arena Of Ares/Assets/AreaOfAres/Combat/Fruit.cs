using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Fruit : MonoBehaviour
{
    [SerializeField] public int Amount;

    public static event Action<Fruit, int> OnCollected = delegate { };
    public static event Action<Fruit, int> OnDropped = delegate { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.isTrigger)
        {
            FruitBasket fruitBasket = other.gameObject.GetComponent<FruitBasket>();
            if (fruitBasket != null)
            {
                fruitBasket.Modify(Amount);
            }

            if (this.gameObject != null && PhotonNetwork.IsMasterClient)
            {
                int playerId = other.gameObject.GetComponent<PhotonView>().OwnerActorNr;
                Debug.Log($"Collection fruit for player id {playerId}");
                if (playerId != 0)
                {
                    OnCollected(this, playerId);
                }
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
