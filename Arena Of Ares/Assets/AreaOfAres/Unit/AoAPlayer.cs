using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class AoAPlayer
{
    public Player Player { get; set; }
    public string Name { get; set; }
    public Sprite Model { get; set; }
    public int FruitCount { get; set; }
    public AoAPlayer(Player player, Sprite image, int count)
    {
        Player = player;
        Name = player.NickName;
        Model = image;
        FruitCount = count;
    }
    public void ModifyCount(int amount)
    {
        FruitCount += amount;
    }
}
