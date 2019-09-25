## 牧师与魔鬼 动作分离版

video : https://www.bilibili.com/video/av68971655

【2019新要求】：设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束

本次编程建立在上一次作业基础上，增加动作管理器用来管理相关动作。

具体实现方式为将执行动作部分与view部分进行分离，view接收的信息传递给动作管理器，相当于在model部分将其与其余部分剥离；关于动作管理器部分的实现，设计抽象类SSAction作为动作基类并设计动作管理器类SSActionManager。

关于SSAction类的具体执行流程为`inform (by parameters) -> execute -> destory -> callback`，关于callback部分来自于消息接口ISSActionCallback，在这里可以自定义返回信息。

动作基类SSAction为抽象类，对于所有动作采用继承自SSAction类的CCMoveToAction和CCSequenceAction，单一动作使用前者顺序组合使用后者。对于事件接口ISSActionCallback无需修改，只需回传信息，即SSActionManager实现ISSActionCallback后信息由SSAction流向SSActionManager。

最后设计本项目专用的动作管理器SSActionManager，用来进行具体的动作管理。然后Controller中调用model的行为部分转交给SSActionManager。

关于裁判类，根据我们的代码设计其类似于SSActionManager，也是从model中进行功能剥离，所以类似于构造SSActionManager即可。

动作管理器只负责model中专门的动作部分，像Cruise这样改变状态的函数是不予负责的，只在状态修改引起的动作部分负责。

原来model具体实现所有功能，现在很多部分只需要将动作类交给controller的actionManager，因此在director中为model + controller.actionManager共同控制GameObject行为，通过参数传递实现同步。

挂载SSActionManager
修改部分：

```csharp
//在SSDirector类中加入动作管理器actionManager

//在model初始化时需要检验actionManager == null
//因为model在行为部分会调用actionManager
public void Start()
{
    sd = SSDirector.getInstance();
    sd.setModel(this);
    if (sd.actionManager == null)
        sd.actionManager = gameObject.AddComponent<RealActionManager>() as RealActionManager;   
    Render();
}

//如下是model中的行为部分
void DynamicPosition()
{
    foreach (GameObject priest in PriestsAtLeft)
        sd.actionManager.posAction(priest, CoastLeft + new Vector3(-(DevilsAtLeft.Count + PriestsAtLeft.IndexOf(priest)) * 1.3f, 0.0f, 0.0f));
    foreach (GameObject priest in PriestsAtRight)
        sd.actionManager.posAction(priest, CoastRight + new Vector3((DevilsAtRight.Count + PriestsAtRight.IndexOf(priest)) * 1.3f, 0.0f, 0.0f));
    foreach (GameObject devil in DevilsAtLeft)
        sd.actionManager.posAction(devil, CoastLeft + new Vector3(-DevilsAtLeft.IndexOf(devil) * 1.3f, 0.0f, 0.0f));
    foreach (GameObject devil in DevilsAtRight)
        sd.actionManager.posAction(devil, CoastRight + new Vector3(DevilsAtRight.IndexOf(devil) * 1.3f, 0.0f, 0.0f));
}

void GetOn(GameObject passenger)
{
    passenger.transform.parent = BoatInstance.transform;
    DetectBoat(out IsAvailable, out VacantPos, out IsEmpty);
    if (IsAvailable)
    {
        int flag = sd.state == State.r ? 1 : -1;
        //overrided
        sd.actionManager.posAction(passenger, new Vector3(flag * (0.5f + (VacantPos + 1) * 2.0f), 0.8f, 0.0f));
        // passenger.transform.position = new Vector3(flag * (0.5f + (VacantPos + 1) * 2.0f), 0.8f, 0.0f);
        BodyOnBoat.Add(passenger);
    }
}

public void Restart()
{
    sd.state = State.r;
    DownBoard();
    BoatInstance.transform.position = BoatRight;

    for (int i = 0;i < PriestsAtLeft.Count; i ++)
    {
        PriestsAtRight.Add(PriestsAtLeft[i]);
    }

    PriestsAtLeft.RemoveAll(f => { return true; });

    for (int i = 0; i < DevilsAtLeft.Count; i++)
    {
        DevilsAtRight.Add(DevilsAtLeft[i]);
    }

    DevilsAtLeft.RemoveAll(f => { return true; });

    //overrided
    foreach (GameObject priest in PriestsAtRight)
        sd.actionManager.posAction(priest, new Vector3(9.4f + PriestsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f));
    // priest.transform.position = new Vector3(9.4f + PriestsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f);

    //overrided
    foreach (GameObject priest in DevilsAtRight)
        sd.actionManager.posAction(priest, new Vector3(5.5f + DevilsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f));
    // priest.transform.position = new Vector3(5.5f + DevilsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f);
}



public void Update()
{
    switch (sd.state)
    {
        case State.ltr:
                // overrided
                sd.actionManager.movAction(BoatInstance, BoatRight, Time.deltaTime * speed);
                // BoatInstance.transform.position = Vector3.MoveTowards(BoatInstance.transform.position, BoatRight, Time.deltaTime * speed);
                if (BoatInstance.transform.position == BoatRight)
                {
                    sd.state = State.r;
                }
                break;
        case State.rtl:
                //overrided
                sd.actionManager.movAction(BoatInstance, BoatLeft, Time.deltaTime * speed);
                // BoatInstance.transform.position = Vector3.MoveTowards(BoatInstance.transform.position, BoatLeft, Time.deltaTime * speed);
                if (BoatInstance.transform.position == BoatLeft)
                {
                    sd.state = State.l;
                }
                break;
        default:
                Check();
                break;
    }
}
```

