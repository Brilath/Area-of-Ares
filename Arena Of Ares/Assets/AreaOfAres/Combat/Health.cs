using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private bool _modifiable;
    [SerializeField] private float _modifyCooldown;
    private PlayerSoundController _playerSoundController;

    private void Awake()
    {
        _playerSoundController = GetComponent<PlayerSoundController>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _modifiable = true;
    }

    public void Modify(int amount)
    {
        if (_modifiable)
        {
            _currentHealth += amount;

            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            if (amount > 0)
                _playerSoundController.PlayHealSound();
            else if (amount < 0)
                _playerSoundController.PlayDamageSound();

            CheckState();

            StartCoroutine("StopModify");
        }
    }

    private void CheckState()
    {
        if (_currentHealth <= 0)
        {
            Debug.Log("Player has died");
        }
    }

    private IEnumerator StopModify()
    {
        _modifiable = false;
        yield return new WaitForSeconds(_modifyCooldown);
        _modifiable = true;
    }
}
