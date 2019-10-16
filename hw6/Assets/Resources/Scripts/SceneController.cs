using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum State { RUNNING, LOSE, STOPPED, WIN, RESET };
public class SceneController : MonoBehaviour, ISceneController, IUserAction
{
    public IActionManager actionManager;
    public DiskFactory factory;
    public Recorder scoreRecorder;
    public int round = 0;
    public Text RoundText;
    public Text GameText;
    public Text FinalText;
    
    // public int game;
    public State state;
    public int num = 0;
    GameObject explosion;
    // Use this for initialization
    void Awake()
    //创建导演实例并载入资源
    {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentScenceController = this;
        director.currentScenceController.LoadResources();
        round = 1;
        state = State.RESET;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RoundText.text = "Round:" + round.ToString();
        // detect whether game over
        if(state == State.LOSE)
            FinalText.text = "Game Over";
    }

    public IEnumerator waitForOneSecond()
    {
        // count represents rest time before game start
        int count = 3;
        while (count >= 0 && state == State.RESET)
        {
            print(count);
            yield return new WaitForSeconds(1);
            count--;
        }
        state = State.RUNNING;
    }
    public void Begin()
    {
        num = 0;
        if (state == State.RESET)
        {
            StartCoroutine(waitForOneSecond());
        }
    }
    public void ReStart()
    {
        // when to restart
        // just to reload scence can be good
        SceneManager.LoadScene("sample");
        state = State.LOSE;
    }
    public void hit()
    {
        if (state == State.RUNNING)
        {
            // add hit event when OnMouseDown
            // detect event in View
            // and respond to event in realization of User action interface - in sceneController
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Disk")
                {
                    explosion.transform.position = hit.collider.gameObject.transform.position;
                    // display particle system
                    explosion.GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    explosion.GetComponent<ParticleSystem>().Play();
                    hit.collider.gameObject.SetActive(false);
                    hit.collider.gameObject.GetComponent<DiskAttribute>().hit = true;
                    scoreRecorder.hit();
                }
            }
        }
    }
    public void LoadResources()
    {
        // use this to create particle system
        explosion = Instantiate(Resources.Load("Prefabs/Explosion"), new Vector3(-40, 0, 0), Quaternion.identity) as GameObject;
        Instantiate(Resources.Load("Prefabs/Light"));
    }
}

