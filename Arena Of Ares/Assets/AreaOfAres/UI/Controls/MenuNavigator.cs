using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AreaOfAres.UI
{
    public class MenuNavigator : MonoBehaviour
    {
        private MainMenuController menuController;
        [SerializeField] private GameObject defaultUIButton;
        [SerializeField] private GameObject defaultUIJoinButton;
        [SerializeField] private GameObject defaultUIRoomButton;
        [SerializeField] private GameObject uiOptions;
        [SerializeField] private GameObject defaultUIOptionsButton;

        private bool mainMenu;


        private void Awake()
        {
            menuController = GetComponent<MainMenuController>();
            if (menuController == null)
            { mainMenu = false; }
            else
            { mainMenu = true; }
        }

        void Update()
        {
            if (mainMenu)
                MainMenuNavigation();
            else
                GameMenuNavigation();
        }

        private void MainMenuNavigation()
        {
            // Close Panels
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire3"))
            {
                menuController.HideAllCanvases();
            }
        }
        private void GameMenuNavigation()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!uiOptions.activeInHierarchy)
                {
                    uiOptions.SetActive(true);

                }
            }
        }
    }
}