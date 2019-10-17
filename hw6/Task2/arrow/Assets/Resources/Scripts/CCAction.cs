using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAction : MonoBehaviour, IAction
{
    private float time = 10;
    float magnet = 0;
    float radius = 0.01f;
    Vector3 initPosition;

    public void Move()
    {
        Move(Singleton<WindController>.Instance.GetDirection(), Singleton<WindController>.Instance.GetStrength());
    }

    public void Move(Vector3 direction, float strength)
    {
        this.transform.position += direction * strength;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            if (this.gameObject.tag == "Hit")
            {
                time -= Time.deltaTime;
                magnet += 0.5f;
                float dy = Mathf.Cos(magnet) * radius;
                transform.position = initPosition + new Vector3(0, dy, 0);
            }
            else
                initPosition = transform.position;
        }
        if (time <= 0)
        {
            this.gameObject.tag = "LOST";
            transform.position = initPosition;
        }
    }
}