﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AreaOfAres.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicController : AudioController
    {
        private void Awake()
        {
            if (_soundSettings != null)
                _soundSettings.MusicVolume.OnValueChanged += HandleVolumeChange;

            _audio = GetComponent<AudioSource>();
        }

        void Start()
        {
            _audio.volume = _soundSettings.MusicVolume.Amount;
        }

        protected override void HandleVolumeChange()
        {
            _audio.volume = _soundSettings.MusicVolume.Amount;
        }

        private void OnDestroy()
        {
            _soundSettings.MusicVolume.OnValueChanged -= HandleVolumeChange;
        }
    }
}