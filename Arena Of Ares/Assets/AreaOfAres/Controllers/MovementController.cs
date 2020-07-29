using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class MovementController : MonoBehaviourPun
{
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 100f;
    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 100f;
    [SerializeField, Range(0f, 100f)]
    private float maxAirAcceleration = 100f;
    [SerializeField, Range(0f, 1f)]
    private float bounciness = 0.5f;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 desiredVelocity;
    [Header("Jump")]
    [SerializeField] private bool desiredJump;
    [SerializeField] private bool onGround;
    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 2f;
    [SerializeField, Range(0, 5)]
    private int maxAirJumps = 0;
    [SerializeField] private int jumpPhase;
    [SerializeField] private float jumpGCD = .1f;
    [SerializeField] private float jumpGCDCounter = .1f;
    [Header("Dash")]
    [SerializeField] private bool desiredDash;
    [SerializeField] private Vector2 dashPower;
    [SerializeField, Range(0f, 15f)] private float dashCooldown = 2f;
    [SerializeField] private float dashCounter = 0f;
    [SerializeField, Range(0, 5)] private int maxDashes = 0;
    [SerializeField] private int dashPhase;
    [SerializeField] private float dashGCD = .1f;
    [SerializeField] private float dashGCDCounter = .1f;
    [Header("Knockback")]
    [SerializeField] private float knockBackLength;
    [SerializeField] private float knockBackCounter;
    [SerializeField] private float knockBackPower;
    [SerializeField] private Vector2 knockBackForce;
    [SerializeField] private Joystick joystick;
    private Rigidbody2D body;
    private PlayerSoundController soundController;
    private float playerInput;
    private Touch touch;
    private float touchDirection;
    [SerializeField] private TextMeshProUGUI textTouch;

    // Events
    public event Action<int> Dashing = delegate { };

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        body = GetComponent<Rigidbody2D>();
        soundController = GetComponent<PlayerSoundController>();
        dashCounter = dashCooldown;
        ActionButton.OnButtonAction += HandleOnButtonAction;
    }
    private void OnDestroy()
    {
        ActionButton.OnButtonAction -= HandleOnButtonAction;
    }

    void Update()
    {
#if UNITY_ANDROID
        //playerInput = joystick.Horizontal;
        if (Input.touchCount > 0)
        {
            foreach (Touch t in Input.touches)
            {
                if (t.position.x < Screen.width / 2)
                {
                    //textTouch.text = t.deltaPosition.x.ToString();
                    if (t.phase == TouchPhase.Moved && Mathf.Abs(t.deltaPosition.x) > 1.0)
                    {
                        touchDirection = t.deltaPosition.x;
                        playerInput = touchDirection;
                    }
                    else if (t.phase != TouchPhase.Stationary && Mathf.Abs(t.deltaPosition.x) < 1.0)
                    {
                        playerInput = touchDirection;
                    }
                    else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
                    {
                        touchDirection = 0;
                    }
                }
                else
                {
                    playerInput = touchDirection;
                }
                // Dash
                if (t.position.x > Screen.width / 2 &&
                    t.position.y > Screen.height / 2)
                {
                    if (dashGCDCounter <= 0)
                    {
                        desiredDash |= true;
                        dashGCDCounter = dashGCD;
                    }
                }
                // Jump
                if (t.position.x > Screen.width / 2 &&
                    t.position.y < Screen.height / 2)
                {
                    if (jumpGCDCounter <= 0)
                    {
                        desiredJump |= true;
                        jumpGCDCounter = jumpGCD;
                    }
                }
            }
        }
        else
        {
            playerInput = 0;
        }
#else
        playerInput = Input.GetAxis("Horizontal");
#endif

        if (knockBackCounter <= 0)
        {
            // Move    
            playerInput = Mathf.Min(playerInput, 1f);
            Vector3 movementHorizontal = transform.right * playerInput;
            desiredVelocity = movementHorizontal.normalized * maxSpeed;
            if (playerInput != 0 && onGround)
            { GetComponent<AnimationController>().PlayDust(); }

            // Jump
            desiredJump |= Input.GetButtonDown("Jump");

            // Dash
            desiredDash |= Input.GetButtonDown("Dash");
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }
        if (jumpGCDCounter > 0)
        {
            jumpGCDCounter -= Time.deltaTime;
        }
        if (dashGCDCounter > 0)
        {
            dashGCDCounter -= Time.deltaTime;
        }

        UpdateDashState();
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
            if (desiredDash)
            {
                desiredDash = false;
                Dash();
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
            jumpPhase++;
            // Limit Upward Velocity
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
            soundController.PlayJumpSound();
            GetComponent<AnimationController>().PlayDust();
        }
    }

    private void Dash()
    {
        Vector2 dashForce = Vector2.zero;

        if (dashPhase < maxDashes)
        {
            if (dashPhase == 0)
            {
                dashCounter = dashCooldown;
            }

            dashPhase++;

            if (playerInput > 0.0f)
            {
                dashForce = transform.right * dashPower.x;
            }
            else if (playerInput < 0.0f)
            {
                dashForce = transform.right * -dashPower.x;
            }

            if (onGround) { dashForce.y = dashPower.y; }
            Dashing(maxDashes - dashPhase);
            GetComponent<AnimationController>().PlayDash();
            GetComponent<AnimationController>().PlayDust();
            body.AddForce(dashForce);
            soundController.PlayDashSound();
        }
    }
    private void UpdateDashState()
    {
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
        }
        if (dashCounter <= 0 && dashPhase > 0)
        {
            dashPhase--;
            Dashing(maxDashes - dashPhase);
        }
        if (dashCounter <= 0 && dashPhase > 0)
        {
            dashCounter = dashCooldown;
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
        GetComponent<AnimationController>().PlayDust();
        body.AddForce(force, ForceMode2D.Impulse);
    }

    private void HandleOnButtonAction(string action)
    {
        if (action.CompareTo("jump") == 0)
        {

            desiredJump |= true;
        }
        else if (action.CompareTo("roll") == 0)
        {
            desiredDash |= true;
        }
    }
}
