using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    // Start is called before the first frame update
    void Awake()
    {
        var json = PlayerPrefs.GetString("scores", "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);
    }

    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.timeScored).ThenByDescending(x=>x.caughtBoss).ThenByDescending(x=>x.caughtEnemy);
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
    }

    private void OnDestroy()
    {
        SaveScore();
    }
    public void SaveScore()
    {
        var json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("scores", json);
    }

    public void DeleteAllData()
    {
        PlayerPrefs.DeleteKey("scores");
    }
}
