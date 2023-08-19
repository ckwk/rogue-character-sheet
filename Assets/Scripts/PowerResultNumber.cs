using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerResultNumber : MonoBehaviour
{
    [HideInInspector]
    public TSV currentPower;
    private GameManager gm;
    private TMP_Text result;
    private InputField resultNum;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        result = gm.powerScreenResult;
        resultNum = gm.powerScreenResultNum;
    }

    public void UpdatePowerResultText()
    {
        result.text = gm.GetPowerResultText(currentPower, int.Parse(resultNum.text));
    }
}
