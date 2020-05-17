using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Modify(-_damage);
            }
            Debug.Log("Hit player");
            //Destroy(other.gameObject);
        }
    }
}
