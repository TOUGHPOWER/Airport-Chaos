using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pilot
{
    public string PilotName {get;}
    public AirplaneData Airplane{get;}
    public string Number{get;}
    public Request Need { get; set; }
    public ScenePilot PilotInScene{ get; private set; }

    public Pilot(AirplaneData data, List<string> numbersInUse, string[] firstNames, string[] lastNames, ScenePilot pilotPrefab, Vector3 position2Spawn)
    {
        Airplane = data;

        string number = "";
        do
        {
            int numberInt = Random.Range(1000, 10000);
            number = numberInt.ToString();
        }while(numbersInUse.Contains(number));

        Number = number;

        PilotName = firstNames[Random.Range(0, firstNames.Length)] + " " + lastNames[Random.Range(0, lastNames.Length)];

        Need = Request.Land;

        PilotInScene = MonoBehaviour.Instantiate(pilotPrefab, position2Spawn, new Quaternion());

        PilotInScene.SetAirplaneSprite(Airplane.Sprite);
    }
}
