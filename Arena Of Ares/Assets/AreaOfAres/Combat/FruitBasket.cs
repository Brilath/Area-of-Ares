using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBasket : MonoBehaviour
{
    [SerializeField] private int _maxFruit;
    [SerializeField] private int _currentFruit;
    [SerializeField] private bool _modifiable;

    private void Start()
    {
        _currentFruit = 0;
        _modifiable = true;
    }

    public void Modify(int amount)
    {
        if (_modifiable)
        {
            _currentFruit += amount;

            _currentFruit = Mathf.Clamp(_currentFruit, 0, _maxFruit);
        }
    }

    public void SetModifiable(bool status)
    {
        _modifiable = status;
    }
}
