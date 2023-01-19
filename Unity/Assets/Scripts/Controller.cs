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

    [SerializeField]
    private PilotsManager manager;
    [SerializeField]
    private TextMeshProUGUI numbersPressed;
    [SerializeField] private TTS tts;
    public bool Ringing{ get => (manager.Calling.Count > 0 && !InCall)||
        (manager.Calling.Count > 1 && InCall); }
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

    public void PickUpPhone()
    {
        if(Ringing)
        {
            manager.PilotOnCall = manager.Calling.First();
            string text = "Hi, this is " + manager.PilotOnCall.PilotName + " from the "
                    + manager.PilotOnCall.Airplane.AirplaneName + ", Requesting to "
                    + manager.PilotOnCall.Need.ToString();
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
                string text = "I want to " + manager.PilotOnCall.Need.ToString();
                Debug.Log(text);
                if(tts != null)
                    tts.Say(text);
                Debug.Log(text);
                break;
            case "2000":
                Debug.Log("Ask name of airplane");
                string text1 = "I'm piloting the " + manager.PilotOnCall.Airplane.AirplaneName;
                Debug.Log(text1);
                if(tts != null)
                    tts.Say(text1);
                Debug.Log(text1);
                break;
            case "3000":
                Debug.Log("Confirm request");
                if(manager.PilotOnCall.Need == Request.Park)
                {
                    checkNeeds = true;
                    if(tts != null)
                        tts.Say("In what garage can I park?");
                }
                else
                {
                    if(manager.PilotOnCall.Need == Request.Land)
                        manager.TryToLand(manager.PilotOnCall);
                    else
                        manager.TryToTakeOf(manager.PilotOnCall);
                    if(tts != null)
                        tts.Say("Roger");
                    manager.Calling.Remove(manager.PilotOnCall);
                    InCall = false;
                    manager.PilotOnCall = null;
                }
                break;
            case "4000":
                InCall = false;
                manager.DenyRequest(manager.PilotOnCall);
                manager.PilotOnCall = null;
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
                manager.TryToPark(manager.PilotOnCall,int.Parse(orderDetails[1].ToString())-1);
                Debug.Log("Go to the garage.");
                if(tts != null)
                    tts.Say("Roger");
                manager.Calling.Remove(manager.PilotOnCall);
                InCall = false;
                manager.PilotOnCall = null;
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
