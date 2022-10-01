using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour {
    public Text statDice;
    private Text typeDice;
    private InputField numDice;
    private GameObject diceMenu, diceSwipeBar, swipeToRoll;

    private void Awake() {
        numDice = GameObject.Find("Number").GetComponent<InputField>();
        typeDice = GameObject.Find("TypeDice").transform.GetChild(0).GetComponent<Text>();
        diceMenu = GameObject.Find("DiceMenu");
        diceSwipeBar = GameObject.Find("DiceSwipeBar");
        swipeToRoll = GameObject.Find("Swipe");
    }

    public void OpenDiceRollerForStat() {
        diceMenu.SetActive(true);
        diceSwipeBar.SetActive(true);
        swipeToRoll.SetActive(true);
        numDice.text = statDice.text[0].ToString();
        typeDice.text = statDice.text.Substring(2);
    }
}
