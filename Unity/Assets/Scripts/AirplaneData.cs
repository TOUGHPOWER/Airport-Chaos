using UnityEngine;

[CreateAssetMenu(menuName ="Airplane")]
public class AirplaneData : ScriptableObject
{
    [field: SerializeField] public Classe       classe{get; private set;}
    [field: SerializeField] public string       AirplaneName{get; private set;}
    [field: SerializeField] public Sprite       Sprite;
}
