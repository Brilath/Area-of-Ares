using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpPad : MonoBehaviour
{

    [SerializeField] private Vector2 _jumpForce;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _launchClip;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _audio.clip = _launchClip;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Launch player");
            MovementController movementController = other.gameObject.GetComponent<MovementController>();

            if (PhotonNetwork.IsMasterClient && movementController != null)
            {
                _animator.SetTrigger("launch");
                _audio.Play();
                movementController.AddForce(_jumpForce);
            }
        }
    }

}
