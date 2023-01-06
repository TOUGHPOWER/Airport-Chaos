using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilot
{
    public string PilotName {get;}
    public AirplaneData Airplane{get;}
    public string Number{get;}
    public Request Need { get; set; }
    
    public Pilot(AirplaneData data, List<string> numbersInUse, string[] firstNames, string[] lastNames)
    {
        Airplane = data;

        string number = "";
        do
        {
            int numberInt = Random.Range(10000, 100000);
            number = numberInt.ToString();
        }while(numbersInUse.Contains(number));

        Number = number;

        PilotName = firstNames[Random.Range(0, firstNames.Length)] + " " + lastNames[Random.Range(0, lastNames.Length)];

        Need = Request.Land;
    }
}
