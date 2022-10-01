using static GameManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EditTextBox : MonoBehaviour {
    private GameManager _gm;
    public TextBox textBox;

    public void EditTextbox() {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        var textField = _gm.inField;
        textField.Select();
        textField.text = textBox.gameObject.GetComponent<Text>().text;
        textBox.enabled = true;
        _gm.activeTextBox = textBox;
    }

    public void PasteIntoBio() {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        textBox.gameObject.GetComponent<Text>().text = GUIUtility.systemCopyBuffer;
        _gm.SetCharacterBioAndNotes();
    }
}
