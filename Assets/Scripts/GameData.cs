using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int coinCount;
    public int bestScore;

    public GameData()
    {
        coinCount = 0;
        bestScore = 0;
    }

    public GameData(int coinCount, int bestScore)
    {
        this.coinCount = coinCount;
        this.bestScore = bestScore;
    }
}
