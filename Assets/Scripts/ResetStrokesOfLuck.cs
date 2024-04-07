using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetStrokesOfLuck : MonoBehaviour
{
    public InputField SOL;

    public void OnTap()
    {
        SOL.text = "3";
    }
}
