using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PhoneInputs : MonoBehaviour
{
    [SerializeField]
    private bool usingCostumController;
    [SerializeField]
    private SerialController serialController;
    [SerializeField]
    private Controller controller;
    [SerializeField]
    private bool testing;
    private bool ringing;
    private bool phoneUp;

    [ButtonGroup("1"), LabelText("1")]
    private void B1(){OnMessageArrived("1");}

    [ButtonGroup("1"), LabelText("2")]
    private void B2(){OnMessageArrived("2");}
    [ButtonGroup("1"), LabelText("3")]
    private void B3(){OnMessageArrived("3");}
    [ButtonGroup("2"), LabelText("4")]
    private void B4(){OnMessageArrived("4");}
    [ButtonGroup("2"), LabelText("5")]
    private void B5(){OnMessageArrived("5");}
    [ButtonGroup("2"), LabelText("6")]
    private void B6(){OnMessageArrived("6");}
    [ButtonGroup("3"), LabelText("7")]
    private void B7(){OnMessageArrived("7");}
    [ButtonGroup("3"), LabelText("8")]
    private void B8(){OnMessageArrived("8");}
    [ButtonGroup("3"), LabelText("9")]
    private void B9(){OnMessageArrived("9");}
    [ButtonGroup("4"), LabelText("*")]
    private void BAsterisco(){OnMessageArrived("*");}
    [ButtonGroup("4"), LabelText("0")]
    private void B0(){OnMessageArrived("0");}
    [ButtonGroup("4"), LabelText("#")]
    private void BCardinal(){OnMessageArrived("#");}
    [Button, LabelText("Pick phone")]
    private void PhoneUp(){OnMessageArrived("up");}
    [Button, LabelText("Drop phone")]
    private void PhoneDown(){OnMessageArrived("down");}
    [Button, LabelText("Ring")]
    private void Ring(){serialController.SendSerialMessage("ring");}
    [Button, LabelText("Stop")]
    private void Stop(){serialController.SendSerialMessage("stop");}

    void OnMessageArrived(string msg)
    {
        switch(msg)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "*":
            case "#":
                controller.Pressed(msg[0]);
                break;
            case "up":
                serialController.SendSerialMessage("stop");
                phoneUp = true;
                controller.PickUpPhone();
                break;
            case "down":
                phoneUp = false;
                controller.PutPhoneDown();
                break;
        }
        Debug.Log("Message arrived: " + msg);
    }

    private void Start()
    {
        phoneUp = false;
    }

    private void Update()
    {

        if(controller.Ringing && !ringing && !phoneUp)
        {
            print("ring");
            ringing = true;
            serialController.SendSerialMessage("ring");
        }else if(!controller.Ringing || phoneUp)
            ringing = false;

        if(testing && controller.InCall)
        {
            if(Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                OnMessageArrived("0");
            }else if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                OnMessageArrived("1");
            }else if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                OnMessageArrived("2");
            }else if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                OnMessageArrived("3");
            }else if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                OnMessageArrived("4");
            }else if(Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                OnMessageArrived("5");
            }else if(Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                OnMessageArrived("6");
            }else if(Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            {
                OnMessageArrived("7");
            }else if(Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                OnMessageArrived("8");
            }else if(Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
            {
                OnMessageArrived("9");
            }else if(Input.GetKeyDown(KeyCode.A))
            {
                OnMessageArrived("*");
            }else if(Input.GetKeyDown(KeyCode.S))
            {
                OnMessageArrived("#");
            }
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            OnMessageArrived("up");
            
        }else if(Input.GetKeyDown(KeyCode.P))
        {
            OnMessageArrived("down");
            phoneUp = false;
        }
    }

}
