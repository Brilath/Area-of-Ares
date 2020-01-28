using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AreaOfAres.Unit.Data
{
    [CreateAssetMenu(menuName = "Game Settings/Player", fileName = "Player Settings")]
    public class PlayerSettings : ScriptableObject
    {

        [SerializeField] private int _selectedCharacter;

        public int SelectedCharacter { get { return _selectedCharacter; } set { _selectedCharacter = value; } }

        public PlayerSettings()
        {
            _selectedCharacter = 0;
        }

    }
}