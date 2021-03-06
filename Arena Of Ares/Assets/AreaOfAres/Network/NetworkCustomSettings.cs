﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCustomSettings
{
    // Room Custom Properties
    public const int ROOM_LENGTH_MIN = 1;
    public const int ROOM_LENGTH_MAX = 15;
    public const string GAME_MODE = "gameMode";
    public const string SURVIVAL_MODE = "survival";
    public const int PLAYER_NAME_MIN = 1;
    public const int PLAYER_NAME_MAX = 10;
    public const float GAME_TIME = 45;
    public const float SCORE_SCREEN_TIME = 7;

    // Player Custom Properties
    public const string PLAYER_LOCKED_IN = "playerLockedIn";
    public const string PLAYER_SELECTION_NUMBER = "playerSelectionNumber";
    public const string PLAYER_NUMBER = "playerNumber";
    public const string ACTOR_NUMBER = "actorNumber";

    public const int MAIN_MENU_SCENE = 0;
    public static int[] CurrentLevels()
    {
        int[] levels = new int[] { 1, 2, 3, 4 };
        return levels;
    }
}