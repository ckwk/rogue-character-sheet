using static GameManager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDice : MonoBehaviour {
    private Text _stat, _myText;
    private List<string> _dice = new List<string>{"1d4", "1d6", "1d7", "1d8", "1d10", "2d6", "2d7", "2d8", "2d10", "3d7"};

    private void Start() {
        _stat = transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        _myText = GetComponent<Text>();
    }

    private void Update() {
        if (Every32Frames) _myText.text = _dice[int.Parse(_stat.text)-1];
    }
}
