using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { WIN, LOSE, STOP, RUN, RESET };

// Controller divided into SceneController + ActionManager

// SceneController implemented by interface 
public interface ScenceController
{
    void Load();
}

public interface Operations
{
    void Hit();
    void Pause();
    void Resume();
    void Restart();
}

public class SSDirector : System.Object, Operations
{
    public static SSDirector sd;

    // Get model
    private Model model;

    // Get control of Scene + Action from GameObject
    private Controller controller;

    public static SSDirector getInstance()
    {
        if (sd == null)
            sd = new SSDirector();
        return sd;
    }

    public void setController(Controller controller)
    {
        this.controller = controller;
    }

    public Controller getController()
    {
        return this.controller;
    }

    public void setModel(Model model)
    {
        this.model = model;
    }

    public Model getModel()
    {
        return this.model;
    }

    // Get operations from User
    public void Hit()
    {
        model.Hit();
    }

    public void Pause()
    {
        model.Pause();
    }

    IEnumerator CountTime()
    {
        while (controller.restTime >= 0)
        {
            // 类似硬件中断延迟的方式
            // use coroutine so this function executes in a parallel way with other processes
            // so it just counts time
            yield return new WaitForSeconds(1);
            controller.restTime--;
        }
    }

    // opposite to pause
    public void Resume()
    {
        model.Resume();
    }

    public void Restart()
    {
        model.Restart();
    }
}

public class Controller : MonoBehaviour, ScenceController
{
    SSDirector sd;

    // ActionManager implemented by member variable
    public RealActionManager actionManager;

    // DiskFactory aims to produce disks
    // Also solve any event regarding to this affair
    public DiskFactory diskFactory;

    // current round and the num of total rounds
    public int round;
    public int n;
    public int shotten; public int hits;
    public int ruler;

    // current time spent
    public int restTime;
    // current trial left
    public int trial;

    // the disks in Game
    public List<GameObject> disks;

    public int nums;

    // store the one has been shot
    public GameObject shotOne;

    public State state;

    // implementation about SceneController
    public void Load()
    {

    }

    public void reset()
    {
        diskFactory.reset();

        // variable initialize
        round = 1;
        n = 10;

        // produce random ruler by round
        ruler = (int)UnityEngine.Random.Range(1, 10 * round);
        // set limits
        restTime = 60;
        trial = 10;

        // set state
        state = State.STOP;

        // initial disks since no disk shot for now
        //for (int i = 0; i < disks.Count; i++)
        //{
        //    Destroy(disks[i]);
        //}
        // disks.RemoveAll(f => { return true; });
        nums = 0;
        shotten = hits = 0;
        shotOne = null;
    }

    // Start is called before the first frame update
    void Awake()
    {
        // lets ssdirector use this controller
        sd = SSDirector.getInstance();
        sd.setController(this);

        if (sd.getModel() == null)
            sd.setModel(GetComponent<Model>() as Model);
            
        // Assure only one Controller for each sort of affair
        // promote security
        diskFactory = Singleton1<DiskFactory>.Instance;
        actionManager = Singleton2<RealActionManager>.Instance;
        
        // variable initialize
        round = 1;
        n = 10;
        
        // produce random ruler by round
        ruler = (int)UnityEngine.Random.Range(1, 10 * round);

        // set limits
        restTime = 60;
        trial = 10;

        // set state
        state = State.STOP;

        // initial disks since no disk shot for now
        disks = new List<GameObject>();
        nums = 0;
        shotten = hits = 0;
        shotOne = null;

        // gets every thing ready to run
        Load();
    }

    // Carry out a loop every frame
    public void produce(int ruler)
    {
        // There can be more disks but not too much
        if (nums < Mathf.Min(round + 2, ruler * round))
        {
            nums ++;
            // produce
            GameObject disk = diskFactory.produce(ruler);
            // set into Game
            disks.Add(disk);
            // put into Game
            actionManager.Fly(disk);
        }
    }

    // time limitation leads to check
    public void check(int ruler)
    {
        // score check - whether continue
        if (shotOne != null)
        {
            Model model = sd.getModel();
            sd.getModel().score(shotOne);
            diskFactory.recycle(shotOne);
            shotOne = null;
            shotten++; hits++;
        }

        // pass to next round
        // whether scores enough or there left no disk (there cannot be better score)
        if (sd.getModel().getTotal() > round * 100 || Mathf.Min(round + 2, ruler * round) == shotten)
        {
            if (round == n)
            {
                StopAllCoroutines();
                state = State.WIN;
                return;
            }
            restTime = 60;
            nums = 0;
            round++;
            ruler = (int)UnityEngine.Random.Range(1, 10 * round);
            shotten = 0;
            state = State.RUN;
        }

        // state check - whether time to finish
        if (restTime == 0)
        {
            if (sd.getModel().getTotal() < round * 100)
            {
                StopAllCoroutines();
                state = State.LOSE;
            }
            else if (round == n)
            {
                StopAllCoroutines();
                state = State.WIN;
            }
            else
            {
                round++;
                ruler = (int)UnityEngine.Random.Range(1, 10 * round);
                shotten = 0;
                state = State.RUN;
            }
        }
    }

    public void recycle()
    {
        disks = new List<GameObject>();
        // recycle those flied-away ones
        for (int i = 0; i < disks.Count; i ++)
        {
            if (Mathf.Abs(disks[i].transform.position.x) > 80 ||
                Mathf.Abs(disks[i].transform.position.y) > 50 ||
                Mathf.Abs(disks[i].transform.position.z) > 20)
            {
                diskFactory.recycle(disks[i]);
                disks.Remove(disks[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        produce(ruler);
        check(ruler);
        recycle();
    }
}



