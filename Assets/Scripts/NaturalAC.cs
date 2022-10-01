using static GameManager;
using UnityEngine;
using UnityEngine.UI;

public class NaturalAC : MonoBehaviour {
    private Text dex, myText;

    private void Start() {
        dex = GameObject.Find("Dexterity").transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        myText = GetComponent<Text>();
    }

    private void Update() {
        if (Every32Frames) {
            myText.text = dex.text;
        }
    }
}
