using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCPositionToAction : SSAction
{
    public Vector3 position;

    //specific action
    public static CCPositionToAction GetSSAction(Vector3 position)
    {
        CCPositionToAction action = ScriptableObject.CreateInstance<CCPositionToAction>();
        action.position = position;
        return action;
    }

    //c# need to declare override
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        this.gameObject.transform.position = position;
        //finish action, finish this SSAction object  & return callback information
        if (this.gameObject.transform.position == position)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
}
