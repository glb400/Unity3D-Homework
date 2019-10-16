using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void Begin();
    void ReStart();
    void hit();
}

public class View : MonoBehaviour
{
    private IUserAction action;
    void Start()
    {
        action = SSDirector.getInstance().currentScenceController as IUserAction;
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 120, 40), "Start"))
        {
            action.Begin();
        }
        if (GUI.Button(new Rect(20, 80, 120, 40), "Restart"))
        {
            action.ReStart();
        }
        if (Input.GetMouseButtonDown(0))
        {
            action.hit();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
