using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBasket : MonoBehaviour
{
    [SerializeField] private int _maxFruit;
    [SerializeField] private int _currentFruit;
    [SerializeField] private bool _modifiable;

    public int FruitCount { get { return _currentFruit; } set { _currentFruit = value; } }

    private void Start()
    {
        FruitCount = 0;
        _modifiable = true;
    }

    public void Modify(int amount)
    {
        if (_modifiable)
        {
            FruitCount += amount;

            FruitCount = Mathf.Clamp(FruitCount, 0, _maxFruit);
        }
    }

    public int GetFruit()
    {
        return FruitCount;
    }

    public void SetModifiable(bool status)
    {
        _modifiable = status;
    }
}
