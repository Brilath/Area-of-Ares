using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamagePlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        DamagePlayer(other);
    }

    private void DamagePlayer(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit player");

            FruitBasket fruitBasket = other.gameObject.GetComponent<FruitBasket>();
            MovementController movementController = other.gameObject.GetComponent<MovementController>();

            if (PhotonNetwork.IsMasterClient && fruitBasket.Modifiable)
            {
                fruitBasket.Modify(-_damage);
                movementController.KnockBack();
            }
        }
    }
}
