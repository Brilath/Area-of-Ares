using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit player");

            FruitBasket fruitBasket = other.gameObject.GetComponent<FruitBasket>();
            PhotonView view = other.gameObject.GetComponent<PhotonView>();
            if (view != null && fruitBasket.FruitCount > 0 && PhotonNetwork.IsMasterClient)
            {
                int playerId = view.OwnerActorNr;
                Fruit.DropFruit(playerId, -_damage);
            }
            if (fruitBasket != null)
            {
                fruitBasket.Modify(-_damage);
            }
        }
    }
}
