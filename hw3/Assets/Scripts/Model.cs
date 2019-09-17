using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    List<GameObject> PriestsAtLeft = new List<GameObject>();
    List<GameObject> PriestsAtRight = new List<GameObject>();
    List<GameObject> DevilsAtLeft = new List<GameObject>();
    List<GameObject> DevilsAtRight = new List<GameObject>();
    List<GameObject> BodyOnBoat = new List<GameObject>();
    GameObject BoatInstance;
    bool IsEmpty = false;
    bool IsAvailable = true;
    int VacantPos = 0;

    public float speed = 10.0f;
    SSDirector sd;

    Vector3 BoatLeft = new Vector3(-3.5f,0.25f,0.0f);
    Vector3 BoatRight = new Vector3(3.5f, 0.25f, 0.0f);

    Vector3 CoastLeft = new Vector3(-5.5f, 1.05f, 0.0f);
    Vector3 CoastRight = new Vector3(5.5f, 1.05f, 0.0f);

    void Render()
    {
        Instantiate(Resources.Load("Prefabs/LeftCoast") as GameObject);
        Instantiate(Resources.Load("Prefabs/RightCoast") as GameObject);
        Instantiate(Resources.Load("Prefabs/River") as GameObject);
        BoatInstance = Instantiate(Resources.Load("Prefabs/Boat") as GameObject);

        int[] index = { 1, 2, 3 };
        foreach (int i in index)
        {
            PriestsAtRight.Add(Instantiate(Resources.Load("Prefabs/Priest" + i.ToString()) as GameObject));
            DevilsAtRight.Add(Instantiate(Resources.Load("Prefabs/Devil" + i.ToString()) as GameObject));
        }
    }

    void DetectBoat(out bool IsAvailable, out int VacantPos, out bool IsEmpty)
    {
        IsEmpty = false;
        IsAvailable = false;
        VacantPos = 0;
        
        if (BodyOnBoat.Count < 2)
        {
            if (BodyOnBoat.Count == 0)
                IsEmpty = true;
            VacantPos = BodyOnBoat.Count;
            IsAvailable = true;
        }
    }

    void DynamicPosition()
    {
        foreach (GameObject priest in PriestsAtLeft)
            priest.transform.position = CoastLeft + new Vector3( - (DevilsAtLeft.Count + PriestsAtLeft.IndexOf(priest)) * 1.3f,0.0f,0.0f);
        foreach (GameObject priest in PriestsAtRight)
            priest.transform.position = CoastRight + new Vector3((DevilsAtRight.Count + PriestsAtRight.IndexOf(priest)) * 1.3f, 0.0f, 0.0f);
        foreach (GameObject devil in DevilsAtLeft)
            devil.transform.position = CoastLeft + new Vector3( - DevilsAtLeft.IndexOf(devil) * 1.3f, 0.0f, 0.0f);
        foreach (GameObject devil in DevilsAtRight)
            devil.transform.position = CoastRight + new Vector3(DevilsAtRight.IndexOf(devil) * 1.3f, 0.0f, 0.0f);
    }

    void GetOn(GameObject passenger)
    {
        passenger.transform.parent = BoatInstance.transform;
        DetectBoat(out IsAvailable, out VacantPos, out IsEmpty);
        if (IsAvailable)
        {
            int flag = sd.state == State.r ? 1 : -1 ;
            passenger.transform.position = new Vector3( flag * (0.5f + (VacantPos + 1) * 2.0f), 0.8f, 0.0f);
            BodyOnBoat.Add(passenger);
        }
    }

    public void Cruise()
    {
        DetectBoat(out IsAvailable, out VacantPos, out IsEmpty);
        if (IsEmpty == false)
            sd.state = sd.state == State.r ? State.rtl : State.ltr;
    }

    public void DownBoard()
    {
        foreach (GameObject passenger in BodyOnBoat)
            if (passenger != null)
            {
                passenger.transform.parent = null;
                if (sd.state == State.r)
                    if (passenger.name[0] == 'P')
                        PriestsAtRight.Add(passenger);
                    else
                        DevilsAtRight.Add(passenger);
                else
                    if (passenger.name[0] == 'P')
                        PriestsAtLeft.Add(passenger);

                    else
                        DevilsAtLeft.Add(passenger);
            }
        BodyOnBoat.RemoveAll(f => { return true; });
        DynamicPosition();
    }

    public void PriestToGetOn()
    {
        switch (sd.state)
        {
            case State.l:
                {
                    DetectBoat(out IsAvailable, out VacantPos, out IsEmpty);
                    if (IsAvailable && PriestsAtLeft.Count != 0)
                        foreach (GameObject priest in PriestsAtLeft)
                        {
                            if (priest != null)
                            {
                                GetOn(priest);
                                PriestsAtLeft.Remove(priest);
                                break;
                            }
                        }
                    break;
                }
            case State.r:
                {
                    DetectBoat(out IsAvailable, out VacantPos, out IsEmpty);
                    if (IsAvailable && PriestsAtRight.Count != 0)
                        foreach (GameObject priest in PriestsAtRight)
                        {
                            if (priest != null)
                            {
                                GetOn(priest);
                                PriestsAtRight.Remove(priest);
                                break;
                            }
                        }
                    break;
                }
        }
    }

    public void DevilToGetOn()
    {
        switch (sd.state)
        {
            case State.l:
                    DetectBoat(out IsAvailable, out VacantPos, out IsEmpty);
                    if (IsAvailable && DevilsAtLeft.Count != 0)
                        foreach (GameObject devil in DevilsAtLeft)
                        {
                            if (devil != null)
                            {
                                GetOn(devil);
                                DevilsAtLeft.Remove(devil);
                                break;
                            }
                        }
                    break;
            case State.r:
                    DetectBoat(out IsAvailable, out VacantPos, out IsEmpty);
                    if (IsAvailable && DevilsAtRight.Count != 0)
                        foreach (GameObject devil in DevilsAtRight)
                        {
                            if (devil != null)
                            {
                                GetOn(devil);
                                DevilsAtRight.Remove(devil);
                                break;
                            }
                        }
                    break;
        }
    }

    public void Restart()
    {
        sd.state = State.r;
        DownBoard();
        BoatInstance.transform.position = BoatRight;

        for (int i = 0;i < PriestsAtLeft.Count; i ++)
        {
            PriestsAtRight.Add(PriestsAtLeft[i]);
        }

        PriestsAtLeft.RemoveAll(f => { return true; });

        for (int i = 0; i < DevilsAtLeft.Count; i++)
        {
            DevilsAtRight.Add(DevilsAtLeft[i]);
        }

        DevilsAtLeft.RemoveAll(f => { return true; });

        foreach (GameObject priest in PriestsAtRight)
            priest.transform.position = new Vector3(9.4f + PriestsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f);

        foreach (GameObject priest in DevilsAtRight)
            priest.transform.position = new Vector3(5.5f + DevilsAtRight.IndexOf(priest) * 1.3f, 1.05f, 0.0f);
    }

    void Check()
    {

        if (PriestsAtLeft.Count == 3 && DevilsAtLeft.Count == 3)
        {
            sd.state = State.win;
            return;
        }

        int BoatPriest = 0;
        int BoatDevil = 0;

        foreach (GameObject body in BodyOnBoat)
            if (body.name[0] == 'P')
                BoatPriest++;
            else if (body.name[0] == 'D')
                BoatDevil++;

        switch (sd.state)
        {
            case State.l:
                    if (((PriestsAtLeft.Count + BoatPriest) != 0 && ((PriestsAtLeft.Count + BoatPriest) < (DevilsAtLeft.Count + BoatDevil))) | (PriestsAtRight.Count != 0 && PriestsAtRight.Count < DevilsAtRight.Count))
                        sd.state = State.lose;
                    break;
            case State.r:
                    if (((PriestsAtRight.Count + BoatPriest) != 0 && ((PriestsAtRight.Count + BoatPriest) < (DevilsAtRight.Count + BoatDevil)))| (PriestsAtLeft.Count != 0 && PriestsAtLeft.Count < DevilsAtLeft.Count))
                        sd.state = State.lose;
                    break;
        }

    }

    // Start is called before the first frame update
    public void Start()
    {
        sd = SSDirector.getInstance();
        sd.setModel(this);
        Render();
    }

    // Update is called once per frame
    public void Update()
    {
        switch (sd.state)
        {
            case State.ltr:
                    BoatInstance.transform.position = Vector3.MoveTowards(BoatInstance.transform.position, BoatRight, Time.deltaTime * speed);
                    if (BoatInstance.transform.position == BoatRight)
                    {
                        sd.state = State.r;
                    }
                    break;
            case State.rtl:
                    BoatInstance.transform.position = Vector3.MoveTowards(BoatInstance.transform.position, BoatLeft, Time.deltaTime * speed);
                    if (BoatInstance.transform.position == BoatLeft)
                    {
                        sd.state = State.l;
                    }
                    break;
            default:
                    Check();
                    break;
        }
    }
}
