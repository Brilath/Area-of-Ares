using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AreaOfAres.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXController : AudioController
    {
        private void Awake()
        {
            if (_soundSettings != null)
                _soundSettings.SFXVolume.OnValueChanged += HandleVolumeChange;

            _audio = GetComponent<AudioSource>();
        }

        void Start()
        {
            _audio.volume = _soundSettings.SFXVolume.Amount;
        }

        protected override void HandleVolumeChange()
        {
            _audio.volume = _soundSettings.SFXVolume.Amount;
        }
    }
}