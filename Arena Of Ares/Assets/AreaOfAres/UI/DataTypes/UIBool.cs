using System;
using UnityEngine;

namespace AreaOfAres.UI.DataTypes
{
    [Serializable]
    public class UIBool
    {
        [SerializeField] private string _text;
        [SerializeField] private bool _flag;
        public string Text { get { return _text; } private set { _text = value; } }
        public bool Flag { get { return _flag; } private set { _flag = value; } }

        public event Action<bool> OnFlagChanged = delegate { };

        public UIBool(string text, bool flag)
        {
            Text = text;
            Flag = flag;
        }

        public void SetFlag(bool flag)
        {
            Flag = flag;
            OnFlagChanged?.Invoke(Flag);
        }
    }
}