最后增加与actionManager类似的referee类：

```csharp
//SSDirector
private static SSDirector sd;
private Model model;
public Controller controller;
public RealActionManager actionManager;
public Referee referee;

//model.start
public void Start()
{
    sd = SSDirector.getInstance();
    sd.setModel(this);
    if (sd.actionManager == null)
        sd.actionManager = gameObject.AddComponent<RealActionManager>() as RealActionManager;
    if (sd.referee == null)
        sd.referee = gameObject.AddComponent<Referee>() as Referee;
    Render();
}

//Referee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referee : MonoBehaviour
{
    public SSDirector sd;

    public void Check(List<GameObject> PriestsAtRight, List<GameObject> DevilsAtRight, List<GameObject> PriestsAtLeft, List<GameObject> DevilsAtLeft, List<GameObject> BodyOnBoat)
    {
        if (PriestsAtLeft.Count == 3 && DevilsAtLeft.Count == 3)
        {
            sd.state = State.win;
            return;
        }

        int BoatPriest = 0;
        int BoatDevil = 0;

        foreach (GameObject body in BodyOnBoat)
            if (body.name[0] == 'P')
                BoatPriest++;
            else if (body.name[0] == 'D')
                BoatDevil++;

        switch (sd.state)
        {
            case State.l:
                if (((PriestsAtLeft.Count + BoatPriest) != 0 && ((PriestsAtLeft.Count + BoatPriest) < (DevilsAtLeft.Count + BoatDevil))) | (PriestsAtRight.Count != 0 && PriestsAtRight.Count < DevilsAtRight.Count))
                    sd.state = State.lose;
                break;
            case State.r:
                if (((PriestsAtRight.Count + BoatPriest) != 0 && ((PriestsAtRight.Count + BoatPriest) < (DevilsAtRight.Count + BoatDevil))) | (PriestsAtLeft.Count != 0 && PriestsAtLeft.Count < DevilsAtLeft.Count))
                    sd.state = State.lose;
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sd = SSDirector.getInstance();
        sd.referee = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


//model.Check
    default:
        // Check();
        sd.referee.Check(PriestsAtRight, DevilsAtRight, PriestsAtLeft, DevilsAtLeft, BodyOnBoat);
        break;
```
