using UnityEngine;
using AreaOfAres.UI.DataTypes;

namespace AreaOfAres.UI
{
    [CreateAssetMenu(menuName = "Game Settings/Sound", fileName = "Sound Settings")]
    public class SoundSettings : ScriptableObject
    {
        [SerializeField] private UIBool _mute;
        [SerializeField] private UIFloat _masterVolume;
        [SerializeField] private UIFloat _musicVolume;
        [SerializeField] private UIFloat _sfxVolume;

        public UIBool Mute { get { return _mute; } set { _mute = value; } }
        public UIFloat MasterVolume { get { return _masterVolume; } private set { _masterVolume = value; } }
        public UIFloat MusicVolume { get { return _musicVolume; } private set { _musicVolume = value; } }
        public UIFloat SFXVolume { get { return _sfxVolume; } private set { _sfxVolume = value; } }

        public SoundSettings()
        {
            Mute = new UIBool("Mute Volume", false);
            MasterVolume = new UIFloat("Master Volume", 0.5f);
            MusicVolume = new UIFloat("Music Volume", 0.5f);
            SFXVolume = new UIFloat("Sound Effect Volume", 0.5f);
        }
    }
}