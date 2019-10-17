using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    private float strength = 0.5f;
    private Vector3 direction = new Vector3(0.01f, 0, 0);

    // Update is called once per frame
    void Update()
    {
        strength = UnityEngine.Random.Range(5.0f, 20.0f);
        direction = new Vector3(UnityEngine.Random.Range(-0.01f, 0.01f), UnityEngine.Random.Range(-0.01f, 0.01f), 0);
    }

    public float GetStrength()
    {
        return strength;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }
}
