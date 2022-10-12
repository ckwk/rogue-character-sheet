using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StatButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public InputField stat;
    private Text typeDice;
    private InputField numDice;
    private GameObject diceMenu, diceSwipeBar, swipeToRoll, diceButton;
    private float _touchTimer = 0f, _secondsUntilEdit = 1f, vibrateTimer = 0f, _stopVibrate = 0.005f;
    private bool pointerDown = false, hasNotVibrated = true;
    private string[] dice = {"1d4", "1d6", "1d7", "1d8", "1d10", "2d6", "2d7", "2d8", "2d10", "3d7"};

    private void Awake() {
        numDice = GameObject.Find("Number").GetComponent<InputField>();
        typeDice = GameObject.Find("TypeDice").transform.GetChild(0).GetComponent<Text>();
        diceMenu = GameObject.Find("DiceMenu");
        diceSwipeBar = GameObject.Find("DiceSwipeBar");
        swipeToRoll = GameObject.Find("Swipe");
        diceButton = GameObject.Find("DiceButton");
    }

    private void Update() {
        if (!pointerDown) return;
        _touchTimer += Time.deltaTime;
        if (_touchTimer > _secondsUntilEdit && hasNotVibrated) {
            hasNotVibrated = false;
            StartCoroutine(HapticPulse());
        }
    }

    public void OpenDiceRollerForStat() {
        if (Int32.Parse(stat.text) - 1 >= 10) return;
        diceMenu.SetActive(true);
        diceSwipeBar.SetActive(true);
        swipeToRoll.SetActive(true);
        diceButton.SetActive(false);
        var die = dice[Int32.Parse(stat.text)-1];
        numDice.text = die[0].ToString();
        typeDice.text = die.Substring(2);

    }


    public void OnPointerDown(PointerEventData eventData) {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (_touchTimer > _secondsUntilEdit) {
            stat.Select();
        } else OpenDiceRollerForStat();

        hasNotVibrated = true;
        pointerDown = false;
        _touchTimer = 0;
    }

    IEnumerator HapticPulse() {
        while (vibrateTimer < _stopVibrate) {
            vibrateTimer += Time.deltaTime;
            Handheld.Vibrate();
            yield return null;
        }
    }
}
