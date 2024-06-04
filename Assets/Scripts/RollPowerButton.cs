using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameManager;

public class RollPowerButton : MonoBehaviour
{
    public InputField powerNameField,
        powerBonusField,
        uiResultNum;
    private GameObject powerScreen;
    private GameManager gm;
    private TMP_Text uiName,
        uiCost,
        uiDuration,
        uiCastingTime,
        uiResult;
    private Text castingStat,
        mutationStat;
    private int levelOfCastingStat;
    private int roll = 0;
    private int[] numDicePerLevel = { 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4 };
    private int[] diceTypePerLevel = { 4, 6, 7, 8, 10, 6, 7, 8, 10, 7, 8, 10, 8, 10 };

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        powerScreen = gm.powerScreen;

        uiName = gm.powerScreenName;
        uiCost = gm.powerScreenCost;
        uiDuration = gm.powerScreenDuration;
        uiCastingTime = gm.powerScreenCastingTime;
        uiResult = gm.powerScreenResult;
        uiResultNum = gm.powerScreenResultNum;
        castingStat = transform.parent.parent.GetChild(2).GetChild(0).GetComponent<Text>();
    }

    public void RollPower(int powerType)
    {
        var power = new TSV("default");
        switch (powerType)
        {
            case 0:
                power = spellObjects.Find(p => p.GetName() == powerNameField.text);
                RollStat(castingStat.text);
                break;
            case 1:
                power = mutationsObjects.Find(p => p.GetName() == powerNameField.text);
                RollStat(mutationStat.text);
                break;
        }
        print(power.GetName());
        uiResultNum.gameObject.GetComponent<PowerResultNumber>().currentPower = power;

        powerScreen.SetActive(true);
        uiName.text = power.GetName();
        uiCost.text = power.GetProperties()[0][0].Replace(": ", ":\n");
        // [0][1] contains range which is skipped over
        uiDuration.text = power.GetProperties()[0][2].Replace(": ", ":\n");
        uiCastingTime.text = power.GetProperties()[0][3].Replace(": ", ":\n");
        uiResultNum.text = roll.ToString();
        uiResult.text = gm.GetPowerResultText(power, roll);
    }

    private void RollStat(string stat)
    {
        roll = 0;
        switch (stat)
        {
            case "INT":
                levelOfCastingStat = int.Parse(gm.intelligence.text);
                break;
            case "CHA":
                levelOfCastingStat = int.Parse(gm.charisma.text);
                break;
            case "STR":
                levelOfCastingStat = int.Parse(gm.strength.text);
                break;
            case "DEX":
                levelOfCastingStat = int.Parse(gm.dexterity.text);
                break;
            case "END":
                levelOfCastingStat = int.Parse(gm.endurance.text);
                break;
        }

        var castingBonus = int.Parse(powerBonusField.text.Substring(1));
        levelOfCastingStat += powerBonusField.text.StartsWith("-") ? -castingBonus : castingBonus;

        var numDiceToRoll =
            levelOfCastingStat < numDicePerLevel.Length
                ? numDicePerLevel[levelOfCastingStat - 1]
                : levelOfCastingStat - 10;
        var diceTypeToRoll =
            levelOfCastingStat < diceTypePerLevel.Length
                ? diceTypePerLevel[levelOfCastingStat - 1]
                : 10;
        print(numDiceToRoll + "d" + diceTypeToRoll);
        for (int i = 0; i < numDiceToRoll; i++)
        {
            var newRoll = Random.Range(1, diceTypeToRoll + 1);
            if (newRoll == diceTypeToRoll)
                newRoll += Random.Range(1, diceTypeToRoll + 1);
            print(newRoll);
            roll += newRoll;
        }
    }

    //method for updating the result text is in the GameManager
}
