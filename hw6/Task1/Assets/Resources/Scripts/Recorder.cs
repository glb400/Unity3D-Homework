using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recorder : MonoBehaviour {
    public Text ScoreText;
    public float Score = 0;
    public SceneController sceneControler;
    void Start () {
        sceneControler = (SceneController)SSDirector.getInstance().currentScenceController;
        sceneControler.scoreRecorder = this;
    }
    // If we hit Score raise
	public void hit()
    {
        Score += sceneControler.round;
    }
    // If we miss Score drop
    public void miss()
    {
        Score -= sceneControler.round;
    }
	// Update is called once per frame
	void Update () {
        ScoreText.text = "Score:" + Score.ToString();
    }
}
