using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ControlsSimulator : MonoBehaviour
{
    private const int NUM_SAVE_KEYS = 5;
    private const char NULL_KEY = 'n';

    [SerializeField]
    private string[] phoneNumbers;
    
    private char[] lastKeysPressed;
    
    
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
        print(keyspressed);
        print("--------------------");
        if(CheckIfCalled(0))
        {
            string phone = "Called: " + phoneNumbers[0];
            
            
            print(phone);
        }


    }

    private bool CheckIfCalled(int index)
    {
        bool called = (phoneNumbers[index].CompareTo(new string(lastKeysPressed)) == 0);

        if(called)
            ResetRegister();

        return called;
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
