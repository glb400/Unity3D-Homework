using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This one is an instance of SSAction
//It peels off from Model.cs & Make itself get control of actions
//By get information from SSAction & give orders to them
public class RealActionManager : SSActionManager
{
    public SSDirector sd;

    public CCMoveToAction mov;
    public CCPositionToAction pos;

    // Start is called before the first frame update
    public new void Start()
    {
        sd = SSDirector.getInstance();
        sd.actionManager = this;
    }

    public void movAction(GameObject boat, Vector3 target, float speed)
    {
        mov = CCMoveToAction.GetSSAction(target, speed);
        this.GetReady(boat, mov, this);
    }

    public void posAction(GameObject boat, Vector3 position)
    {
        // boat.transform.position = position;
        pos = CCPositionToAction.GetSSAction(position);
        this.GetReady(boat, pos, this);
    }

}
