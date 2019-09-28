using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    SSDirector sd;

    private float total;

    public float getTotal()
    {
        return total;
    }

    public void reset()
    {
        total = 0.0f;
    }
    // Score depends on color + size + speed
    public float score(GameObject disk)
    {  
        float shot = 0;
        // scores from size + speed
        shot += (sd.getController().n - disk.GetComponent<DiskAttribute>().size + 1) * disk.GetComponent<DiskAttribute>().speed;
        // scores from color rgb value
        shot += (int)(disk.GetComponent<DiskAttribute>().color.r + disk.GetComponent<DiskAttribute>().color.g + disk.GetComponent<DiskAttribute>().color.b) >> 4;
        total += shot;
        return shot;
    }

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

    // Start is called before the first frame update
    void Start()
    {
        sd = SSDirector.getInstance();
        if (sd.getController() == null)
            sd.setController(GetComponent<Controller>() as Controller);
        sd.setModel(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
