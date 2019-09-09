using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{ 
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;
        fontStyle.normal.textColor = new Color(1, 0, 0);
        fontStyle.fontSize = 40;
        fontStyle.fontStyle = FontStyle.BoldAndItalic;

        GUIStyle fontStyle2 = new GUIStyle();
        fontStyle2.normal.background = null;
        fontStyle2.fontSize = 40;
        fontStyle2.fontStyle = FontStyle.BoldAndItalic;

        GUI.Label(new Rect(255, 70, 100, 100), "TicTacToe", fontStyle2);

        if (GUI.Button(new Rect(120, 40, 100, 50), "Reset"))
            Func.Reset();

        if (Func.IsFinish() && Param.result == 1)
        {
            GUI.Label(new Rect(190, 360, 100, 100), "Player Two Wins", fontStyle);
            Param.result = 0;
        }
        else if (Func.IsFinish() && Param.result == 2)
        {
            GUI.Label(new Rect(190, 360, 50, 50), "Player One Wins", fontStyle);
            Param.result = 0;
        }
        else if (Func.IsFinish() && Param.cnt == 9)
        {
            GUI.Label(new Rect(300, 360, 50, 50), "Draw", fontStyle);
        }
    }
}
