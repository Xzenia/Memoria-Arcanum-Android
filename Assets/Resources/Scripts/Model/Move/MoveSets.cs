using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveSets
{
    //Shou moveset
    public static Move cleave = new Move()
    {
        name = "Cleave",
        description = "Shou slashes at the enemy. Deals light damage.",
        attack = 15
    };

    public static Move lightningSlash = new Move()
    {
        name = "Lightning Slash",
        description = "Shou slashes at the enemy at lightning speed. Deals medium damage",
        attack = 20
    };

    public static Move powerSlash = new Move()
    {
        name = "Power Slash",
        description = "Shou slashes at the enemy with deadly force. Deals heavy damage",
        attack = 30
    };

    public static Move zanKei = new Move()
    {
        name = "Zan-Kei",
        description = "Shou uses an ancient technique to punish his enemy. Deals heavy damage",
        attack = 35
    };

    public static Move fatalEnd = new Move()
    {
        name = "Fatal End",
        description = "Shou uses a forbidden series of techniques to destroy his enemy. Deals almighty damage",
        attack = 50
    };

    //Rikko moveset
    public static Move agi = new Move()
    {
        name = "Agi",
        description = "Rikkou casts a burst of fire at the enemy. Deals light damage.",
        attack = 20
    };

    public static Move bufu = new Move()
    {
        name = "Bufu",
        description = "Rikkou casts a torrent of ice at the enemy. Deals light damage",
        attack = 20
    };

    public static Move zio = new Move()
    {
        name = "Zio",
        description = "Rikkou casts a lightning strike at the enemy. Slight chance of paralyzation to enemy. Deals light damage.",
        attack = 20
    };

    public static Move hama = new Move()
    {
        name = "Hama",
        description = "Rikkou casts rays of Holy Light at the enemy. Deals light damage.",
        attack = 30
    };

    public static Move megidolaon = new Move()
    {
        name = "Megidolaon",
        description = "Rikkou casts a wave of dark energy at the enemy. Deals almighty damage.",
        attack = 45
    };

    //Emily moveset
    public static Move arrowRain = new Move()
    {
        name = "Arrow Rain",
        description = "Emily unleashes a rain of arrows in quick succession. Deals light damage.",
        attack = 15
    };

    public static Move frostShot = new Move()
    {
        name = "Frost Shot",
        description = "Emily shoots an arrow with a tip coated in Frostbite potion. Slight chance of paralyzation to enemy. Deals medium damage.",
        attack = 20
    };

    public static Move holyArrow = new Move()
    {
        name = "Holy Arrow",
        description = "Emily shoots an arrow blessed in the Temple of Amon'rah. Deals medium damage.",
        attack = 25
    };

    public static Move torrentShot = new Move()
    {
        name = "Torrent Shot",
        description = "Emily unleashes multiple high velocity arrows at the enemy in quick succession. Deals heavy damage.",
        attack = 35
    };

    public static Move myriadArrows = new Move()
    {
        name = "Myriad Arrows",
        description = "Emily unleashes a full array of holy arrows. Deals almighty damage.",
        attack = 45
    };

    public static Move RetrieveMove(Tile tile)
    {
        if (tile.character == Characters.Emily)
        {
            switch (tile.effectId)
            {
                case 1:
                    return arrowRain;
                case 2:
                    return frostShot;
                case 3:
                    return holyArrow;
                case 4:
                    return torrentShot;
                case 5:
                    return myriadArrows;
                default:
                    Debug.LogError("RetrieveMove(): Emily switch case defaulted!");
                    return cleave;
            }
        }
        else if (tile.character == Characters.Shou)
        {
            switch (tile.effectId)
            {
                case 1:
                    return cleave;
                case 2:
                    return lightningSlash;
                case 3:
                    return powerSlash;
                case 4:
                    return zanKei;
                case 5:
                    return fatalEnd;
                default:
                    Debug.LogError("RetrieveMove(): Shou switch case defaulted!");
                    return arrowRain;
            }
        }
        else if (tile.character == Characters.Rikko)
        {
            switch (tile.effectId)
            {
                case 1:
                    return agi;
                case 2:
                    return bufu;
                case 3:
                    return zio;
                case 4:
                    return hama;
                case 5:
                    return megidolaon;
                default:
                    Debug.LogError("RetrieveMove(): Rikko switch case defaulted!");
                    return agi;
            }
        }
        else
        {
            Debug.LogError("RetrieveMove(): Input tile does not contain a valid Character value");
            return agi;
        }
    }
}