using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EntryField : MonoBehaviour {
    public HandleEntries handler, parent;
    public int index;
    public Text myText;
    private GameManager _gm;

    private void Start() {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        handler = parent.handler;
    }

    public void UpdateHandler() {
        var properties = handler.serializedEntries[parent.index].Split('|');
        if (myText.text != properties[index]) {
            properties[index] = myText.text;
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
