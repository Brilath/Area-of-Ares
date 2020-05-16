using System.Collections;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _isPlaying;

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

            if (_playerInput > 0.01f || _playerInput < -0.01f)
            {
                StartCoroutine("PlayClip", _walkClip);
            }
        }
    }

    private IEnumerator PlayClip(AudioClip clip)
    {
        _isPlaying = true;
        _audioSource.clip = clip;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length * 1.5f);
        _isPlaying = false;
    }

    public void PlayJumpSound()
    {
        _audioSource.PlayOneShot(_jumpClip);
    }
}
