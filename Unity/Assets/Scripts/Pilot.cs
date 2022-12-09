using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilot
{
    public string PilotName {get;}
    public AirplaneData Airplane{get;}
    public string Number{get;}
    
    public Pilot(AirplaneData data, List<string> numbersInUse, string[] names)
    {
        Airplane = data;

        string number = "";
        do
        {
            int numberInt = Random.Range(1000, 10000);
            number = numberInt.ToString();
        }while(numbersInUse.Contains(number));

        Number = number;

        PilotName = names[Random.Range(0, names.Length)];
    }
}
