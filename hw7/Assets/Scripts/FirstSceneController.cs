using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController
{
    public PropFactory patrol_factory;                               
    public ScoreRecorder recorder;                                   
    public PatrolActionManager action_manager;                       
    public int wall_sign = -1;                                       
    public GameObject player;                                        
    public Camera main_camera;                                       
    public float player_speed = 5;                                   
    public float rotate_speed = 135f;                                
    private List<GameObject> patrols;                                
    private bool game_over = false;                                  

    void Update()
    {
        for (int i = 0; i < patrols.Count; i++)
            patrols[i].gameObject.GetComponent<PatrolData>().wall_sign = wall_sign;
    }
    void Start()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        patrol_factory = Singleton<PropFactory>.Instance;
        action_manager = gameObject.AddComponent<PatrolActionManager>() as PatrolActionManager;
        LoadResources();
        main_camera.GetComponent<CameraFlow>().follow = player;
        recorder = Singleton<ScoreRecorder>.Instance;
    }

    public void LoadResources()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Plane"));
        player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(0, 9, 0), Quaternion.identity) as GameObject;
        patrols = patrol_factory.GetPatrols();
        for (int i = 0; i < patrols.Count; i++)
        {
            action_manager.GoPatrol(patrols[i]);
        }
    }
    public void MovePlayer(float translationX, float translationZ)
    {
        if(!game_over)
        {
            if (translationX != 0 || translationZ != 0)
                player.GetComponent<Animator>().SetBool("run", true);
            else
                player.GetComponent<Animator>().SetBool("run", false);
            player.transform.Translate(0, 0, translationZ * player_speed * Time.deltaTime);
            player.transform.Rotate(0, translationX * rotate_speed * Time.deltaTime, 0);
            if (player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0)
                player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
            if (player.transform.position.y != 0)
                player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);     
        }
    }

    public int GetScore()
    {
        return recorder.GetScore();
    }

    public bool GetGameover()
    {
        return game_over;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    void OnEnable()
    {
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameoverChange += Gameover;
    }
    void OnDisable()
    {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameoverChange -= Gameover;
    }
    void AddScore()
    {
        recorder.AddScore();
    }
    void Gameover()
    {
        game_over = true;
        patrol_factory.StopPatrol();
        action_manager.DestroyAllAction();
    }
}
