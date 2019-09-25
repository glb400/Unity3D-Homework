using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referee : MonoBehaviour
{
    public SSDirector sd;

    public void Check(List<GameObject> PriestsAtRight, List<GameObject> DevilsAtRight, List<GameObject> PriestsAtLeft, List<GameObject> DevilsAtLeft, List<GameObject> BodyOnBoat)
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
                if (((PriestsAtRight.Count + BoatPriest) != 0 && ((PriestsAtRight.Count + BoatPriest) < (DevilsAtRight.Count + BoatDevil))) | (PriestsAtLeft.Count != 0 && PriestsAtLeft.Count < DevilsAtLeft.Count))
                    sd.state = State.lose;
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sd = SSDirector.getInstance();
        sd.referee = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
