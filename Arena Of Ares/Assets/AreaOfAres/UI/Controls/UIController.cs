using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AreaOfAres.UI
{
    [RequireComponent(typeof(SFXController))]
    public class UIController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;
        [SerializeField] private bool _initialized;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
            _initialized = false;
        }
        private void Start()
        {
            _initialized = true;
        }

        public void PlayAudioSound()
        {
            if (_initialized)
                _audio.Play();
        }
    }
}