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
        if (sd.getController() == null)
            sd.setController(GetComponent<Controller>() as Controller);
        if (sd.getModel() == null)
            sd.setModel(GetComponent<Model>() as Model);
    }

    // Update is called once per frame
    void Update()
    {
        // respond when there's a hit from user
        op.Hit();
    }

    public void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;
        fontStyle.normal.textColor = new Color(1, 0, 0);
        fontStyle.fontSize = 20;
        fontStyle.fontStyle = FontStyle.BoldAndItalic;

        switch (sd.getController().state)
        {
            case State.RUN:
                GUI.Label(new Rect(320, 380, 100, 100), "Running", fontStyle);
                break;
            case State.STOP:
                GUI.Label(new Rect(320, 380, 100, 100), "Stopped", fontStyle);
                break;
            case State.WIN:
                GUI.Label(new Rect(380, 380, 100, 100), "Win", fontStyle);
                break;
            case State.LOSE:
                GUI.Label(new Rect(360, 380, 100, 100), "Lose", fontStyle);
                break;
            case State.RESET:
                GUI.Label(new Rect(340, 380, 100, 100), "Reset", fontStyle);
                break;
        }

         // buttons for user
        if (GUI.Button(new Rect(140, 40, 120, 40), "Pause"))
        {
            op.Pause();
        }

        if (GUI.Button(new Rect(290, 40, 120, 40), "Resume"))
        {
            op.Resume();
        }

        if (GUI.Button(new Rect(440, 40, 120, 40), "Restart"))
        {
            op.Restart();
        }

        // user interface
        GUI.Label(new Rect(100, 90, 100, 100), "Points: " + (int)sd.getModel().getTotal(), fontStyle);
        GUI.Label(new Rect(250, 90, 100, 100), "Round: " + sd.getController().round, fontStyle);
        GUI.Label(new Rect(400, 90, 100, 100), "TimeLeft: " + sd.getController().restTime, fontStyle);
        GUI.Label(new Rect(550, 90, 100, 100), "Hits: " + sd.getController().hits, fontStyle);
    }

}
