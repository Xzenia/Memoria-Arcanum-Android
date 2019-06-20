using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileId;
    public int effectId;
    public Sprite tile;
    public Characters character;
    public TileType tileType;
}

public enum TileType
{
    Move, Item
}
