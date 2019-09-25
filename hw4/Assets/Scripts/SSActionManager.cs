using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour, ISSActionCallback
{
    public Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    public List<SSAction> waitList = new List<SSAction>();
    public List<int> deleteList = new List<int>();

    protected void Start()
    {
        
    }

    //auto update & recycle for SSAction
    protected void Update()
    {
        //Add SSActions waiting to be carried out into actions
        foreach (SSAction s in waitList)
            actions[s.GetInstanceID()] = s;
        waitList.Clear();

        //Carry out those enabled
        foreach (KeyValuePair<int, SSAction> pr in actions)
        {
            SSAction s = pr.Value;
            if (s.destroy)
                deleteList.Add(s.GetInstanceID());
            else if (s.enable)
            {
                Debug.Log(s.gameObject);
                s.Update();
            }
        }

        //Empty the deleteList
        foreach (int key in deleteList)
        {
            SSAction s = actions[key];
            actions.Remove(key);
            Destroy(s);
        }
        deleteList.Clear();
    }

    //Prepare one SSAction ready for running
    //include:
    //Attach real gameObject to SSAction
    //Attach manager to be the one receiving callback information (and manage)
    public void GetReady(GameObject gameObject, SSAction action, ISSActionCallback manager)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        action.destroy = false;
        action.enable = true;
        waitList.Add(action);
        action.Start();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.finished, int intParam = 0, string strParam = null, Object objectParam = null)
    {

    }
}
