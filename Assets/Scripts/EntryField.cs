using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EntryField : MonoBehaviour
{
    public HandleEntries handler,
        parent;
    public int index;
    public Text myText;
    private GameManager _gm;
    private Transform textBox;

    private void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        textBox = _gm.textBox.transform;
        handler = parent.handler;
    }

    public void OpenTextBox()
    {
        textBox.gameObject.SetActive(true);
        _gm.textBoxActivator = gameObject;
        var details = handler.serializedEntries[parent.index].Split('|').ToList();

        Debug.Log(handler.transform.parent.name);
        switch (handler.transform.parent.name)
        {
            case "Traits":
                details = details.GetRange(0, 2);
                break;
            case "EquipmentContainer":
                details = details.GetRange(0, 3);
                break;
            case "Armour":
                details = details.GetRange(0, 4);
                break;
            case "Weapons":
                details = details.GetRange(0, 5);
                break;
        }

        textBox.GetChild(2).GetComponent<Text>().text = details[0] + " — Effects";
        textBox.GetChild(1).GetComponent<InputField>().text = details.Last();
    }

    public void UpdateHandler(bool fromTextBox)
    {
        var properties = handler.serializedEntries[parent.index].Split('|');
        var textBoxText = textBox.GetChild(1).GetComponent<InputField>().text;
        var textToCompare = fromTextBox ? textBoxText : myText.text;
        if (textToCompare != properties[index])
        {
            properties[index] = textToCompare;
            handler.serializedEntries[parent.index] = string.Join("|", properties);
            _gm.SetCharacterArmour();
            _gm.SetCharacterEquipment();
            _gm.SetCharacterSpells();
            _gm.SetCharacterTraits();
            _gm.SetCharacterWeapons();
            _gm.SetCharacterCandM();
            _gm.SetCharacterBioAndNotes();
        }
    }
}
