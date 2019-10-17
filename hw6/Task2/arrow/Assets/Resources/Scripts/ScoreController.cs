using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int score;
    public void ClearScore()
    {
        score = 0;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public int GetScore()
    {
        return this.score;
    }

}
