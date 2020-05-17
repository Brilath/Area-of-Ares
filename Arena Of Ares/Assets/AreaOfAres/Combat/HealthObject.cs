using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthObject : MonoBehaviour
{
    [SerializeField] private int _heal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Modify(_heal);

                Debug.Log($"Healed player for {_heal}");
            }

            if (this.gameObject != null && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
