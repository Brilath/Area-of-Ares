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
            int playerId = other.gameObject.GetComponent<PlayerSetup>().PlayerNumber;
            MovementController movementController = other.gameObject.GetComponent<MovementController>();

            if (fruitBasket.FruitCount > 0 && fruitBasket.Modifiable && PhotonNetwork.IsMasterClient)
            {
                Fruit.DropFruit(playerId, -_damage);
                fruitBasket.Modify(-_damage);
                movementController.KnockBack();
            }
            else if (fruitBasket.FruitCount <= 0 && fruitBasket.Modifiable && PhotonNetwork.IsMasterClient)
            {
                fruitBasket.SetModifiable(false);
                movementController.KnockBack();
            }
        }
    }
}
