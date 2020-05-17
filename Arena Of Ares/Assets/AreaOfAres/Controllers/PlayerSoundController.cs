using System.Collections;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _healClip;
    [SerializeField] private AudioClip _damageClip;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _isPlaying;
    private bool onGround;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isPlaying)
        {
            float _playerInput = Input.GetAxis("Horizontal");

            if ((_playerInput > 0.01f || _playerInput < -0.01f) && onGround)
            {
                StartCoroutine("PlayClip", _walkClip);
            }
        }
        onGround = false;
    }

    private IEnumerator PlayClip(AudioClip clip)
    {
        _isPlaying = true;
        _audioSource.clip = clip;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length * 1.5f);
        _isPlaying = false;
    }

    public void PlayHealSound()
    {
        _audioSource.PlayOneShot(_healClip);
    }

    public void PlayDamageSound()
    {
        _audioSource.PlayOneShot(_damageClip);
    }

    public void PlayJumpSound()
    {
        _audioSource.PlayOneShot(_jumpClip);
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
}
