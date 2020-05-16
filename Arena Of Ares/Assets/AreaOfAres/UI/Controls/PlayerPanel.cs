using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AreaOfAres.Network;
using AreaOfAres.Unit.Data;
using Photon.Realtime;

namespace AreaOfAres.UI.Controls
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _characterImage;
        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;
        [SerializeField] private GameObject _ready;
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private Player _player;

        public void Initialize(Player player)
        {
            _player = player;

            _name.gameObject.SetActive(true);
            _characterImage.gameObject.SetActive(true);

            if (_player.IsLocal)
            {
                _leftArrow.gameObject.SetActive(true);
                _rightArrow.gameObject.SetActive(true);
                _ready.gameObject.SetActive(true);
            }
        }
        public void OnReadyClicked()
        {
            //_player.Ready = true;
        }
    }
}