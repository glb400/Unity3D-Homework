## 编程项目

视频链接

https://www.bilibili.com/video/av67211208/

### part1 component

1. 挂载对象MountObject

Empty Object

1. 棋盘chessboard

黑白相间的scale(2,0.2,2)cube

![](https://raw.githubusercontent.com/glb400/glb400.github.io/master/_posts/img/2.png)

2. 棋子chess

gray/cyan的scale(1.5,0.4,1.5)cylinder

### part2 behaviour & principle

1. 落子及状态转移

2. 判断是否为终态

3. reset + clean

### part3 OnGUI

最终代码

```
├── TicTacToe (Applications)
    ├── Operations.cs
    |   └──  PutDown (by OnMouseDown)
    |
    ├── UI.cs
    |   └──  ShowGUI (by OnGUI)
    |
    ├── Clean.cs
    |   └── CleanChessboard (by Update)
    |
    ├── Funct.vs
    |   └── Static Functions
    |
    └── Param.cs
        └── Static Variables  
```

部分代码释疑：

方格接棋盘，棋子接方格，因此需要从双层绑定部分开始删

![](https://raw.githubusercontent.com/glb400/glb400.github.io/master/_posts/img/3.png)

```csharp
    void Update()
    {
        if (Param.rst == 0)
            return;

        Param.rst = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetType().ToString() != "UnityEngine.Transform")
                continue;
            for(int j = 0;j < transform.GetChild(i).childCount; j++)
                if (transform.GetChild(i).GetChild(j).name == "Cylinder")
                    Destroy(transform.GetChild(i).GetChild(j).gameObject);
        }
    }
```
