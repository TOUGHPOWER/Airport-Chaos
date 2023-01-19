using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TTS : MonoBehaviour
{
    [SerializeField]
    private UAP_BaseElement element;
    [SerializeField]
    private UAP_AccessibilityManager manager;
    [SerializeField]
    private Text text;
    public string CurrentMessage{ get => text.text; }

    private void Sart()
    {
        manager.m_DefaultState = true;
    }


    public void Say(string message)
    {
        text.text = message;

        element.SelectItem(true);
    }    
}
