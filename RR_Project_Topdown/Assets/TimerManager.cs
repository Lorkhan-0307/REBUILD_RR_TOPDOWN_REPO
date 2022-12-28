using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Text timeText;
    [SerializeField] private int duration = 900;
    private int remainingDuration;
    public int timeScore;

    // Start is called before the first frame update
    void Start()
    {
        Being(duration);
        FindObjectOfType<StageManager>().GameOverEvent += UpdateTimeScore;
    }

    private void Being(int second)
    {
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration >= 0 && !FindObjectOfType<StageManager>().isGameOver)
        {
            timeText.text = $"{ remainingDuration / 60:00} : {remainingDuration % 60:00}";
            remainingDuration--;
            timeScore++;
            yield return new WaitForSeconds(1f);
        }
    }
    
    private void UpdateTimeScore(object sender, EventArgs e)
    {
        //Debug.Log("time score");
        FindObjectOfType<UIManager>().UpdateTimeScore(timeScore);
    }
}
