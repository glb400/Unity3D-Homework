using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    SSDirector sd;
    Operations op;
    // Start is called before the first frame update
    void Start()
    {
        sd = SSDirector.getInstance();
        op = SSDirector.getInstance() as Operations;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;
        fontStyle.normal.textColor = new Color(1, 0, 0);
        fontStyle.fontSize = 40;
        fontStyle.fontStyle = FontStyle.BoldAndItalic;

        switch(sd.state)
        {
            case State.win:
                GUI.Label(new Rect(320, 180, 100, 100), "Win", fontStyle);
                break;
            case State.lose:
                GUI.Label(new Rect(320, 180, 100, 100), "Lose", fontStyle);
                break;
        }

        if (GUI.Button(new Rect(350, 40, 120, 40), "Restart"))
        {
            op.Restart();
        }

        if (GUI.Button(new Rect(500, 40, 120, 40), "Cruise"))
        {
            op.Cruise();
        }

        if (GUI.Button(new Rect(300, 250, 120, 40), "DownBoard") && sd.state != State.ltr && sd.state != State.rtl)
        {
            op.DownBoard();
        }

        if (GUI.Button(new Rect(20, 40, 120, 40), "PriestToGetOn"))
        {
            op.PriestToGetOn();
        }

        if (GUI.Button(new Rect(180, 40, 120, 40), "DevilToGetOn"))
        {
            op.DevilToGetOn();
        }
    }
}
