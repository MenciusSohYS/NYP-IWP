using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBossScript : MonoBehaviour
{    public enum Phase
    {
        Phase0,
        Phase1,
        Phase2,
        Phase3,
        Phase4
    }

    [SerializeField] Phase CurrentPhaseNumber;
    [SerializeField] Phase PreviousPhaseNumber;

    private void Start()
    {
        CurrentPhaseNumber = Phase.Phase0;
        PreviousPhaseNumber = Phase.Phase0;
    }

    public Phase ReturnCurrentPhaseNumber()
    {
        return CurrentPhaseNumber;
    }

    public bool CheckForPhaseChange(int Health)
    {
        if (Health > 950) //technically phase0
            return false;

        if (Health > 900) //phase depends on hp
        {
            ++CurrentPhaseNumber;
            if (CurrentPhaseNumber > Phase.Phase1)
                CurrentPhaseNumber = Phase.Phase1;
        }
        else if (Health > 750)
        {
            ++CurrentPhaseNumber;
            if (CurrentPhaseNumber > Phase.Phase2)
                CurrentPhaseNumber = Phase.Phase2;
        }
        else if (Health > 500)
        {
            ++CurrentPhaseNumber;
            if (CurrentPhaseNumber > Phase.Phase3)
                CurrentPhaseNumber = Phase.Phase3;
        }
        else if (Health > 250)
        {
            ++CurrentPhaseNumber;
            if (CurrentPhaseNumber > Phase.Phase4)
                CurrentPhaseNumber = Phase.Phase4;
        }

        if (CurrentPhaseNumber != PreviousPhaseNumber)
        {
            PreviousPhaseNumber = CurrentPhaseNumber;
            return true;
        }
        return false;
    }
}
