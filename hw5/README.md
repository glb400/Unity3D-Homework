## 编写一个简单的鼠标打飞碟（Hit UFO）游戏

video : https://www.bilibili.com/video/av69366111

+ 游戏内容要求：
    1. 游戏有 n 个 round，每个 round 都包括10 次 trial；
    2. 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
    3. 每个 trial 的飞碟有随机性，总体难度随 round 上升；
    4. 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。

+ 游戏的要求：
    + 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
    + 尽可能使用前面 MVC 结构实现人机交互与游戏模型分离
如果你的使用工厂有疑问，参考：弹药和敌人：减少，重用和再利用


首先根据游戏要求设计游戏架构，重点在于设计单实例飞碟工厂类并实现MVC。关于前者，具体实现可以参考前述代码；关于后者，MVC架构的Model部分用于组织数据，Controller部分用于控制Model，在此处分为动作管理与场景管理，其中根据前面实现，场景管理使用接口实现，动作管理使用成员变量SSActionManager。

关于unity输入，使用光标拾取函数消除被点击的飞碟。DiskFactory类采用actionManager类中使用的队列针对不同状态的飞碟进行分类处理。

---

### 实现

根据一般游戏的特性，实现时间限制(restTime) + 失败次数限制(trial)同时控制，时间限制指向check()函数决定 enter next round / end / finish，失败次数限制决定是否 fail 从而无需check()直接结束。

具体而言，计时系统采用协程，当StartCoroutine刚调用的时候，可以理解为正常的函数调用，然后接着看调用的函数里面。当被调用函数执行到yield return null；（暂停协程，等待下一帧继续执行）时，根据Unity解释协同程序先返回开始协程的地方，然后再暂停协程。可以理解为yield处执行并行，配合计时函数WaitForSeconds()可精确完成计时。

```
├──> user input
		├──> respond from `controller` leading to `model` change   ──> `view` change
		├──> state change from `controller` ──> `view` change
	   	└── Restart(chosen)
```

使用向GameObject添加属性的方法构造Disk，因为Unity中对于非GameObject类操作有限，关于对象间操作均在GameObject中。创建GameObject类Disk并使用添加Component的方式为Disk类添加属性类DiskAttribute，但是需要注意Component是不能够随意初始化的，因此需要对其增加reset函数，实际上GameObject类中成员变量都属于其Component，因此需要加入reset()。


具体实现时针对disk出现位置的问题，使用对称点方法，从(x, y, z)走到(-x, -y, -z)，那么对于在screen外两点一定是穿屏而过。关于pause时动作暂停，根据代码回溯到动作类CCMoveToAction，在其update时根据进行状态`SSDirector.getInstance().getController().state == State.STOP`判断决定是否需要暂停动作更新。

部分代码如下，解释见注释
```csharp
//user actions

// specific actions for user in this model
// Get operations from User
public void Hit()
{
	// 使用光标拾取
	if (Input.GetMouseButtonDown(0) && (sd.getController().state == State.RUN))
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (sd.getController().state == State.RUN)
			{
				// we click on Cylinder by which leads to its parent gameobject - Disk
				sd.getController().shotOne = hit.transform.gameObject.transform.parent.gameObject;
			}
		}
	}
}

public void Pause()
{
	sd.getController().state = State.STOP;
	sd.getController().StopAllCoroutines();
	foreach (GameObject disk in sd.getController().disks)
	{
		disk.SetActive(false);
	}
}

IEnumerator CountTime()
{
	while (sd.getController().restTime >= 0)
	{
		// 类似硬件中断延迟的方式
		// use coroutine so this function executes in a parallel way with other processes
		// so it just counts time
		yield return new WaitForSeconds(1);
		sd.getController().restTime--;
	}
}

// opposite to pause
public void Resume()
{
	sd.getController().state = State.RUN;
	sd.getController().StartCoroutine(CountTime());
	foreach (GameObject disk in sd.getController().disks)
	{
		disk.SetActive(true);
	}
}

public void Restart()
{
	sd.getController().StopAllCoroutines();
	sd.getController().reset();
	sd.getController().state = State.RUN;
	sd.getModel().reset();
	sd.getController().StartCoroutine(CountTime());
}
```

```csharp
// diskFactory
public GameObject produce(int ruler)
{
	GameObject disk;
	// do not produce when game is over
	if (sd.getController().state == State.WIN)
		return null;
	if (available.Count > 0)
	{
		disk = available[0];
		available.Remove(available[0]);
	}
	else
	{
		// for a brand-new disk having to set attributes
		disk = Instantiate(Resources.Load("Prefabs/Disk") as GameObject);
		disk.AddComponent<DiskAttribute>();

		// according to requests we need to carry out by 'ruler'
		// As time goes by it becomes more difficult
		disk.GetComponent<DiskAttribute>().size = UnityEngine.Random.Range(0.5f, 3.0f);
		disk.GetComponent<DiskAttribute>().color = new Color((int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)), (int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)), (int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)), (int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)));
		disk.GetComponent<DiskAttribute>().speed = 10 * sd.getController().round;

		// set attribute for disk
		disk.transform.localScale = new Vector3(disk.GetComponent<DiskAttribute>().size, disk.GetComponent<DiskAttribute>().size, disk.GetComponent<DiskAttribute>().size);
		disk.GetComponent<Renderer>().material.color = disk.GetComponent<DiskAttribute>().color;
		// since disk composes by several components each need to be colored
		for (int i = 0;i < disk.transform.childCount; i ++)
		{
			disk.transform.GetChild(i).GetComponent<Renderer>().material.color = new Color((int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)), (int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)), (int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)), (int)UnityEngine.Random.Range(0, Mathf.Min(ruler << 1, 255)));
		}
	}
	disk.SetActive(true);
	occupied.Add(disk);
	return disk;
}

public void recycle(GameObject disk)
{
	for (int i = 0; i < occupied.Count; i++)
	{
		if (occupied[i] == disk)
		{
			occupied[i].SetActive(false);
			occupied.Remove(occupied[i]);
			available.Add(disk);
		}
	}
}
```

```csharp
// actionManager
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
```

result:

![](https://github.com/glb400/glb400.github.io/blob/master/_posts/img/8.png?raw=true)
