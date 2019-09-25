using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int start = 0;

    public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    //To attach List<SSAction> to Current GameObject(Empty GameObject)
    public new void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    //To act that current SSAction
    public new void Update()
    {
        if (sequence.Count == 0)
            return;
        if (start < sequence.Count)
            sequence[start].Update();
    }

    //start >= sequence.Count means one cycle has finished
    //repeat == 0 means the whole CCSequenceAction has finished / time to destroy & callback
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.finished, int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destroy = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0)
                repeat--;
            if (repeat == 0)
            {
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
            else
            {
                sequence[start].Start();
            }
        }
    }

    private void OnDestroy()
    {
        
    }
}
