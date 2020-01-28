using UnityEngine;
using AreaOfAres.UI.DataTypes;
using UnityEngine.UI;
using TMPro;

namespace AreaOfAres.UI
{
    public class SettingToggle : MonoBehaviour
    {
        [SerializeField] private UIBool _setting;
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private Toggle _toggle;

        public void Initialize(UIBool setting)
        {
            _setting = setting;
            _textMesh.text = _setting.Text;
            _toggle.isOn = _setting.Flag;
        }

        public void UpdateSetting()
        {
            _setting.SetFlag(_toggle.isOn);
        }
    }
}