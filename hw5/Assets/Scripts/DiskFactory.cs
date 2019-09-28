using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    SSDirector sd;
    private List<GameObject> occupied = new List<GameObject>();
    private List<GameObject> available = new List<GameObject>();

    public void reset()
    {
        for (int i = 0;i < occupied.Count; i++)
        {
            Destroy(occupied[i]);
        }
        for (int i = 0; i < available.Count; i++)
        {
            Destroy(available[i]);
        }
        occupied.RemoveAll(f => { return true; });
        available.RemoveAll(f => { return true; });
    }

    public void Start()
    {
        sd = SSDirector.getInstance();
        if (sd.getController() == null)
            sd.setController(GetComponent<Controller>() as Controller);
        sd.getController().diskFactory = this;
    }

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
}

// Add Singleton
public class Singleton1<DiskFactory> : MonoBehaviour where DiskFactory : MonoBehaviour
{
    protected static DiskFactory instance;

    public static DiskFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (DiskFactory)FindObjectOfType(typeof(DiskFactory));
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(DiskFactory) +
                    " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}
