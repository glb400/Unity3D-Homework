using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGUI : MonoBehaviour
{
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            border = new RectOffset(10, 10, 10, 10),
            fontSize = 20,
            fontStyle = FontStyle.Bold,
        };

        if (GUI.Button(new Rect(600, 50, 100, 50), "Reset"))
            Director.GetInstance().CurrentSceneController.LoadSource();
        GUI.Label(new Rect(20, 10, 200, 50), "Score : " + Singleton<ScoreController>.Instance.GetScore().ToString(), style);
        GUI.Label(new Rect(20, 50, 200, 50), "Wind Direction : [" + Singleton<WindController>.Instance.GetDirection().x.ToString() + "," +
            Singleton<WindController>.Instance.GetDirection().y.ToString() + "]", style);
        GUI.Label(new Rect(20, 90, 200, 50), "Wind Strength : " + Singleton<WindController>.Instance.GetStrength().ToString(), style);
    }
}