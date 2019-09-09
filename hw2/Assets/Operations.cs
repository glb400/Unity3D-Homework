using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operations : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("This OnMouseDown!");

        GameObject Mount = GameObject.Find("MountObject");
        Mount.SetActive(true);
        Component[] cps = Mount.GetComponents<Component>();

        if (null != Mount)
        {
            for (int i = 0; i < cps.Length; i++)
            {
                if (cps[i].ToString() == "MountObject (Func)" && Func.IsFinish())
                    return;
            }
        }

        string grid = this.name;
        GameObject chess = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        chess.transform.localScale = new Vector3(1.5f, 0.4f, 1.5f);
        chess.transform.Rotate(new Vector3(-90, 0, 0));
        chess.transform.position = this.transform.position;
        chess.transform.parent = this.transform;

        for (int i = 0; i < cps.Length; i++)
        {
            if (cps[i].ToString() == "MountObject (Param)")
            {
                if (Param.board[(grid[4] - '1') % 3, (grid[4] - '1') / 3] != 0)
                    return;

                Param.board[(grid[4] - '1') % 3, (grid[4] - '1') / 3] = Param.turn;
                Param.cnt ++;
                if (Param.turn == 1)
                    chess.GetComponent<MeshRenderer>().material.color = Color.cyan;
                else
                    chess.GetComponent<MeshRenderer>().material.color = Color.grey;
            }
        }
    }
}
