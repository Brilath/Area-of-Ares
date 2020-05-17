using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AreaOfAres.UI
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private SoundSettings _settings;
        [SerializeField] private SettingSlider _settingSlider;
        [SerializeField] private SettingToggle _settingToggle;

        SettingSlider masterVolumeSlider;
        SettingToggle masterVolumeToggle;

        private void Awake()
        {
            SetupSoundUI();
            _settings.MasterVolume.OnValueChanged += HandleMasterVolumeChange;
            _settings.Mute.OnFlagChanged += HandleMuteColumeToggle;
            SetupAudio();
        }

        private void SetupAudio()
        {
            if (_settings.Mute.Flag)
            {
                AudioListener.volume = 0f;
            }
            else
            {
                AudioListener.volume = _settings.MasterVolume.Amount;
            }
        }

        private void SetupSoundUI()
        {
            masterVolumeToggle = Instantiate(_settingToggle, transform);
            masterVolumeToggle.Initialize(_settings.Mute);

            masterVolumeSlider = Instantiate(_settingSlider, transform);
            masterVolumeSlider.Initialize(_settings.MasterVolume);

            SettingSlider musicVolumeSlider = Instantiate(_settingSlider, transform);
            musicVolumeSlider.Initialize(_settings.MusicVolume);

            SettingSlider sfxVolumeSlider = Instantiate(_settingSlider, transform);
            sfxVolumeSlider.Initialize(_settings.SFXVolume);
        }

        private void HandleMasterVolumeChange()
        {
            if (!_settings.Mute.Flag)
            {
                AudioListener.volume = _settings.MasterVolume.Amount;
            }
        }
        private void HandleMuteColumeToggle(bool flag)
        {
            if (flag)
            {
                AudioListener.volume = 0f;
            }
            else
            {
                AudioListener.volume = _settings.MasterVolume.Amount;
            }
        }

        private void OnDestroy()
        {
            _settings.MasterVolume.OnValueChanged -= HandleMasterVolumeChange;
            _settings.Mute.OnFlagChanged -= HandleMuteColumeToggle;
        }
    }
}