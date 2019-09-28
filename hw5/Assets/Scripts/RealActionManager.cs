using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { running, finished };

// callback information interface
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.finished,
        int intParam = 0, string strParam = null, Object objectParam = null);
}

// base class for actions
public class SSAction : ScriptableObject
{
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }

    // assure to be implemented
    public virtual void Start()
    {
        Debug.Log("Virtual SSAction Start Error");
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        Debug.Log("Virtual SSAction Update Error");
        throw new System.NotImplementedException();
    }
}

// specific action class - move
public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    // specific action
    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    // c# need to declare override
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (SSDirector.getInstance().getController().state == State.STOP)
            return;
        if (this.gameObject == null)
            return;
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, target, 10f * Time.deltaTime);
        // finish action, finish this SSAction object  & return callback information
        if (this.gameObject.transform.position == target)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
}

// specific action class - position
public class CCPositionToAction : SSAction
{
    public Vector3 position;

    // specific action
    public static CCPositionToAction GetSSAction(Vector3 position)
    {
        CCPositionToAction action = ScriptableObject.CreateInstance<CCPositionToAction>();
        action.position = position;
        return action;
    }

    // c# need to declare override
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        this.gameObject.transform.position = position;
        // finish action, finish this SSAction object  & return callback information
        if (this.gameObject.transform.position == position)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
}

// sequence actions class - sequence
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

    // To attach List<SSAction> to Current GameObject(Empty GameObject)
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

    // To act that current SSAction
    public new void Update()
    {
        if (sequence.Count == 0)
            return;
        if (start < sequence.Count)
            sequence[start].Update();
    }

    // start >= sequence.Count means one cycle has finished
    // repeat == 0 means the whole CCSequenceAction has finished / time to destroy & callback
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

// abstract class for ActionManager
public class SSActionManager : MonoBehaviour, ISSActionCallback
{
    public Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    public List<SSAction> waitList = new List<SSAction>();
    public List<int> deleteList = new List<int>();

    protected void Start()
    {

    }

    // auto update & recycle for SSAction
    protected void Update()
    {
        // Add SSActions waiting to be carried out into actions
        foreach (SSAction s in waitList)
            actions[s.GetInstanceID()] = s;
        waitList.Clear();

        // Carry out those enabled
        foreach (KeyValuePair<int, SSAction> pr in actions)
        {
            SSAction s = pr.Value;
            if (s.destroy)
                deleteList.Add(s.GetInstanceID());
            else if (s.enable)
            {
                // Debug.Log(s.gameObject);
                s.Update();
            }
        }

        // Empty the deleteList
        foreach (int key in deleteList)
        {
            SSAction s = actions[key];
            actions.Remove(key);
            Destroy(s);
        }
        deleteList.Clear();
    }

    // Prepare one SSAction ready for running
    // include:
    // Attach real gameObject to SSAction
    // Attach manager to be the one receiving callback information (and manage)
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

//This one is an instance of SSAction
//It peels off from Model.cs & Make itself get control of actions
//By get information from SSAction & give orders to them
public class RealActionManager : SSActionManager
{
    public SSDirector sd;

    // Start is called before the first frame update
    public new void Start()
    {
        sd = SSDirector.getInstance();
        if (sd.getController() == null)
            sd.setController(GetComponent<Controller>() as Controller);
        sd.getController().actionManager = this;
    }

    // several new functions being added 
    // according to those specific requirements
    public void Fly(GameObject disk)
    {
        if (disk == null)
            return;
        // generate points(start + end) + flying direction
        // ensure disk to cross screen to disappear
        int st_x = UnityEngine.Random.Range(-100, -50);
        int st_y = UnityEngine.Random.Range(-40, -30);
        int st_z = UnityEngine.Random.Range(-100, -50);
        // print(st_x.ToString() + " " + st_y.ToString() + " " + st_z.ToString());
        disk.transform.position = new Vector3(st_x, st_y, st_z);
        Vector3 target = new Vector3(-st_x, -st_y, -st_z);

        float speed = 10 * sd.getController().round;
        CCMoveToAction track = CCMoveToAction.GetSSAction(target, speed);
        this.GetReady(disk, track, this);
    }
}

// Add Singleton
public class Singleton2<RealActionManager> : MonoBehaviour where RealActionManager : MonoBehaviour
{
    protected static RealActionManager instance;

    public static RealActionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (RealActionManager)FindObjectOfType(typeof(RealActionManager));
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(RealActionManager) +
                    " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}