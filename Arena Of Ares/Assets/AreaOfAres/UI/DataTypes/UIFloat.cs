using System;
using UnityEngine;

namespace AreaOfAres.UI.DataTypes
{
    [Serializable]
    public class UIFloat
    {
        [SerializeField] private string _text;
        [SerializeField] private float _amount;
        public string Text { get { return _text; } private set { _text = value; } }
        public float Amount { get { return _amount; } private set { _amount = value; } }

        public event Action OnValueChanged = delegate { };

        public UIFloat(string text, float amount)
        {
            Text = text;
            Amount = amount;
        }

        public void SetAmount(float amount)
        {
            Amount = amount;
            OnValueChanged?.Invoke();
        }
    }
}