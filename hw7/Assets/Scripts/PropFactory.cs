using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropFactory : MonoBehaviour
{
    private GameObject patrol = null;                              
    private List<GameObject> used = new List<GameObject>();        
    private Vector3[] vec = new Vector3[9];                        

    public FirstSceneController sceneControler;                   

    public List<GameObject> GetPatrols()
    {
        int[] pos_x = { -7, 2, 13 };
        int[] pos_z = { -11, -2, 7 };
        int index = 0;
        for(int i=0;i < 3;i++)
            for(int j=0;j < 3;j++)
                vec[index++] = new Vector3(pos_x[i], 0, pos_z[j]);
        for(int i=0; i < 9; i++)
        {
            patrol = Instantiate(Resources.Load<GameObject>("Prefabs/Patrol"));
            patrol.transform.position = vec[i];
            patrol.GetComponent<PatrolData>().sign = i + 1;
            patrol.GetComponent<PatrolData>().start_position = vec[i];
            used.Add(patrol);
        }   
        return used;
    }

    public void StopPatrol()
    {
        for (int i = 0; i < used.Count; i++)
            used[i].gameObject.GetComponent<Animator>().SetBool("run", false);
    }
}
