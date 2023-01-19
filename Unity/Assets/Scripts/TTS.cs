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
    private Text text;


    public void Say(string message)
    {
        text.text = message;

        element.SelectItem(true);
    }    
}
