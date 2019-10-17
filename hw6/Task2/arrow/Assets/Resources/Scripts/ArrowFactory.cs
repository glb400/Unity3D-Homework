using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowFactory : MonoBehaviour
{
    private List<GameObject> used = new List<GameObject>();
    private List<GameObject> free = new List<GameObject>();

    public GameObject GetArrow()
    {
        GameObject newArrow;
        if (free.Count > 0)
        {
            newArrow = free[0];
            free.Remove(free[0]);
        }
        else
            newArrow = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Arrow"));
        used.Add(newArrow);
        newArrow.name = newArrow.GetInstanceID().ToString();
        newArrow.AddComponent<physicsAction>();
        newArrow.AddComponent<CCAction>();
        newArrow.SetActive(true);
        return newArrow;
    }

    public void FreeArrow(GameObject arrow)
    {
        if (arrow != null)
        {
            used.Remove(arrow);
            Destroy(arrow);
        }
    }

}