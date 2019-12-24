using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class vb : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject btn;
    public Animator anime;
    void IVirtualButtonEventHandler.OnButtonPressed(VirtualButtonBehaviour vbb) {
        anime.SetTrigger("Jump");
        Debug.Log("Jump");
    }
    void IVirtualButtonEventHandler.OnButtonReleased(VirtualButtonBehaviour vbb){
        
    }
    // Start is called before the first frame update
    void Start()
    {
        VirtualButtonBehaviour vbb = btn.GetComponent<VirtualButtonBehaviour>();
        if (vbb)
        {
            vbb.RegisterEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
