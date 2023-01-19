using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class costumSelect : MonoBehaviour
{
    public void Select(UAP_BaseElement element)
    {
        element.SelectItem(true);
    }
}
