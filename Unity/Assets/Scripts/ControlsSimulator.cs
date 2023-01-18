using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Linq;

public class ControlsSimulator : MonoBehaviour
{
    private const int NUM_SAVE_KEYS = 4;
    private const char NULL_KEY = '-';
    public bool InCall { get; private set; }
    private bool checkNeeds;

    [SerializeField]
    private PilotsManager manager;
    [SerializeField]
    private TextMeshProUGUI numbersPressed;
    
    private char[] lastKeysPressed;

    //private Pilot manager.PilotOnCall;
    
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
        checkNeeds = false;
        ResetRegister();
    }

    private void ResetRegister()
    {
        for(int i=0; i < NUM_SAVE_KEYS; i++)
            lastKeysPressed[i] = NULL_KEY;
    }

    void Update()
    {
        string keysPressed = "";
        for(int i=0; i < NUM_SAVE_KEYS; i++)
            keysPressed += lastKeysPressed[i];

        numbersPressed.text = keysPressed;

        /*if (!keysPressed.Contains(NULL_KEY) && !InCall)
        {
            if (CheckIfCalled(keysPressed))
            {
                Debug.Log("Called " + keysPressed);
                Debug.Log("Hi, this is " + manager.PilotOnCall.PilotName + " from the "
                    + manager.PilotOnCall.Airplane.AirplaneName + ", Requesting to "
                    + manager.PilotOnCall.Need.ToString());
                InCall = true;
                ResetRegister();
            }
        }else */if (InCall && !keysPressed.Contains(NULL_KEY)) 
        {
            if(!checkNeeds)
                CheckCommand(keysPressed);
            else
                CheckNeeds(keysPressed);
        }
    }

    [Button]
    public void PickUpPhone()
    {
        manager.PilotOnCall = manager.Calling.First();
        Debug.Log("Hi, this is " + manager.PilotOnCall.PilotName + " from the "
                + manager.PilotOnCall.Airplane.AirplaneName + ", Requesting to "
                + manager.PilotOnCall.Need.ToString());
        InCall = true;
        ResetRegister();
    }

    private bool CheckIfCalled(string number)
    {
        foreach (Pilot pilot in manager.OnAir)
            if (pilot.Number == number) 
            {
                manager.PilotOnCall = pilot;
                return true;
            } 
        foreach (Pilot pilot in manager.OnRunway)
            if (pilot.Number == number)
            {
                manager.PilotOnCall = pilot;
                return true;
            }
        foreach (Pilot pilot in manager.OnGarage)
            if (pilot.Number == number)
            {
                manager.PilotOnCall = pilot;
                return true;
            }

        return false;
    }

    private void CheckCommand(string number)
    {
        switch (number)
        {
            case "1000":
                Debug.Log("Repeat Request");
                Debug.Log("I want to " + manager.PilotOnCall.Need.ToString());
                break;
            case "2000":
                Debug.Log("Ask name of airplane");
                Debug.Log("I'm piloting the " + manager.PilotOnCall.Airplane.AirplaneName);
                break;
            case "3000":
                Debug.Log("Confirm request");
                if(manager.PilotOnCall.Need == Request.Park)
                    checkNeeds = true;
                else
                {
                    if(manager.PilotOnCall.Need == Request.Land)
                        manager.TryToLand(manager.PilotOnCall);

                    InCall = false;
                    manager.PilotOnCall = null;
                }
                break;
            case "4000":
                InCall = false;
                manager.PilotOnCall = null;
                Debug.Log("Deny Request");
                break;
            default:
                Debug.Log("I don't recognize that order");
                break;
        }
        ResetRegister();
    }

    private void CheckNeeds(string orderDetails)
    {
        switch(orderDetails)
        {
            case "*100":
                Debug.Log("Go to the 1st garage.");
                break;
            case "*200":
                Debug.Log("Go to the 2nd garage.");
                break;
            case "*300":
                Debug.Log("Go to the 3rd garage.");
                break;
            case "*400":
                Debug.Log("Go to the 4th garage.");
                break;
            case "*500":
                Debug.Log("Go to the 5th garage.");
                break;
            default:
                Debug.Log("I don't recognize that order");
                break;
        }
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
