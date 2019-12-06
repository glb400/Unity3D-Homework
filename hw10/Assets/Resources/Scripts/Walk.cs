using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walk : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // response to click event
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                // forward to position
                Vector3 point = hit.point;
                Debug.Log(point);
                transform.LookAt(new Vector3(point.x, point.y, point.z));
                agent.SetDestination(point);
            }
        }

        // animation controller
        if (agent.remainingDistance < 0.5f)
        {
            anim.Play("idle");
        }
        else
        {
            anim.Play("run");
        }
    }
}
