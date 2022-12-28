using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Score
{
    public string name;
    public int timeScored;
    public int caughtBoss;
    public int caughtEnemy;

    public Score(string name, int TimeScored, int caughtBoss, int caughtEnemy)
    {
        this.name = name;
        this.timeScored = TimeScored;
        this.caughtBoss = caughtBoss;
        this.caughtEnemy = caughtEnemy;
    }
}
