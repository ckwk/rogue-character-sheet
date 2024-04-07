using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StatButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InputField stat;
    private Dropdown typeDice;
    private InputField numDice;
    private GameObject diceMenu,
        diceSwipeBar,
        swipeToRoll,
        diceButton;
    private float _touchTimer = 0f,
        _secondsUntilEdit = 1f;
    private bool pointerDown = false,
        hasNotVibrated = true;
    private string[] dice =
    {
        "1d4",
        "1d6",
        "1d7",
        "1d8",
        "1d10",
        "2d6",
        "2d7",
        "2d8",
        "2d10",
        "3d7"
    };

    private void Awake()
    {
        numDice = GameObject.Find("NumDice").GetComponent<InputField>();
        typeDice = GameObject.Find("TypeDice").transform.GetComponent<Dropdown>();
        diceMenu = GameObject.Find("DiceMenu");
        diceSwipeBar = GameObject.Find("DiceSwipeBar");
        swipeToRoll = GameObject.Find("Swipe");
        diceButton = GameObject.Find("DiceButton");
    }

    private void Update()
    {
        if (!pointerDown)
            return;
        _touchTimer += Time.deltaTime;
        if (_touchTimer > _secondsUntilEdit && hasNotVibrated)
        {
            Vibration.Vibrate(50);
            hasNotVibrated = false;
        }
    }

    public void OpenDiceRollerForStat()
    {
        if (Int32.Parse(stat.text) - 1 >= 10)
            return;
        diceMenu.SetActive(true);
        diceSwipeBar.SetActive(true);
        swipeToRoll.SetActive(true);
        diceButton.SetActive(false);
        var die = dice[Int32.Parse(stat.text) - 1];
        print(die);
        numDice.text = die[0].ToString();
        var index = 0;
        foreach (var option in typeDice.options)
        {
            if (option.text == die.Substring(2))
                typeDice.value = index;
            index++;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_touchTimer > _secondsUntilEdit)
        {
            stat.Select();
        }
        else
            OpenDiceRollerForStat();

        hasNotVibrated = true;
        pointerDown = false;
        _touchTimer = 0;
    }
}
