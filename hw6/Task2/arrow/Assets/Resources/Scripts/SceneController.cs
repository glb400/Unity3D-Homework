using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour, ISceneController
{
    public GameObject target;
    public Queue<GameObject> arrowQueue = new Queue<GameObject>();
    public GameObject arrow;
    public GameObject wind;
    public void LoadSource()
    {
        Singleton<ScoreController>.Instance.ClearScore();
        if (target != null)
            Destroy(target);
        target = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Target"));
        if (arrow != null)
            Singleton<ArrowFactory>.Instance.FreeArrow(arrow);
        while (arrowQueue.Count != 0)
            Singleton<ArrowFactory>.Instance.FreeArrow(arrowQueue.Dequeue());
        arrowQueue.Clear();
        arrow = Singleton<ArrowFactory>.Instance.GetArrow();
        arrowQueue.Enqueue(arrow);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<ArrowFactory>();
        this.gameObject.AddComponent<ScoreController>();
        this.gameObject.AddComponent<IGUI>();
        this.gameObject.AddComponent<WindController>();
        wind = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Wind"));
        Director.GetInstance().CurrentSceneController = this;
        Director.GetInstance().CurrentSceneController.LoadSource();
    }

    // Update is called once per frame
    void Update()
    {
        if (arrow.tag == "LOST")
        {
            arrow = Singleton<ArrowFactory>.Instance.GetArrow();
            arrowQueue.Enqueue(arrow);
        }
    }
}