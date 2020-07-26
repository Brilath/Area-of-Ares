using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class AnimationController : MonoBehaviourPun, IPunObservable
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _velocityX = "velocityX";
    [SerializeField] private string _velocityY = "velocityY";
    [SerializeField] private float _playerInput;
    [SerializeField] private bool _desiredJump;

    [Header("Spirit")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _isSpiritFlipped;

    [Header("Particals")]
    [SerializeField] private ParticleSystem dust;

    private Rigidbody2D _body;
    [SerializeField] private Joystick _joystick;

    private void Awake()
    {
#if UNITY_ANDROID
        _joystick = FindObjectOfType<Joystick>();
#endif
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _isSpiritFlipped = false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        _playerInput = _joystick.Horizontal;
#else
        _playerInput = Input.GetAxis("Horizontal");
#endif
        SetSpriteDirection();

        _animator.SetFloat(_velocityX, _body.velocity.x);
        _animator.SetFloat(_velocityY, _body.velocity.y);
    }

    private void SetSpriteDirection()
    {
        if (_playerInput > 0.0f)
        {
            _isSpiritFlipped = false;
        }
        else if (_playerInput < 0.0f)
        {
            _isSpiritFlipped = true;
        }
        _spriteRenderer.flipX = _isSpiritFlipped;
    }

    private void AddObservable()
    {
        if (!photonView.ObservedComponents.Contains(this))
        {
            photonView.ObservedComponents.Add(this);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_spriteRenderer.flipX);
        }
        else
        {
            _spriteRenderer.flipX = (bool)stream.ReceiveNext();
        }
    }
    public void PlayDash()
    {
        _animator.SetTrigger("dash");
    }
    public void PlayDust()
    {
        dust.Play();
    }
}
