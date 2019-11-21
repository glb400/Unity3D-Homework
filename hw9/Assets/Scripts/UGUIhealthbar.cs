using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIhealthbar : MonoBehaviour
{
    // slider
    public Slider healthSlider;

    public float health = 0.0f;
    private float res = 0.0f;

    private Rect healthbar = new Rect(50, 50, 200, 10);
    private Rect add = new Rect(80, 100, 50, 20);
    private Rect decrease = new Rect(150, 100, 70, 20);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(add, "add"))
            res = res + 0.1f > 1.0f ? 1.0f : res + 0.1f;
        if (GUI.Button(decrease, "decrease"))
            res = res - 0.1f < 0.0f ? 0.0f : res - 0.1f;
        health = Mathf.Lerp(health, res, 0.05f);
        GUI.HorizontalScrollbar(healthbar, 0.0f, health, 0.0f, 1.0f);
        healthSlider.value = health;
    }
}
