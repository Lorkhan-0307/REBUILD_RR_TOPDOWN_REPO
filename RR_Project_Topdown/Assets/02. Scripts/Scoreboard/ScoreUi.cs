using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    public RowUi rowUi;
    public ScoreManager scoreManager;
    private void Start()
    {

        

        var scores = scoreManager.GetHighScores().ToArray();

        for(int i=0; i<Math.Min(scores.Length,10); i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.rank.text = (i + 1).ToString();
            row.playerId.text = scores[i].name.ToString();
            row.leftTime.text = scores[i].timeScored.ToString();
            row.caughtBoss.text = scores[i].caughtBoss.ToString();
            row.caughtEnemy.text = scores[i].caughtEnemy.ToString();
        }
    }
}
