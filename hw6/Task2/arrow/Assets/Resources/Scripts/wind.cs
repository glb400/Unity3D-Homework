using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    void OnTriggerEnter(Collider arrowHead)
    {
        Transform arrow = arrowHead.gameObject.transform.parent;
        if (arrow == null)
            return;
        if (arrow.tag == "Arrow")
            arrow.GetComponent<CCAction>().Move();
    }
}
