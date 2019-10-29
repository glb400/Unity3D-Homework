using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    public GameObject follow;           
    public float smothing = 5f;          
    Vector3 offset;                      

    void Start()
    {
        offset = transform.position - follow.transform.position;
    }

    void FixedUpdate()
    {
        Vector3 offsetHeight = new Vector3(0.0f, 20.0f, -10.0f);
        Vector3 target = follow.transform.position + offset + offsetHeight;
        transform.position = Vector3.Lerp(transform.position, target, smothing * Time.deltaTime);
    }
}
