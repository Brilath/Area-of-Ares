using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class NetworkMovementController : MonoBehaviourPun, IPunObservable
{
    [Header("Local")]
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 100f;
    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 100f;
    [SerializeField, Range(0f, 100f)]
    private float maxAirAcceleration = 100f;
    [SerializeField, Range(0f, 1f)]
    private float bounciness = 0.5f;

    private Rigidbody2D _body;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 desiredVelocity;
    [SerializeField] private bool desiredJump;
    [SerializeField] private bool onGround;
    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 2f;
    [SerializeField, Range(0, 5)]
    private int maxAirJumps = 0;
    [SerializeField] private int jumpPhase;

    [Header("Network")]
    [SerializeField] private Vector3 _networkPosition;
    [SerializeField] private Vector3 _networkVelocity;
    [SerializeField] private float _networkMovementSpeed;
    [SerializeField] private double _networkLastDataReceivedTime;
    [SerializeField] private Vector3 exterpolatedTargetPosition;
    [SerializeField] private Vector3 newPosition;
    [SerializeField] private float pingInSeconds;
    [SerializeField] private float timeSinceLastUpdate;
    [SerializeField] private float totalTimePassed;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        // PhotonNetwork.SendRate = 60;
        // PhotonNetwork.SerializationRate = 60;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Move
            float playerInput = Input.GetAxis("Horizontal");
            playerInput = Mathf.Min(playerInput, 1f);

            Vector3 movementHorizontal = transform.right * playerInput;
            desiredVelocity = movementHorizontal.normalized * maxSpeed;

            // Jump
            desiredJump |= Input.GetButtonDown("Jump");
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            LocalMovement();
        }
        else
        {
            NetworkMovement();
        }
    }

    private void NetworkMovement()
    {
        pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
        timeSinceLastUpdate = (float)(PhotonNetwork.Time - _networkLastDataReceivedTime);
        totalTimePassed = pingInSeconds + timeSinceLastUpdate;

        // Depending if grounded use the correct acceleration
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        //float maxSpeedChange = acceleration * Time.deltaTime;
        float maxSpeedChange = _networkVelocity.magnitude * Time.deltaTime;

        //Vector3 exterpolatedTargetPosition = _networkPosition + Vector3.Scale(transform.position, _networkVelocity) * totalTimePassed;
        //exterpolatedTargetPosition = _networkPosition + Vector3.Scale(transform.position, _networkVelocity) * totalTimePassed;

        //newPosition = Vector3.MoveTowards(transform.position, exterpolatedTargetPosition, _body.velocity.magnitude * Time.fixedDeltaTime);
        //newPosition.x = Mathf.MoveTowards(transform.position.x, exterpolatedTargetPosition.x, maxSpeedChange);
        //newPosition.y = Mathf.MoveTowards(transform.position.y, exterpolatedTargetPosition.y, maxSpeedChange);

        velocity.x =
            Mathf.MoveTowards(velocity.x, _networkVelocity.x, maxSpeedChange);
        velocity.y =
        Mathf.MoveTowards(velocity.x, _networkVelocity.y, maxSpeedChange);

        if (Vector3.Distance(transform.position, exterpolatedTargetPosition) > 2f)
        {
            newPosition = exterpolatedTargetPosition;
        }

        //newPosition.y = Mathf.Clamp(newPosition.y, 0.0f, 50f);
        _body.velocity = velocity;
        //transform.position = newPosition;
    }

    private void LocalMovement()
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

        _body.velocity = velocity;
        onGround = false;
    }

    private void UpdateState()
    {
        velocity = _body.velocity;
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
            stream.SendNext(transform.position);
            stream.SendNext(_body.velocity);
            stream.SendNext(_body.velocity.magnitude);
        }
        else
        {
            _networkPosition = (Vector3)stream.ReceiveNext();
            _networkVelocity = (Vector2)stream.ReceiveNext();
            _networkMovementSpeed = (float)stream.ReceiveNext();
            _networkLastDataReceivedTime = info.SentServerTime;
        }
    }
}
