using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clean : MonoBehaviour
{
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
}
