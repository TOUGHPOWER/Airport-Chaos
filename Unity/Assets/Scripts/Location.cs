using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public struct Location
{
    public Pilot PilotInLocation{get; private set;}
    
    [SerializeField] private string name;
    [SerializeField] private Classe classeRestriction;
    public bool IsOccupied => (PilotInLocation != null);

    /*[SerializeField, ShowIf("IsOccupied"), InfoBox("Occupied by:"), ReadOnly]
    private string OcupiedName;*/

    public (bool, bool) TryToMovePilotIn(Pilot pilot)
    {
        bool canMoveIn = false;
        bool crash = false;
        
        if(IsOccupied)
        {
            crash = true;
        }else if(((byte)pilot.Airplane.classe) <= ((byte)classeRestriction))
        {
            PilotInLocation = pilot;

            canMoveIn = true;
            crash = false;
        }

        return (canMoveIn, crash);
    }

    public void RemovePilot()=>PilotInLocation = null;
}
