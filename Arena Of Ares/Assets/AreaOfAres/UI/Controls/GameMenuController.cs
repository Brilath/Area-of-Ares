using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{

    [SerializeField] private GameObject _gameOptions;
    [SerializeField] private GameObject _keyBindsSettings;
    [SerializeField] private GameObject _soundSettings;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActiveSetting(_keyBindsSettings);
            _gameOptions.SetActive(!_gameOptions.activeSelf);

            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OnKeybindsClicked()
    {
        SetActiveSetting(_keyBindsSettings);
    }

    public void OnSoundSettingsClicked()
    {
        SetActiveSetting(_soundSettings);
    }

    public void OnCloseOptionsClicked()
    {
        SetActiveSetting(_keyBindsSettings);
        _gameOptions.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetActiveSetting(GameObject setting)
    {
        _keyBindsSettings.SetActive(_keyBindsSettings.name.Equals(setting.name));
        _soundSettings.SetActive(_soundSettings.name.Equals(setting.name));
    }
}
