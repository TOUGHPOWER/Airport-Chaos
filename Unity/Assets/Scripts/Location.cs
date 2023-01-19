using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public struct Location
{
    public Pilot PilotInLocation { get; set; }

    [field: SerializeField] public string Name{ get; private set; }
    [SerializeField] private Classe classeRestriction;
    [field: SerializeField] public Transform Position{ get; set; }

/*[SerializeField, ShowIf("IsOccupied"), InfoBox("Occupied by:"), ReadOnly]
private string OcupiedName;*/

public bool TryToMovePilotIn(Pilot pilot)
    {
        bool canMoveIn = false;
        
        if(pilot.Airplane.classe <= classeRestriction)
        {
            PilotInLocation = pilot;
            MonoBehaviour.print(Name);
            canMoveIn = true;
        }

        return canMoveIn;
    }

    public void RemovePilot()=>PilotInLocation = null;
}
