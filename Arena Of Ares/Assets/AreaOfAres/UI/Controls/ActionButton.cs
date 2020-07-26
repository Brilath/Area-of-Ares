using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public static Action<string> OnButtonAction = delegate { };

    public void ActionButtonPressed(string value)
    {
        OnButtonAction?.Invoke(value);
    }
}
