using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour
{

    public float speedX = 1.0f;
    public float speedY = 1.0f;
    bool flag = true;
    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            if (Input.GetButton("Jump"))
            {
                this.gameObject.AddComponent<Rigidbody>();
                this.gameObject.GetComponent<Rigidbody>().useGravity = false;
                flag = false;
            }
        }
    }
}
