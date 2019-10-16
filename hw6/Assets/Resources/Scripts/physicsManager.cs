using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsManager : SSActionManager, ISSActionCallback, IActionManager
{
    public SceneController sceneController;
    public DiskFactory diskFactory;
    public Recorder scoreRecorder;
    public physicsEmit EmitDisk;
    public GameObject Disk;
    int count = 0;
    // Use this for initialization
    protected void Start()
    {
        sceneController = (SceneController)SSDirector.getInstance().currentScenceController;
        diskFactory = sceneController.factory;
        scoreRecorder = sceneController.scoreRecorder;
        sceneController.actionManager = this;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (sceneController.round <= 3 && sceneController.state == State.RUNNING)
        {
            count++;
            if (count == 60)
            {
                playDisk();
                sceneController.num++;
                print(sceneController.num);
                count = 0;
            }
            base.Update();
        }
    }

    public void playDisk()
    {
        EmitDisk = physicsEmit.GetSSAction();
        Disk = diskFactory.getDisk(sceneController.round);
        this.RunAction(Disk, EmitDisk, this);
        Disk.GetComponent<DiskAttribute>().action = EmitDisk;
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        if (!source.gameobject.GetComponent<DiskAttribute>().hit)
            scoreRecorder.miss();
        diskFactory.freeDisk(source.gameobject);
        source.gameobject.GetComponent<DiskAttribute>().hit = false;
    }
}
