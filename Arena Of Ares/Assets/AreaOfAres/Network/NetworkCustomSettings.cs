using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCustomSettings
{
    // Room Custom Properties
    public const int ROOM_LENGTH_MIN = 1;
    public const int ROOM_LENGTH_MAX = 15;
    public const string GAME_MODE = "gameMode";
    public const string SURVIVAL_MODE = "survival";


    // Player Custom Properties
    public const string PLAYER_LOCKED_IN = "playerLockedIn";
    public const string PLAYER_SELECTION_NUMBER = "playerSelectionNumber";
    public const int PLAYER_NAME_MIN = 1;
    public const int PLAYER_NAME_MAX = 10;
}
