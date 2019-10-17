using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsAction : MonoBehaviour, IAction
{
    float force = 3;

    public void Move() { }


    public void Move(float force)
    {
        this.force = force;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.z > 5)
            this.gameObject.tag = "LOST";
    }

    void FixedUpdate()
    {
        if (this.gameObject.GetComponent<Rigidbody>() != null)
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1) * force);
    }
}