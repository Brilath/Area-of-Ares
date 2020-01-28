using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AreaOfAres.UI.DataTypes;

namespace AreaOfAres.UI
{
    public class SettingSlider : MonoBehaviour
    {
        [SerializeField] private UIFloat _setting;
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private Slider _slider;

        public void Initialize(UIFloat setting)
        {
            _setting = setting;
            _textMesh.text = _setting.Text;
            _slider.value = _setting.Amount;
        }

        public void UpdateSetting()
        {
            _setting.SetAmount(_slider.value);
        }
    }
}