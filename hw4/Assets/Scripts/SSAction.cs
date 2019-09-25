using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject
{
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }

    //assure to be implemented
    public virtual void Start()
    {
        Debug.Log("Virtual SSAction Start Error");
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        Debug.Log("Virtual SSAction Update Error");
        throw new System.NotImplementedException();
    }
}
