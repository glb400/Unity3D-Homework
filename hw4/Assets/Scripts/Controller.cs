using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { l, r, ltr, rtl, win, lose };

public interface Operations
{
    void Cruise();
    void DownBoard();
    void PriestToGetOn();
    void DevilToGetOn();
    void Restart();
}

// we use actionManager to get control of actions
public class SSDirector : System.Object, Operations
{
    private static SSDirector sd;
    private Model model;
    public Controller controller;
    public RealActionManager actionManager;
    public Referee referee;
    public State state = State.r;

    void Start ()
    {

    }

    public static SSDirector getInstance()
    {
        if (sd == null)
            sd = new SSDirector();
        return sd;
    }

    public void setRealActionManager(RealActionManager actionManager2)
    {
        if (actionManager == null)
            actionManager = actionManager2;
    }

    public RealActionManager getRealActionManager()
    {
        return actionManager;
    }

    public Model getModel()
    {
        return model;
    }

    public void setModel(Model model2)
    {
        if (model == null)
            model = model2;
    }

    public void Cruise()
    {
        model.Cruise();
    }

    public void DownBoard()
    {
        model.DownBoard();
    }

    public void PriestToGetOn()
    {
        model.PriestToGetOn();
    }

    public void DevilToGetOn()
    {
        model.DevilToGetOn();
    }

    public void Restart()
    {
        model.Restart();
    }
}

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SSDirector sd = SSDirector.getInstance();
    }

    // Update is called once per frame
    void Update()
    {

    }
}


