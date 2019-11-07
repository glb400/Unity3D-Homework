using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Halo : MonoBehaviour
{
    public ParticleSystem ps;
    public Camera camera;
    public int num = 10000;
    public float innerRadius = 8.0f;
    public float outerRadius = 10.0f;
    public bool isShrink = false;

    private ParticleSystem.Particle[] pts;
    // data
    private float[] angles;
    private float[] sizes;
    private float[] sizeBeforeMouseOn;
    private float[] sizeAfterMouseOn;
    // attribute
    public int speedLevel = 5;
    public float circleSpeed = 0.1f;
    public float shrinkSpeed = 2.0f;
    // camera
    private Ray ray;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        // init
        pts = new ParticleSystem.Particle[num];
        angles = new float[num];
        sizes = new float[num];
        sizeBeforeMouseOn = new float[num];
        sizeAfterMouseOn = new float[num];

        // set up
        // main module of particlesystem manage its attribute
        var main = ps.main;
        main.maxParticles = num;

        ps.Emit(num);
        ps.GetParticles(pts);

        // set attribute means finish of init
        for (int i = 0; i < num; i ++)
        {

            float radius = Random.Range(0, 3) > 1 ? Random.Range(innerRadius + (outerRadius - innerRadius) / 4, outerRadius - (outerRadius - innerRadius) / 4) : Random.Range(innerRadius, outerRadius);
            float angle = Random.Range(0.0f, 360.0f);
            sizes[i] = radius;
            angles[i] = angle;
            sizeBeforeMouseOn[i] = radius;
            sizeAfterMouseOn[i] = 0.8f * radius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check state first
        for (int i = 0; i < num; i++)
        {
            if (isShrink)
            {
                if (sizes[i] > sizeAfterMouseOn[i])
                {
                    // uniform change
                    sizes[i] -= shrinkSpeed * (sizes[i] / sizeAfterMouseOn[i]) * Time.deltaTime;
                }    
            }
            else
            {
                if (sizes[i] < sizeBeforeMouseOn[i])
                {
                    sizes[i] += shrinkSpeed * (sizeBeforeMouseOn[i] / sizes[i]) * Time.deltaTime;
                }
                else if (sizes[i] > sizeBeforeMouseOn[i])
                {
                    sizes[i] = sizeBeforeMouseOn[i];
                }
            }

            // random clockwise
            bool random = Random.Range(-1, 1) > 0 ? true : false;
            if (random)
            {
                angles[i] += (i % speedLevel + 1) * circleSpeed;
            }
            else
            {
                angles[i] -= (i % speedLevel + 1) * circleSpeed;
            }

            // implement change
            float theta = (angles[i] % 360.0f) / 180 * Mathf.PI;
            pts[i].position = new Vector3(sizes[i] * Mathf.Cos(theta), sizes[i] * Mathf.Sin(theta), 0.0f);
        }
        ps.SetParticles(pts, pts.Length);

        // then check click event
        ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "BoxColider")
                isShrink = true;
        }
        else
            isShrink = false;
    }
}
