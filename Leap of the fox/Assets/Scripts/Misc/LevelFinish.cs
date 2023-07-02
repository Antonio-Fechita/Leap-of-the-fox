using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    private float levelStartTime;
    private float levelFinishTime;
    private int sceneBuildIndex;
    [SerializeField] GameObject LevelFinishScreen;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI deaths;
    [SerializeField] TextMeshProUGUI gems;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] Image medal;
    [SerializeField] Sprite bronzeMedal;
    [SerializeField] Sprite silverMedal;
    [SerializeField] Sprite goldMetal;

    LevelData levelData;
    private int numberOfDeaths;
    void Start()
    {
        levelData = SaveSystem.LoadLevels();
        Time.timeScale = 1f;
        numberOfDeaths = 0;
        Respawn.respawnEvent.AddListener(IncreaseNumberOfDeaths);
        levelStartTime = Time.time;
        sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void IncreaseNumberOfDeaths()
    {
        numberOfDeaths++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelFinishTime = Time.time - levelStartTime;
            Debug.Log("Level finished in " + ConvertSecondsToTimePassedString(levelFinishTime));



            int gemsCollectedThisLevel = Collectables.gemsCollected;
            int gemsCollectedOverall = SaveSystem.LoadGems().currentGems;
            gemsCollectedOverall += gemsCollectedThisLevel;
            SaveSystem.SaveGems(new GemsSaveData(gemsCollectedOverall));
            
            Debug.Log("Gems collected this level" + gemsCollectedThisLevel);

            Time.timeScale = 0;

            //set level finish screen data (time, deaths, gems, score)
            time.text = (((int)levelFinishTime / 60) < 10 ? "0" + ((int)levelFinishTime / 60).ToString() : ((int)levelFinishTime / 60).ToString()) + ":" +
                        (((int)levelFinishTime % 60) < 10 ? "0" + ((int)levelFinishTime % 60).ToString() : ((int)levelFinishTime % 60).ToString());
            deaths.text = numberOfDeaths.ToString();
            gems.text = Collectables.gemsCollected.ToString();
            Collectables.gemsCollected = 0;
            int numericalScore = (int) Mathf.Max(100, (1000 + 50 * Collectables.gemsCollected - levelFinishTime * 3 - 100 * numberOfDeaths));
            score.text = numericalScore.ToString();

            if(numericalScore < 400)
            {
                medal.sprite = bronzeMedal;
            }
            else if(numericalScore >= 400 && numericalScore <= 700)
            {
                medal.sprite = silverMedal;
            }
            else
            {
                medal.sprite = goldMetal;
            }

            LevelFinishScreen.SetActive(true);

            Debug.Log("Comparing old best of: " + levelData.bestScores[sceneBuildIndex] + " with new score of " + numericalScore + " in scene with build index " + sceneBuildIndex);
            if (levelData.bestScores[sceneBuildIndex] < numericalScore)
            {
                levelData.changeScore(sceneBuildIndex, numericalScore);
            }
            else
                Debug.Log("Current best score for this level:" + levelData.bestScores[sceneBuildIndex]);
            levelData.markLevelFinished(sceneBuildIndex);
            SaveSystem.SaveLevels(levelData);
        }

    }



    private string ConvertSecondsToTimePassedString(float seconds)
    {
        string timeString = "";
        int minutes = (int)seconds / 60;
        if (minutes != 0)
        {
            if (minutes > 1)
                timeString = minutes.ToString() + " minutes ";
            else
                timeString = minutes.ToString() + " minute ";
        }

        timeString += (seconds - (60 * minutes)).ToString() + " seconds";
        return timeString;
    }

}
