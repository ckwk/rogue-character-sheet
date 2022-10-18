using static GameManager;
using UnityEngine;
using UnityEngine.UI;

public class NaturalAC : MonoBehaviour {
    private InputField dex, myText;

    private void Start() {
        dex = GameObject.Find("Dexterity").transform.GetChild(0).GetComponent<InputField>();
        myText = GetComponent<InputField>();
    }

    private void Update() {
        if (Every32Frames) {
            myText.text = dex.text;
        }
    }
}
