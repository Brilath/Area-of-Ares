using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AreaOfAres.UI
{
    public class GameMenuController : MonoBehaviour
    {

        [SerializeField] private GameObject _gameOptions;
        [SerializeField] private GameObject _defaultOptionsButton;
        [SerializeField] private GameObject _keyBindsSettings;
        [SerializeField] private GameObject _soundSettings;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //SetActiveSetting(_keyBindsSettings);
                _gameOptions.SetActive(!_gameOptions.activeSelf);

                if (_gameOptions.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(_defaultOptionsButton);
                }
#if UNITY_ANDROID
                Debug.Log("Android no cursor");
#else

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
#endif

            }
        }

        public void OnKeybindsClicked()
        {
            SetActiveSetting(_keyBindsSettings);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_defaultOptionsButton);
        }

        public void OnSoundSettingsClicked()
        {
            SetActiveSetting(_soundSettings);
            GameObject _defaultSoundButton;
            _defaultSoundButton = FindObjectOfType<Toggle>().gameObject;
            Debug.Log($"{_defaultSoundButton.name} found");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_defaultSoundButton);
        }

        public void OnCloseOptionsClicked()
        {
            SetActiveSetting(_keyBindsSettings);
            _gameOptions.SetActive(false);
#if UNITY_ANDROID
            Debug.Log("Android no cursor");
#else
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
#endif
        }

        private void SetActiveSetting(GameObject setting)
        {
            _keyBindsSettings.SetActive(_keyBindsSettings.name.Equals(setting.name));
            _soundSettings.SetActive(_soundSettings.name.Equals(setting.name));
        }
    }
}