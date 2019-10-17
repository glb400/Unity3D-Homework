using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEvent : MonoBehaviour
{
    void OnTriggerEnter(Collider arrowHead)
    {
        Transform arrow = arrowHead.gameObject.transform.parent;
        if (arrow == null)
            return;
        if (arrow.tag == "Arrow")
        {
            arrowHead.gameObject.SetActive(false);
            arrow.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            arrow.tag = "Hit";
            // get ring from target 
            int score = this.gameObject.GetComponent<ScoreAttribute>().GetScore();
            print(score);
            Singleton<ScoreController>.Instance.AddScore(score);
        }
    }
}
