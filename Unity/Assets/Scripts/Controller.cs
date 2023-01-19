using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Linq;

public class Controller : MonoBehaviour
{
    private const int NUM_SAVE_KEYS = 4;
    private const char NULL_KEY = '-';
    public bool InCall { get; private set; }
    private bool checkNeeds;

    [field: SerializeField]
    public PilotsManager Manager { get; private set;}
[SerializeField]
    private TextMeshProUGUI numbersPressed;
    [SerializeField] private TTS tts;
    public bool Ringing{ get => (Manager.Calling.Count > 0 && !InCall)||
        (Manager.Calling.Count > 1 && InCall); }
    private char[] lastKeysPressed;


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
        
        if(!Manager.FinishGame)
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
    }

    public void PickUpPhone()
    {
        if(Ringing)
        {
            Manager.PilotOnCall = Manager.Calling.First();
            string text = "Hi, this is " + Manager.PilotOnCall.PilotName + " from the "
                    + Manager.PilotOnCall.Airplane.AirplaneName + ", Requesting to "
                    + Manager.PilotOnCall.Need.ToString();
            Debug.Log(text);
            if(tts != null)
                tts.Say(text);
            InCall = true;
            ResetRegister();
        }
    }
     public void PutPhoneDown()
     {

     }

    private bool CheckIfCalled(string number)
    {
        foreach (Pilot pilot in Manager.OnAir)
            if (pilot.Number == number) 
            {
                Manager.PilotOnCall = pilot;
                return true;
            } 
        foreach (Pilot pilot in Manager.OnRunway)
            if (pilot.Number == number)
            {
                Manager.PilotOnCall = pilot;
                return true;
            }
        foreach (Pilot pilot in Manager.OnGarage)
            if (pilot.Number == number)
            {
                Manager.PilotOnCall = pilot;
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
                string text = "I want to " + Manager.PilotOnCall.Need.ToString();
                Debug.Log(text);
                if(tts != null)
                    tts.Say(text);
                Debug.Log(text);
                break;
            case "2000":
                Debug.Log("Ask name of airplane");
                string text1 = "I'm piloting the " + Manager.PilotOnCall.Airplane.AirplaneName;
                Debug.Log(text1);
                if(tts != null)
                    tts.Say(text1);
                Debug.Log(text1);
                break;
            case "3000":
                Debug.Log("Confirm request");
                if(Manager.PilotOnCall.Need == Request.Park)
                {
                    checkNeeds = true;
                    if(tts != null)
                        tts.Say("In what garage can I park?");
                }
                else
                {
                    if(Manager.PilotOnCall.Need == Request.Land)
                        Manager.TryToLand(Manager.PilotOnCall);
                    else
                        Manager.TryToTakeOf(Manager.PilotOnCall);
                    if(tts != null)
                        tts.Say("Roger");
                    Manager.Calling.Remove(Manager.PilotOnCall);
                    InCall = false;
                    Manager.PilotOnCall = null;
                }
                break;
            case "4000":
                InCall = false;
                Manager.DenyRequest(Manager.PilotOnCall);
                Manager.PilotOnCall = null;
                Debug.Log("Deny Request");
                if(tts != null)
                    tts.Say("Ok, I'll wait for now");
                
                break;
            default:
                if(tts != null)
                    tts.Say("I don't recognize that order");
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
            case "*200":
            case "*300":
            case "*400":
            case "*500":
                Manager.TryToPark(Manager.PilotOnCall,int.Parse(orderDetails[1].ToString())-1);
                Debug.Log("Go to the garage.");
                if(tts != null)
                    tts.Say("Roger");
                Manager.Calling.Remove(Manager.PilotOnCall);
                InCall = false;
                Manager.PilotOnCall = null;
                checkNeeds = false;
                break;
            default:
                if(tts != null)
                    tts.Say("I don't recognize that order");
                Debug.Log("I don't recognize that order");
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
