using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    //specific action
    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    //c# need to declare override
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, target, 10f * Time.deltaTime);
        //finish action, finish this SSAction object  & return callback information
        if (this.gameObject.transform.position == target)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
}
