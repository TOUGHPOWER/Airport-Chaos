using UnityEngine;

[CreateAssetMenu(menuName ="Pilot")]
public class PilotData : ScriptableObject
{
    [field: SerializeField] public string           PilotName{get; private set;}
    [field: SerializeField] public AirplaneData     Airplane{get; private set;}
    [field: SerializeField] public string           PhoneNumber{get; private set;}
}
