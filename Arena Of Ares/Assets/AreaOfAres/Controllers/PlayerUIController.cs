using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private Transform playerDashImages;
    private MovementController movementController;
    public void Intitalize(MovementController mover)
    {
        movementController = mover;
        movementController.Dashing += HandleDash;
    }

    private void HandleDash(int dashesLeft)
    {
        int dashImages = playerDashImages.childCount;
        if (dashesLeft < dashImages) { dashImages = dashesLeft; }

        foreach (Transform child in playerDashImages)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < dashImages; i++)
        {
            playerDashImages.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        movementController.Dashing -= HandleDash;
    }
}
