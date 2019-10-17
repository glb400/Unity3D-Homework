using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    private static DiskFactory _instance;
    public SceneController sceneControler;
    public Recorder scoreRecorder;
    DiskAttribute diskAttr;
    public List<GameObject> occupied;
    public List<GameObject> available;
    public Color[] colors = { Color.black, Color.red, Color.green, Color.blue };

   
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = Singleton<DiskFactory>.Instance;
            _instance.occupied = new List<GameObject>();
            _instance.available = new List<GameObject>();
        }
    }
    public void Start()
    {
        sceneControler = (SceneController)SSDirector.getInstance().currentScenceController;
        sceneControler.factory = this;
        scoreRecorder = sceneControler.scoreRecorder;
    }

    public void tryGetOne(int round)
    {
        if (sceneControler.num == 30)
            if (scoreRecorder.Score >= round * 10)
            {
                sceneControler.round++;
                sceneControler.num = 0;
            }
            else if (scoreRecorder.Score < round * 10)
                sceneControler.state = State.LOSE;
    }

    public GameObject getOne()
    {
        GameObject disk;
        if (available.Count == 0)
            disk = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/disk"), new Vector3(40, 0, 0), Quaternion.identity) as GameObject;
        else
        {
            disk = available[0];
            available.Remove(available[0]);
        }
        return disk;
    }

    public void setUp(GameObject disk, int round)
    {
        diskAttr = disk.GetComponent<DiskAttribute>();
        diskAttr.size = (float)1.0 / round;
        diskAttr.color = colors[round];
        disk.transform.localScale = new Vector3(diskAttr.size, diskAttr.size, diskAttr.size);
        disk.GetComponent<Renderer>().material.color = diskAttr.color;
    }

    public GameObject getDisk(int round)
    {
        // determine the state first
        tryGetOne(round);
        // get one disk when we can
        GameObject disk = getOne();
        // set up a new disk
        setUp(disk, round);
        occupied.Add(disk);
        return disk;
    }

    public void freeDisk(GameObject disk1)
    {
        for (int i = 0; i < occupied.Count; i++)
        {
            if (occupied[i] == disk1)
            {
                occupied.Remove(disk1);
                disk1.SetActive(true);
                available.Add(disk1);
            }
        }
        return;
    }
}
