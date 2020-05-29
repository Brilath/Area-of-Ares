﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementController : MonoBehaviourPun
{
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 100f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 100f;
    [SerializeField, Range(0f, 100f)]
    private float maxAirAcceleration = 100f;

    [SerializeField, Range(0f, 1f)]
    private float bounciness = 0.5f;

    private Rigidbody2D body;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 desiredVelocity;
    [SerializeField] private bool desiredJump;
    [SerializeField] private bool onGround;
    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 2f;
    [SerializeField, Range(0, 5)]
    private int maxAirJumps = 0;
    [SerializeField] private int jumpPhase;

    private PlayerSoundController soundController;

    [SerializeField] private float knockBackLength;
    [SerializeField] private float knockBackCounter;
    [SerializeField] private float knockBackPower;
    [SerializeField] private Vector2 knockBackForce;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        soundController = GetComponent<PlayerSoundController>();
    }

    void Update()
    {
        float playerInput = Input.GetAxis("Horizontal");

        if (knockBackCounter <= 0)
        {
            // Move    
            playerInput = Mathf.Min(playerInput, 1f);
            Vector3 movementHorizontal = transform.right * playerInput;
            desiredVelocity = movementHorizontal.normalized * maxSpeed;

            // Jump
            desiredJump |= Input.GetButtonDown("Jump");
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (knockBackCounter <= 0)
        {
            UpdateState();

            // Depending if grounded use the correct acceleration
            float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;

            velocity.x =
                Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

            if (desiredJump)
            {
                desiredJump = false;
                Jump();
            }

            body.velocity = velocity;
            onGround = false;
        }
    }

    private void UpdateState()
    {
        velocity = body.velocity;
        if (onGround)
        {
            jumpPhase = 0;
        }
    }

    private void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            // Limit Upward Velocity
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
            soundController.PlayJumpSound();
        }
    }

    // Check if collision is with the ground
    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        EvaluateCollision(other);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        EvaluateCollision(other);
    }

    public void KnockBack()
    {
        photonView.RPC("RPCKnockBack", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void RPCKnockBack()
    {
        knockBackCounter = knockBackLength;

        if (body.velocity.x <= 0.0f)
        {
            knockBackForce.x = knockBackPower;
        }
        else
        {
            knockBackForce.x = -knockBackPower;
        }

        if (body.velocity.y <= 0.0f)
        {
            knockBackForce.y = knockBackPower;
        }
        else
        {
            knockBackForce.y = -knockBackPower;
        }

        body.velocity = new Vector2(0, 0);
        body.AddForce(knockBackForce, ForceMode2D.Impulse);
    }
    public void AddForce(Vector2 force)
    {
        photonView.RPC("RPCAddForce", RpcTarget.AllBuffered, force);
    }
    [PunRPC]
    private void RPCAddForce(Vector2 force)
    {
        body.velocity = new Vector2(0, 0);
        body.AddForce(force, ForceMode2D.Impulse);
    }
}
