using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { running , finished };

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.finished,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
