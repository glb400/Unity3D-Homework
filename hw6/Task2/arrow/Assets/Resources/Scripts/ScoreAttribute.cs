using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAttribute : MonoBehaviour
{
    public int GetScore()
    {
        print(this.gameObject.name);
        int score = 100 / (this.gameObject.name[0] - '0');
        return score;
    }

}
