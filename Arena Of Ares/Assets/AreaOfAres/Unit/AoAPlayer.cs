using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class AoAPlayer
{
    public Player Player { get; set; }
    public string Name { get; set; }
    public Sprite Model { get; set; }
    public int Amount { get; set; }
    public AoAPlayer(Player player, Sprite image, int amount)
    {
        Player = player;
        Name = player.NickName;
        Model = image;
        Amount = amount;
    }
}
