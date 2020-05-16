using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<AnimationController>().enabled = true;
            transform.GetComponent<PlayerSoundController>().enabled = true;
        }
        else
        {
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<AnimationController>().enabled = false;
            transform.GetComponent<PlayerSoundController>().enabled = false;
        }
    }
}
