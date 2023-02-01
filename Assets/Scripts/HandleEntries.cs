using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleEntries : MonoBehaviour
{
    private int _numEntries;
    private float offset = 0.1f * Screen.height,
        noteOffset = 0.03f * Screen.height;
    private GameManager _gm;
    private string defaultEntry = "||||";
    public int index = -1;
    public List<string> serializedEntries;
    public List<RectTransform> _entries;
    public GameObject entry;
    public List<RectTransform> fieldsBelow;
    public HandleEntries handler;
    public VerticalScroll scroller;

    private void Awake()
    {
        serializedEntries = new List<string> { defaultEntry };
        _numEntries = 1;
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        var parentName = transform.parent.name;
        noteOffset = parentName == "Notes" ? noteOffset : 0;
        offset =
            parentName.Contains("Spell") || parentName.Contains("Mutation")
                ? 0.05f * Screen.height
                : offset;
    }

    public void Start()
    {
        //serializedEntries = new List<string> {"|||"};
        if (!handler)
            return;
        handler._entries.Add(gameObject.GetComponent<RectTransform>());
        if (handler.fieldsBelow.Count < 1)
            scroller.lowestEntry = gameObject.GetComponent<RectTransform>();
    }

    // used by the top level add button
    public void CreateEntry()
    {
        var newEntry = Instantiate(entry, transform.parent, false);
        var entryScript = newEntry.transform.GetChild(1).GetComponent<HandleEntries>();
        entryScript.handler = this;
        entryScript.scroller = scroller;
        entryScript.index = _numEntries;
        serializedEntries.Add(defaultEntry);

        var rTrans = newEntry.GetComponent<RectTransform>();
        var anchoredPos = rTrans.anchoredPosition;
        anchoredPos = new Vector2(
            anchoredPos.x,
            anchoredPos.y - (_numEntries * offset + noteOffset)
        );
        rTrans.anchoredPosition = anchoredPos;

        foreach (var field in fieldsBelow)
        {
            anchoredPos = field.anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x, anchoredPos.y - offset);
            field.anchoredPosition = anchoredPos;
        }

        scroller.numEntries++;
        _numEntries++;
    }

    public void LoadEntry(string[] parameters)
    {
        var newEntry = Instantiate(entry, transform.parent, false);
        var fieldIndex = 0;
        foreach (Transform field in newEntry.transform)
        {
            if (field == newEntry.transform.GetChild(1) || field.name == "Roll")
                continue;
            var fieldText = field.GetChild(0).GetComponent<InputField>();
            if (fieldIndex < parameters.Length)
                fieldText.text = parameters[fieldIndex];
            fieldIndex++;
        }
        var entryScript = newEntry.transform.GetChild(1).GetComponent<HandleEntries>();
        entryScript.handler = this;
        entryScript.scroller = scroller;
        entryScript.index = _numEntries;

        var rTrans = newEntry.GetComponent<RectTransform>();
        var anchoredPos = rTrans.anchoredPosition;
        anchoredPos = new Vector2(anchoredPos.x, anchoredPos.y - _numEntries * offset);
        rTrans.anchoredPosition = anchoredPos;

        foreach (var field in fieldsBelow)
        {
            anchoredPos = field.anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x, anchoredPos.y - offset);
            field.anchoredPosition = anchoredPos;
        }

        scroller.numEntries++;
        _numEntries++;
    }

    // used by the entries themselves (in the delete button)
    public void DeleteEntry()
    {
        print(transform.parent.parent.position);
        if (handler.serializedEntries[index] != defaultEntry)
        {
            SetEverything();
        }
        handler.serializedEntries.RemoveAt(index);
        RemoveEntryForLoadAndOtherThings();
        if (scroller.numEntries <= scroller.entriesUntilScroll)
            scroller.ScrollUp();
        print(transform.parent.parent.position);
    }

    public void RemoveEntryForLoadAndOtherThings()
    {
        var myTrans = gameObject.GetComponent<RectTransform>();
        var below = false;
        handler._numEntries--;
        scroller.numEntries--;
        foreach (var e in handler._entries)
        {
            if (e == myTrans)
            {
                below = true;
            }
            else if (!below || e.name != "Delete")
            {
                continue;
            }
            e.gameObject.GetComponent<HandleEntries>().index--;
            var anchoredPos = e.parent.GetComponent<RectTransform>().anchoredPosition;
            e.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                anchoredPos.x,
                anchoredPos.y + offset
            );
        }

        foreach (var field in handler.fieldsBelow)
        {
            var anchoredPos = field.anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x, anchoredPos.y + offset);
            field.anchoredPosition = anchoredPos;
        }

        handler._entries.Remove(myTrans);
        Destroy(transform.parent.gameObject);
    }

    public void SetEverything()
    {
        _gm.SetCharacterArmour();
        _gm.SetCharacterEquipment();
        _gm.SetCharacterSpells();
        _gm.SetCharacterTraits();
        _gm.SetCharacterWeapons();
        _gm.SetCharacterCandM();
        _gm.SetCharacterBioAndNotes();
    }

    // Used by top level button to load entrys
    public void LoadEntries()
    {
        transform.parent
            .GetChild(2)
            .GetChild(1)
            .GetComponent<HandleEntries>()
            .RemoveEntryForLoadAndOtherThings();
        if (serializedEntries == null)
            serializedEntries = new List<string> { defaultEntry };
        foreach (var e in serializedEntries)
        {
            var mutableEntry = e;
            for (var i = 1; i < 4; i++)
            {
                if (e.Substring(e.Length - i)[0] != '|')
                {
                    if (i == 1)
                        break;
                    mutableEntry = e.Remove(e.Length - (i - 1));
                    break;
                }

                mutableEntry = e.Remove(e.Length - 3);
            }
            LoadEntry(mutableEntry.Split('|'));
        }
    }
}
