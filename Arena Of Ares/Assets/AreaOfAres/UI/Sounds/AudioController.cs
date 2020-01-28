using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AreaOfAres.UI
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AudioController : MonoBehaviour
    {

        [SerializeField] protected SoundSettings _soundSettings;
        [SerializeField] protected AudioSource _audio;

        protected abstract void HandleVolumeChange();
    }
}