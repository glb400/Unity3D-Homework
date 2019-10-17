using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsEmit : SSAction
{
    bool enableEmit = true;
    Vector3 force;
    float st;
    public SceneController sceneControler = (SceneController)SSDirector.getInstance().currentScenceController;
    public override void Start () {
        st = 6 - Random.value * 12;
        this.transform.position = new Vector3(st, 0, 0);
        force = new Vector3(6 * Random.Range(-1, 1), 6 * Random.Range(0.5f, 2), 13 + 2 * sceneControler.round);
    }
    public static physicsEmit GetSSAction()
    {
        physicsEmit action = ScriptableObject.CreateInstance<physicsEmit>();
        return action;
    }

    public override void Update()
    {
        
    }
    public void Destory()
    {
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
    // Update is called once per frame
    public override void FixedUpdate () {
        if(!this.destroy)
        {
            if(enableEmit)
            {
                // In Physics Emitter
                // Get Rigidbody to add physics move
                gameobject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameobject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
                enableEmit = false;
            }
        }
	}
}
