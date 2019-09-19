## part2 编程实践

video : https://www.bilibili.com/video/av68198732

阅读以下游戏脚本
>Priests and Devils
>
>Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!

程序需要满足的要求：

+ play the game ( http://www.flash-game.net/game/2535/priests-and-devils.html )
+ 列出游戏中提及的事物（Objects）

```
Priests
Devils
Boat
River
Coast
```

+ 用表格列出玩家动作表（规则表），注意，动作越少越好

| 条件 | 动作 | 结果
| -- | -- | -- |
| 船有人 & 船靠岸 | 船开动 | 船行至对岸
| 右有人 & 船在右 & 未载满 | 右上船 | 船++，右--
| 左有人 & 船在左 & 船未满 | 左上船 | 船++，左-- 
| 船有人 & 船在右 | 右下船 | 右++，船--
| 船有人 & 船在左 | 左下船 | 左++，船--


+ 请将游戏中对象做成预制

![](https://github.com/glb400/glb400.github.io/blob/master/_posts/img/4.png?raw=true)

+ 在 GenGameObjects 中创建 长方形、正方形、球 及其色彩代表游戏中的对象。

+ 使用 C# 集合类型 有效组织对象

此处使用C#中的List<>或Stack<>进行对象组织与管理，这样可以保证Priests和Devils依序走到船上。

+ 整个游戏仅 主摄像机 和 一个 Empty 对象， 其他对象必须代码动态生成！！！ 。 整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的 通讯耦合 语句。 违背本条准则，不给分

+ 请使用课件架构图编程，不接受非 MVC 结构程序

使用MVC程序设计模式，因此将脚本分为Model + View + Controller三部分，又因为对象须由动态生成，因此只需要上述脚本即完成所有功能。

创建如下目录组织：

```
├── Assets
    ├── Materials
    ├── Resources
    |   └──  Prefabs
    ├── Scripts
    └── Textures
```

+ 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！

关键解释：

因为只有controller可以使用model，因此将model封装在namespace models中，controller使用using models确保访问model的安全性。

为了统一位置问题，需要将每一个对象与固定位置对应，也可以动态维护队列位置。后者比较复杂，采用前者，在每一次移动后重新排序：

```csharp
    void DynamicPosition()
    {
        foreach (GameObject priest in PriestsAtLeft)
            priest.transform.position = CoastLeft + new Vector3( - (DevilsAtLeft.Count + PriestsAtLeft.IndexOf(priest)) * 1.3f,0.0f,0.0f);
        foreach (GameObject priest in PriestsAtRight)
            priest.transform.position = CoastRight + new Vector3((DevilsAtRight.Count + PriestsAtRight.IndexOf(priest)) * 1.3f, 0.0f, 0.0f);
        foreach (GameObject devil in DevilsAtLeft)
            devil.transform.position = CoastLeft + new Vector3( - DevilsAtLeft.IndexOf(devil) * 1.3f, 0.0f, 0.0f);
        foreach (GameObject devil in DevilsAtRight)
            devil.transform.position = CoastRight + new Vector3(DevilsAtRight.IndexOf(devil) * 1.3f, 0.0f, 0.0f);
    }

```

```csharp
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

        foreach (GameObject priest in PriestsAtRight)
            priest.transform.position = new Vector3(9.4f + PriestsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f);

        foreach (GameObject priest in DevilsAtRight)
            priest.transform.position = new Vector3(5.5f + DevilsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f);
    }
```

PS：正解为：

魔鬼先把魔鬼送过河，魔鬼再把魔鬼送过河，魔鬼回来下船，两个传教士过河，一个传教士下船，一个魔鬼上船，一个魔鬼下船，两个传教士过河，两个传教士下船，魔鬼上船去接另外两个魔鬼。

![](https://github.com/glb400/glb400.github.io/blob/master/_posts/img/5.png?raw=true)
