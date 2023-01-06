using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class ControlsSimulator : MonoBehaviour
{
    private const int NUM_SAVE_KEYS = 5;
    private const char NULL_KEY = '-';
    public bool InCall { get; private set; }

    [SerializeField]
    private PilotsManager manager;
    [SerializeField]
    private TextMeshProUGUI numbersPressed;
    
    private char[] lastKeysPressed;

    private Pilot pilotOncALL;
    
    [ButtonGroup("1"), LabelText("1")]
    private void B1(){Pressed('1');}

    [ButtonGroup("1"), LabelText("2")]
    private void B2(){Pressed('2');}
    [ButtonGroup("1"), LabelText("3")]
    private void B3(){Pressed('3');}
    [ButtonGroup("2"), LabelText("4")]
    private void B4(){Pressed('4');}
    [ButtonGroup("2"), LabelText("5")]
    private void B5(){Pressed('5');}
    [ButtonGroup("2"), LabelText("6")]
    private void B6(){Pressed('6');}
    [ButtonGroup("3"), LabelText("7")]
    private void B7(){Pressed('7');}
    [ButtonGroup("3"), LabelText("8")]
    private void B8(){Pressed('8');}
    [ButtonGroup("3"), LabelText("9")]
    private void B9(){Pressed('9');}
    [ButtonGroup("4"), LabelText("*")]
    private void BAsterisco(){Pressed('*');}
    [ButtonGroup("4"), LabelText("0")]
    private void B0(){Pressed('0');}
    [ButtonGroup("4"), LabelText("#")]
    private void BCardinal(){Pressed('#');}

    private void Start()
    {
        lastKeysPressed = new char[NUM_SAVE_KEYS];
        InCall = false;
        ResetRegister();
    }

    private void ResetRegister()
    {
        for(int i=0; i < NUM_SAVE_KEYS; i++)
            lastKeysPressed[i] = NULL_KEY;
    }

    void Update()
    {
        string keyspressed = "";
        for(int i=0; i < NUM_SAVE_KEYS; i++)
            keyspressed += lastKeysPressed[i];

        numbersPressed.text = keyspressed;

        if (!keyspressed.Contains(NULL_KEY) && !InCall)
        {
            if (CheckIfCalled(keyspressed))
            {
                Debug.Log("Called " + keyspressed);
                Debug.Log("Hi, this is " + pilotOncALL.PilotName + " from the "
                    + pilotOncALL.Airplane.AirplaneName + ", Requesting to "
                    + pilotOncALL.Need.ToString());
                InCall = true;
                ResetRegister();
            }
        }else if (InCall && !keyspressed.Contains(NULL_KEY)) 
        {
            CheckComand(keyspressed);
        }
    }

    private bool CheckIfCalled(string number)
    {
        foreach (Pilot pilot in manager.OnAir)
            if (pilot.Number == number) 
            {
                pilotOncALL = pilot;
                return true;
            } 
        foreach (Pilot pilot in manager.OnRunway)
            if (pilot.Number == number)
            {
                pilotOncALL = pilot;
                return true;
            }
        foreach (Pilot pilot in manager.OnGarage)
            if (pilot.Number == number)
            {
                pilotOncALL = pilot;
                return true;
            }

        return false;
    }

    private void CheckComand(string number)
    {
        switch (number)
        {
            case "10000":
                Debug.Log("Repeat Request");
                Debug.Log("I want to " + pilotOncALL.Need.ToString());
                break;
            case "20000":
                Debug.Log("Ask name of airplane");
                Debug.Log("I'm piloting the " + pilotOncALL.Airplane.AirplaneName);
                break;
            case "30000":
                InCall = false;
                pilotOncALL = null;
                Debug.Log("Confirm request");
                break;
            case "40000":
                InCall = false;
                pilotOncALL = null;
                Debug.Log("Deny Request");
                break;
            default:
                break;
        }
        ResetRegister();
    }

    public void Pressed(char key)
    {
        for(int i = 0; i < NUM_SAVE_KEYS; i++)
        {
            if(i > 0)
            {
                lastKeysPressed[i-1] = lastKeysPressed[i];
            }
        }
        lastKeysPressed[NUM_SAVE_KEYS-1] = key;
    }
}
