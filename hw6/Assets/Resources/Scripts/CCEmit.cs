using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCEmit : SSAction
{
    public SceneController sceneControler = (SceneController)SSDirector.getInstance().currentScenceController;
    public GameObject target; 
    public float speed;   
    private float dist;  
    float st;
    float targetX;
    float targetY;

    public override void Start()
    {
        speed = sceneControler.round * 5;
        gameobject.GetComponent<DiskAttribute>().speed = speed;
        st =  - Random.value * 20;
        targetX = Random.value > 0.5 ? 40 : -40;
        targetY = Random.value * 25;
        this.transform.position = new Vector3(st, 0, 0);
        target = new GameObject();
        target.transform.position = new Vector3(targetX, targetY, 30);
        dist = Vector3.Distance(this.transform.position, target.transform.position);
    }
    public static CCEmit GetSSAction()
    {
        return ScriptableObject.CreateInstance<CCEmit>();
    }
    public override void FixedUpdate()
    {
        
    }
    public override void Update()
    {
        Vector3 targetPos = target.transform.position;
        gameobject.transform.LookAt(targetPos);
        gameobject.transform.rotation = gameobject.transform.rotation * Quaternion.Euler(Mathf.Clamp(-Mathf.Min(1, Vector3.Distance(gameobject.transform.position, targetPos) / dist) * 45, -42, 42), 0, 0);
        gameobject.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, Vector3.Distance(gameobject.transform.position, target.transform.position)));
        if (this.transform.position == target.transform.position)
        {
            Destroy(target);
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
}
