using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class LevelData
{
    public int[] bestScores;
    public bool[] finished;

    public LevelData(int[] bestScores, bool[] finished)
    {
        this.bestScores = bestScores;
        this.finished = finished;
    }

    public LevelData()
    {
        bestScores = new int[100]; //0 by default;
        finished = new bool[100]; //false by default;
    }

    public void changeScore(int levelIndex, int value)
    {
        bestScores[levelIndex] = value;
    }

    public void markLevelFinished(int levelIndex)
    {
        finished[levelIndex] = true;
    }
}
